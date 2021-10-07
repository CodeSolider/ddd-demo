using DotNetCore.CAP;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayPlatform.Application.Dtos;
using EbayPlatform.Application.Dtos.Orders;
using EbayPlatform.Application.Quartz.Commons;
using EbayPlatform.Application.Services;
using EbayPlatform.Domain.IntegrationEvents;
using EbayPlatform.Infrastructure.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz
{
    /// <summary>
    /// 下载所有订单数据
    /// </summary>
    public class GetAllOrderListJob : BaseTaskJob, IEbayCollection, IDependency
    {
        private readonly IOrderAppService _orderAppService;
        public GetAllOrderListJob(IOrderAppService orderAppService, ISyncTaskJobAppService syncTaskJobAppService,
            ICapPublisher capPublisher) : base(syncTaskJobAppService, capPublisher)
        {
            _orderAppService = orderAppService;
        }

        /// <summary>
        /// 接收处理数据
        /// </summary>
        /// <param name="integrationEvent"></param>
        [CapSubscribe(name: nameof(GetAllOrderListJob))]
        public async Task ReceiveCapMQEventAsync(CollectionIntegrationEvent integrationEvent)
        {
            var apiCall = this.BeforeRequest(integrationEvent.ParamValue);
            var apiResult = await DownloadDataAsync(apiCall).ConfigureAwait(false);
            await SaveDataAsync(integrationEvent, apiResult).ConfigureAwait(false);
        }

        /// <summary>
        /// 保存结果
        /// </summary>
        /// <param name="integrationEvent"></param>
        /// <param name="apiResult"></param>
        /// <returns></returns>
        public async Task SaveDataAsync(CollectionIntegrationEvent integrationEvent, ApiResult apiResult)
        {
            if (apiResult.Code == 200 && apiResult is ApiResult<ParamValueToEntityDto<List<OrderDto>>> responseData)
            {
                var orderIdList = responseData.Data.Data.Select(o => o.OrderID);
                if (orderIdList.Any())
                {
                    await _orderAppService.DeleteOrderByIdsAsync(orderIdList).ConfigureAwait(false);
                }
                await _orderAppService.AddOrderAsync(responseData.Data.Data).ConfigureAwait(false);
                //存储结果集
                await base.SaveResultAsync<List<OrderDto>>(nameof(GetAllOrderListJob), integrationEvent.ShopName, apiResult).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 请求前
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public ApiCall BeforeRequest(string paramValue)
        {
            ParamValueToEntityDto paramValueEntityDto = JsonConvert.DeserializeObject<ParamValueToEntityDto>(paramValue);
            return new eBay.Service.Call.GetOrdersCall
            {
                Site = SiteCodeType.US,
                CreateTimeFrom = paramValueEntityDto.FromDate,
                CreateTimeTo = paramValueEntityDto.ToDate,
                OrderStatus = OrderStatusCodeType.All,
                Pagination = new PaginationType
                {
                    EntriesPerPage = paramValueEntityDto.PageSize,
                    PageNumber = paramValueEntityDto.PageIndex,
                },
                DetailLevelList = new List<DetailLevelCodeType>
                {
                    DetailLevelCodeType.ReturnAll
                }
            };
        }

        /// <summary>
        /// 下载请求
        /// </summary>
        /// <param name="apiCall"></param>
        /// <returns></returns>
        public Task<ApiResult> DownloadDataAsync(ApiCall apiCall)
        {
            return Task.Run(() =>
            {
                var getOrdersCall = apiCall as eBay.Service.Call.GetOrdersCall;
                bool hasMoreOrders = false;

                List<OrderDto> orderDtoList = new();
                do
                {
                    getOrdersCall.Execute();
                    hasMoreOrders = getOrdersCall.ApiResponse.HasMoreOrders.GetValueOrDefault();
                    getOrdersCall.Pagination.PageNumber++;
                    if (getOrdersCall.OrderList.Any())
                    {
                        orderDtoList.AddRange(ConvertData(getOrdersCall.OrderList));
                    }

                } while (hasMoreOrders);

                return ApiResult.OK("下载订单数据成功", new ParamValueToEntityDto<List<OrderDto>>
                {
                    FromDate = getOrdersCall.CreateTimeTo,
                    ToDate = DateTime.Now,
                    PageIndex = (getOrdersCall?.Pagination?.PageNumber).GetValueOrDefault(),
                    PageSize = 100,
                    Data = orderDtoList
                });
            });
        }


        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="orderTypeList"></param>
        /// <returns></returns>
        private List<OrderDto> ConvertData(List<OrderType> orderTypeList)
        {
            List<OrderDto> orderDtoList = new();
            orderTypeList.ForEach(orderType =>
            {
                OrderDto orderDto = new OrderDto
                {
                    OrderID = orderType.OrderID,
                    SellerEmail = orderType.SellerEmail,
                    SellerUserID = orderType.SellerUserID,
                    SyncDate = DateTime.Now
                };

                if (orderType.OrderStatus.HasValue)
                {
                    orderDto.OrderStatus = Convert.ToString(orderType.OrderStatus);
                }

                if (orderType.AdjustmentAmount != null)
                {
                    orderDto.AdjustmentAmountValue = (decimal)orderType.AdjustmentAmount.Value;
                    orderDto.AdjustmentAmountCurrency = orderType.AdjustmentAmount.currencyID.ToString();
                }

                if (orderType.AmountPaid != null)
                {
                    orderDto.AmountPaidValue = (decimal)orderType.AmountPaid.Value;
                    orderDto.AmountPaidCurrency = orderType.AmountPaid.currencyID.ToString();
                }

                if (orderType.AmountSaved != null)
                {
                    orderDto.AmountSavedValue = (decimal)orderType.AmountSaved.Value;
                    orderDto.AmountSavedCurrency = orderType.AmountSaved.currencyID.ToString();
                }

                if (orderType.Total != null)
                {
                    orderDto.TotalValue = (decimal)orderType.Total.Value;
                    orderDto.TotalCurrency = orderType.Total.currencyID.ToString();
                }

                if (orderType.Subtotal != null)
                {
                    orderDto.SubtotalValue = (decimal)orderType.Subtotal.Value;
                    orderDto.SubtotalCurrency = orderType.Subtotal.currencyID.ToString();
                }

                if (orderType.PaymentMethods.Any())
                {
                    orderDto.PaymentMethods = string.Join(",", orderType.PaymentMethods.Select(o => o.ToString()));
                }

                if (orderType.CreatedTime.HasValue)
                {
                    orderDto.CreatedTime = orderType.CreatedTime.Value.ToLocalTime();
                }

                if (orderType.CheckoutStatus != null)
                {
                    if (orderType.CheckoutStatus.eBayPaymentStatus.HasValue)
                    {
                        orderDto.EBayPaymentStatus = Convert.ToString(orderType.CheckoutStatus.eBayPaymentStatus);
                    }

                    if (orderType.CheckoutStatus.PaymentMethod.HasValue)
                    {
                        orderDto.PaymentMethod = Convert.ToString(orderType.CheckoutStatus.PaymentMethod);
                    }

                    if (orderType.CheckoutStatus.Status.HasValue)
                    {
                        orderDto.Status = Convert.ToString(orderType.CheckoutStatus.Status);
                    }

                    orderDto.IntegratedMerchantCreditCardEnabled = orderType.CheckoutStatus.IntegratedMerchantCreditCardEnabled;
                    if (orderType.CheckoutStatus.PaymentInstrument.HasValue)
                    {
                        orderDto.PaymentInstrument = Convert.ToString(orderType.CheckoutStatus.PaymentInstrument);
                    }
                    orderDto.LastModifiedTime = orderType.CheckoutStatus.LastModifiedTime;
                }

                if (orderType.ShippingAddress != null)
                {
                    orderDto.ShippingAddress = new AddressDto
                    {
                        AddressID = orderType.ShippingAddress.AddressID,
                        Name = orderType.ShippingAddress.Name,
                        Street = orderType.ShippingAddress.Street,
                        Street1 = orderType.ShippingAddress.Street1,
                        Street2 = orderType.ShippingAddress.Street2,
                        CityName = orderType.ShippingAddress.CityName,
                        StateOrProvince = orderType.ShippingAddress.StateOrProvince,
                        CountryName = orderType.ShippingAddress.CountryName,
                        Phone = orderType.ShippingAddress.Phone,
                        PostalCode = orderType.ShippingAddress.PostalCode,
                    };

                    if (orderType.ShippingAddress.Country.HasValue)
                    {
                        orderDto.ShippingAddress.Country = Convert.ToString(orderType.ShippingAddress.Country);
                    }
                    if (orderType.ShippingAddress.AddressOwner.HasValue)
                    {
                        orderDto.ShippingAddress.AddressOwner = Convert.ToString(orderType.ShippingAddress.AddressOwner);
                    }
                }

                //订单交易
                orderDto.OrderTransactions = orderType.TransactionArray.Select(transactionItem =>
                {
                    var orderTransactionDto = new OrderTransactionDto
                    {
                        TransactionID = transactionItem.TransactionID,
                        OrderLineItemID = transactionItem.OrderLineItemID,
                        CreatedDate = transactionItem.CreatedDate
                    };

                    if (transactionItem.TransactionSiteID.HasValue)
                    {
                        orderTransactionDto.SiteCode = Convert.ToString(transactionItem.TransactionSiteID);
                    }

                    if (transactionItem.Item != null)
                    {
                        orderTransactionDto.Title = transactionItem.Item.Title;
                        orderTransactionDto.ConditionID = transactionItem.Item.ConditionID;
                        orderTransactionDto.ConditionDisplayName = transactionItem.Item.ConditionDisplayName;
                    }

                    orderTransactionDto.QuantityPurchased = transactionItem.QuantityPurchased.GetValueOrDefault();

                    if (transactionItem.TransactionPrice != null)
                    {
                        orderTransactionDto.Value = (decimal)transactionItem.TransactionPrice.Value;
                        orderTransactionDto.Currency = Convert.ToString(transactionItem.TransactionPrice.currencyID);
                    }
                    if (transactionItem.Status != null)
                    {
                        if (transactionItem.Status.PaymentHoldStatus.HasValue)
                        {
                            orderTransactionDto.PaymentHoldStatus = Convert.ToString(transactionItem.Status.PaymentHoldStatus);
                        }

                        if (transactionItem.Status.InquiryStatus.HasValue)
                        {
                            orderTransactionDto.InquiryStatus = Convert.ToString(transactionItem.Status.InquiryStatus);
                        }

                        if (transactionItem.Status.ReturnStatus.HasValue)
                        {
                            orderTransactionDto.ReturnStatus = Convert.ToString(transactionItem.Status.ReturnStatus);
                        }
                    }

                    if (transactionItem.ShippingServiceSelected != null)
                    {
                        orderTransactionDto.ShippingServiceSelected = GetShippingServiceOption(transactionItem.ShippingServiceSelected);
                    }
                    return orderTransactionDto;
                }).ToList();

                //发货明细
                if (orderType.ShippingDetails != null)
                {
                    orderDto.ShippingDetail = new ShippingDetailDto
                    {
                        SellingManagerSalesRecordNumber = orderType.ShippingDetails.SellingManagerSalesRecordNumber,
                        GetItFast = orderType.ShippingDetails.GetItFast
                    };

                    if (orderType.ShippingDetails.SalesTax != null)
                    {
                        orderDto.ShippingDetail.SalesTaxPercent = orderType.ShippingDetails.SalesTax.SalesTaxPercent.GetValueOrDefault();
                        orderDto.ShippingDetail.SalesTaxState = orderType.ShippingDetails.SalesTax.SalesTaxState;

                        if (orderType.ShippingDetails.SalesTax.SalesTaxAmount != null)
                        {
                            orderDto.ShippingDetail.Value = (decimal)orderType.ShippingDetails.SalesTax.SalesTaxAmount.Value;
                            orderDto.ShippingDetail.Currency = Convert.ToString(orderType.ShippingDetails.SalesTax.SalesTaxAmount.currencyID);
                        }
                    }
                    orderDto.ShippingDetail.ShippingServiceOptions = orderType.ShippingDetails.ShippingServiceOptions.Select(shippingOptionItem =>
                    {
                        var shippingServiceOption = GetShippingServiceOption(shippingOptionItem);
                        return shippingServiceOption;
                    }).ToList();
                }

                if (orderType.ShippingServiceSelected != null)
                {
                    orderDto.ShippingServiceSelected = GetShippingServiceOption(orderType.ShippingServiceSelected);
                }
                orderDtoList.Add(orderDto);
            });
            return orderDtoList;
        }


        /// <summary>
        /// 获取发货选项信息
        /// </summary>
        /// <param name="serviceOptionsType"></param>
        /// <returns></returns>
        private ShippingServiceOptionDto GetShippingServiceOption(ShippingServiceOptionsType serviceOptionsType)
        {
            var shippingServiceOptionDto = new ShippingServiceOptionDto
            {
                ShippingService = serviceOptionsType.ShippingService,
                ShippingServicePriority = serviceOptionsType.ShippingServicePriority,
                ExpeditedService = serviceOptionsType.ExpeditedService,
                ShippingTimeMin = serviceOptionsType.ShippingTimeMin,
                ShippingTimeMax = serviceOptionsType.ShippingTimeMax
            };

            if (serviceOptionsType.ShippingServiceCost != null)
            {
                shippingServiceOptionDto.Value = (decimal)serviceOptionsType.ShippingServiceCost.Value;
                shippingServiceOptionDto.Currency = Convert.ToString(serviceOptionsType.ShippingServiceCost.currencyID);
            }

            shippingServiceOptionDto.ShippingPackages = serviceOptionsType.ShippingPackageInfo
                                                        .Select(packageItem => new ShippingPackageDto
                                                        {
                                                            StoreID = packageItem.StoreID,
                                                            ShippingTrackingEvent = packageItem.ShippingTrackingEvent,
                                                            ScheduledDeliveryTimeMin = packageItem.ScheduledDeliveryTimeMin,
                                                            ScheduledDeliveryTimeMax = packageItem.ScheduledDeliveryTimeMax,
                                                            ActualDeliveryTime = packageItem.ActualDeliveryTime,
                                                            EstimatedDeliveryTimeMin = packageItem.EstimatedDeliveryTimeMin,
                                                            EstimatedDeliveryTimeMax = packageItem.EstimatedDeliveryTimeMax,
                                                        }).ToList();

            return shippingServiceOptionDto;
        }
    }
}
