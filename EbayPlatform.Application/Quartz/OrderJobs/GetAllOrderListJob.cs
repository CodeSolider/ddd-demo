using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbayPlatform.Application.Dtos;
using EbayPlatform.Application.Quartz.Commons;
using EbayPlatform.Application.Services;
using EbayPlatform.Infrastructure.Core;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;

namespace EbayPlatform.Application.Quartz.OrderJobs
{
    /// <summary>
    /// 下载所有订单数据
    /// </summary>
    [DisallowConcurrentExecution]
    public class GetAllOrderListJob : AbstractEbayCollection, IJob, IDependency
    {
        private readonly IOrderAppService _orderAppService;
        public GetAllOrderListJob(ISyncTaskJobAppService syncTaskJobAppService,
            IOrderAppService orderAppService)
            : base(syncTaskJobAppService)
        {
            this._orderAppService = orderAppService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            await RunJobAsync(context.JobDetail.Key.Name).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        protected override ApiCall GetRequestApiCall(string paramValue)
        {
            AcquisitionTaskDto paramValueEntityDto = JsonConvert.DeserializeObject<AcquisitionTaskDto>(paramValue);
            eBay.Service.Call.GetOrdersCall getOrdersCall = new();
            getOrdersCall.Site = SiteCodeType.US;
            getOrdersCall.CreateTimeFrom = paramValueEntityDto.FromDate;
            getOrdersCall.CreateTimeTo = paramValueEntityDto.ToDate;
            getOrdersCall.OrderStatus = OrderStatusCodeType.All;
            getOrdersCall.Pagination = new PaginationType
            {
                EntriesPerPage = paramValueEntityDto.PageSize,
                PageNumber = paramValueEntityDto.PageIndex,
            };
            getOrdersCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            return getOrdersCall;
        }

        /// <summary>
        /// 获取下载数据
        /// </summary>
        /// <param name="apiCall"></param>
        /// <returns></returns>
        protected override Task<List<AbstractResponseType>> GetResponseListAsync(ApiCall apiCall)
        {
            return Task.Run(() =>
            {
                var getOrdersCall = apiCall as eBay.Service.Call.GetOrdersCall;
                List<AbstractResponseType> abstractResponseList = new();
                bool hasMoreOrders = false;
                do
                {
                    getOrdersCall.Execute();
                    hasMoreOrders = getOrdersCall.ApiResponse.HasMoreOrders.GetValueOrDefault();
                    getOrdersCall.Pagination.PageNumber++;

                    if (getOrdersCall.OrderList.Any())
                    {
                        abstractResponseList.Add(getOrdersCall.ApiResponse);
                    }
                } while (hasMoreOrders);

                return abstractResponseList;
            });
        }

        /// <summary>
        /// 保存下载的结果数据
        /// </summary>
        /// <param name="apiCall"></param>
        /// <param name="abstractResponseList"></param>
        /// <returns></returns>
        protected override async Task<string> SaveResponseResultAsync(ApiCall apiCall, List<AbstractResponseType> abstractResponseList)
        {
            var getOrdersResponseList = abstractResponseList.Cast<GetOrdersResponseType>();
            var responseOrderList = getOrdersResponseList.SelectMany(o => o.OrderArray);
            //先删除数据，再添加数据
            var orderIdList = responseOrderList.Select(o => o.OrderID);
            _ = await _orderAppService.GetOrderListByOrderIdsAsync(orderIdList);

            List<Domain.Models.Orders.Order> orderList = new();
            foreach (var orderType in responseOrderList)
            {
                //地址
                Domain.Models.Orders.Address shippingAddress = new(orderType.ShippingAddress?.AddressID, orderType.ShippingAddress?.Name,
                                                                   orderType.ShippingAddress?.Street, orderType.ShippingAddress?.Street1,
                                                                   orderType.ShippingAddress?.Street2, orderType.ShippingAddress?.CityName,
                                                                   orderType.ShippingAddress?.StateOrProvince, Convert.ToString(orderType.ShippingAddress?.Country),
                                                                   orderType.ShippingAddress?.CountryName, orderType.ShippingAddress?.Phone,
                                                                   orderType.ShippingAddress?.PostalCode, Convert.ToString(orderType.ShippingAddress?.AddressOwner));

                Domain.Models.Orders.Order orderItem = new(orderType.OrderID,
                                                           Convert.ToString(orderType.OrderStatus),
                                                           string.Join(",", orderType.PaymentMethods.Select(o => o.ToString())),
                                                           orderType.SellerEmail,
                                                           new Domain.Models.Orders.MoneyValue((decimal)orderType.AdjustmentAmount.Value, Convert.ToString(orderType.AdjustmentAmount.currencyID)),
                                                            new Domain.Models.Orders.MoneyValue((decimal)orderType.AmountPaid.Value, Convert.ToString(orderType.AmountPaid.currencyID)),
                                                            new Domain.Models.Orders.MoneyValue((decimal)orderType.AmountSaved.Value, Convert.ToString(orderType.AmountSaved.currencyID)),
                                                            new Domain.Models.Orders.MoneyValue((decimal)orderType.Total.Value, Convert.ToString(orderType.Total.currencyID)),
                                                            new Domain.Models.Orders.MoneyValue((decimal)orderType.Subtotal.Value, Convert.ToString(orderType.Subtotal.currencyID)),
                                                            shippingAddress, orderType.CreatedTime);


                if (orderType.CheckoutStatus != null)
                {
                    orderItem.AddCheckoutStatus(Convert.ToString(orderType.CheckoutStatus.eBayPaymentStatus), Convert.ToString(orderType.CheckoutStatus.PaymentMethod),
                                                Convert.ToString(orderType.CheckoutStatus.Status), orderType.CheckoutStatus.IntegratedMerchantCreditCardEnabled,
                                                Convert.ToString(orderType.CheckoutStatus.PaymentInstrument), orderType.CheckoutStatus.LastModifiedTime);
                }

                if (orderType.ShippingDetails != null)
                {

                    Domain.Models.Orders.SalesTax salesTax = new Domain.Models.Orders.SalesTax((orderType.ShippingDetails.SalesTax?.SalesTaxPercent).GetValueOrDefault(), orderType.ShippingDetails.SalesTax?.SalesTaxState);
                    orderItem.AddShippingDetail(salesTax, orderType.ShippingDetails.SellingManagerSalesRecordNumber,
                                                 orderType.ShippingDetails.GetItFast, orderType.ShippingDetails.ShippingServiceOptions.Adapt<List<Domain.Models.Orders.ShippingServiceOption>>());
                }
                orderList.Add(orderItem);
            }

            if (apiCall is eBay.Service.Call.GetOrdersCall getOrdersCall)
            {
                return JsonConvert.SerializeObject(new
                {
                    FromDate = getOrdersCall.CreateTimeTo,
                    ToDate = DateTime.Now,
                    PageIndex = getOrdersCall.Pagination.PageNumber,
                    PageSize = getOrdersCall.Pagination.EntriesPerPage,
                });
            }

            return string.Empty;
        }
    }
}
