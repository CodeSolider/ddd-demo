using EbayPlatform.Application.Dtos.Orders;
using EbayPlatform.Domain.Commands.Order;
using EbayPlatform.Infrastructure.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using System.Linq;
using System.Threading;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderAppService : IOrderAppService, IDependency
    {
        private readonly IMediator _mediator;
        public OrderAppService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 根据订单ID获取订单
        /// </summary>
        /// <param name="orderIdList"></param>
        /// <returns></returns>
        public Task<bool> DeleteOrderByIdsAsync(IEnumerable<string> orderIdList, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new OrderDeleteCommand(orderIdList), cancellationToken);
        }

        #region CreateOrder
        /// <summary>
        /// 添加订单数据
        /// </summary>
        /// <param name="orderDtos"></param>
        /// <returns></returns>
        public Task<bool> AddOrderAsync(List<OrderDto> orderDtos, CancellationToken cancellationToken = default)
        {
            List<Domain.Models.Orders.Order> orderList = new();
            orderDtos.ForEach(orderDtoItem =>
            {
                Domain.Models.Orders.CheckoutStatus checkoutStatus = new(orderDtoItem.EBayPaymentStatus, orderDtoItem.PaymentMethod, orderDtoItem.Status,
                orderDtoItem.IntegratedMerchantCreditCardEnabled, orderDtoItem.PaymentInstrument, orderDtoItem.LastModifiedTime);

                Domain.Models.Orders.Order orderItem = new(orderDtoItem.OrderID, orderDtoItem.OrderStatus, orderDtoItem.PaymentMethods,
                                                           orderDtoItem.SellerEmail, orderDtoItem.SellerUserID, new Domain.Models.MoneyValue(orderDtoItem.AdjustmentAmountValue, orderDtoItem.AdjustmentAmountCurrency),
                                                           new Domain.Models.MoneyValue(orderDtoItem.AmountPaidValue, orderDtoItem.AmountPaidCurrency),
                                                           new Domain.Models.MoneyValue(orderDtoItem.AmountSavedValue, orderDtoItem.AmountSavedCurrency),
                                                           new Domain.Models.MoneyValue(orderDtoItem.TotalValue, orderDtoItem.TotalCurrency),
                                                           new Domain.Models.MoneyValue(orderDtoItem.SubtotalValue, orderDtoItem.SubtotalCurrency),
                                                           checkoutStatus, orderDtoItem.ShippingAddress.Adapt<Domain.Models.Orders.Address>(), GetShippingServiceOption(orderDtoItem.ShippingServiceSelected),
                                                           orderDtoItem.CreatedTime);

                //订单交易
                orderDtoItem.OrderTransactions.ForEach(transactionItem =>
                {
                    orderItem.AddOrderTransaction(transactionItem.TransactionID, transactionItem.OrderLineItemID, transactionItem.SiteCode, transactionItem.Title, transactionItem.ConditionID,
                                                  transactionItem.ConditionDisplayName, transactionItem.QuantityPurchased,
                                                  new Domain.Models.MoneyValue(transactionItem.Value, transactionItem.Currency),
                                                  new Domain.Models.Orders.TransactionStatus(transactionItem.PaymentHoldStatus, transactionItem.InquiryStatus, transactionItem.ReturnStatus),
                                                  GetShippingServiceOption(transactionItem.ShippingServiceSelected), transactionItem.CreatedDate);
                });

                //添加发货详情信息
                orderItem.AddShippingDetail(new Domain.Models.Orders.SalesTax(orderDtoItem.ShippingDetail.SalesTaxPercent,
                                            orderDtoItem.ShippingDetail.SalesTaxState,
                                            new Domain.Models.MoneyValue(orderDtoItem.ShippingDetail.Value, orderDtoItem.ShippingDetail.Currency)),
                                            orderDtoItem.ShippingDetail.SellingManagerSalesRecordNumber, orderDtoItem.ShippingDetail.GetItFast,
                                            orderDtoItem.ShippingDetail.ShippingServiceOptions.Select(o => GetShippingServiceOption(o)).ToList());




                orderList.Add(orderItem);
            });

            return _mediator.Send(new OrderCreatedCommand(orderList), cancellationToken);
        }

        /// <summary>
        /// 发货打包信息
        /// </summary>
        /// <returns></returns>
        private Domain.Models.Orders.ShippingServiceOption GetShippingServiceOption(ShippingServiceOptionDto shippingServiceOptionDto)
        {
            var shippingServiceOption = new Domain.Models.Orders.ShippingServiceOption(shippingServiceOptionDto?.ShippingService,
                                                                                       new Domain.Models.MoneyValue((decimal)shippingServiceOptionDto?.Value, shippingServiceOptionDto?.Currency),
                                                                                       shippingServiceOptionDto?.ShippingServicePriority,
                                                                                       shippingServiceOptionDto?.ExpeditedService,
                                                                                       shippingServiceOptionDto?.ShippingTimeMin,
                                                                                       shippingServiceOptionDto?.ShippingTimeMax);

            //发货打包
            shippingServiceOptionDto.ShippingPackages.ForEach(pagckageItem =>
            {
                shippingServiceOption.ChangeShippingPackage(pagckageItem.StoreID, pagckageItem.ShippingTrackingEvent, pagckageItem.ScheduledDeliveryTimeMin, pagckageItem.ScheduledDeliveryTimeMax,
                                                            pagckageItem.EstimatedDeliveryTimeMin, pagckageItem.EstimatedDeliveryTimeMax);
            });
            return shippingServiceOption;
        }
        #endregion


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
