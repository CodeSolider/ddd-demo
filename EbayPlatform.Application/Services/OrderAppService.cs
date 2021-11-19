using EbayPlatform.Application.Dtos.Orders;
using EbayPlatform.Domain.Commands.Order;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using System.Linq;
using System.Threading;
using EbayPlatform.Domain.Core.Abstractions; 

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderAppService : IOrderAppService, IScopedDependency
    {
        private readonly IMediator _mediator;
        public OrderAppService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 根据订单ID删除订单信息
        /// </summary>
        /// <param name="orderIdList"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOrderByIdsAsync(IEnumerable<string> orderIdList, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new OrderDeleteCommand(orderIdList), cancellationToken);
        }

        #region CreateOrder

        /// <summary>
        /// 添加订单数据
        /// </summary>
        /// <param name="orderDtos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> AddOrderAsync(List<OrderDto> orderDtos, CancellationToken cancellationToken = default)
        {
            List<Domain.AggregateModel.OrderAggregate.Order> orderList = new();
            orderDtos.ForEach(orderDtoItem =>
            {
                Domain.AggregateModel.OrderAggregate.CheckoutStatus checkoutStatus = new(orderDtoItem.EBayPaymentStatus, orderDtoItem.PaymentMethod, orderDtoItem.Status,
                orderDtoItem.IntegratedMerchantCreditCardEnabled, orderDtoItem.PaymentInstrument, orderDtoItem.LastModifiedTime);

                Domain.AggregateModel.OrderAggregate.Order orderItem = new(orderDtoItem.OrderID, orderDtoItem.OrderStatus, orderDtoItem.ShopName, orderDtoItem.PaymentMethods,
                                                           orderDtoItem.SellerEmail, orderDtoItem.SellerUserID, new Domain.AggregateModel.MoneyValue(orderDtoItem.AdjustmentAmountValue, orderDtoItem.AdjustmentAmountCurrency),
                                                           new Domain.AggregateModel.MoneyValue(orderDtoItem.AmountPaidValue, orderDtoItem.AmountPaidCurrency),
                                                           new Domain.AggregateModel.MoneyValue(orderDtoItem.AmountSavedValue, orderDtoItem.AmountSavedCurrency),
                                                           new Domain.AggregateModel.MoneyValue(orderDtoItem.TotalValue, orderDtoItem.TotalCurrency),
                                                           new Domain.AggregateModel.MoneyValue(orderDtoItem.SubtotalValue, orderDtoItem.SubtotalCurrency),
                                                           checkoutStatus, orderDtoItem.ShippingAddress.Adapt<Domain.AggregateModel.OrderAggregate.Address>(), GetShippingServiceOption(orderDtoItem.ShippingServiceSelected),
                                                           orderDtoItem.CreatedTime);
                //订单交易
                orderDtoItem.OrderTransactions.ForEach(transactionItem =>
                {
                    orderItem.AddOrderTransaction(transactionItem.TransactionID, transactionItem.OrderLineItemID, transactionItem.SiteCode, transactionItem.Title, transactionItem.ConditionID,
                                                  transactionItem.ConditionDisplayName, transactionItem.QuantityPurchased,
                                                  new Domain.AggregateModel.MoneyValue(transactionItem.Value, transactionItem.Currency),
                                                  new Domain.AggregateModel.OrderAggregate.TransactionStatus(transactionItem.PaymentHoldStatus, transactionItem.InquiryStatus, transactionItem.ReturnStatus),
                                                  GetShippingServiceOption(transactionItem.ShippingServiceSelected), transactionItem.CreatedDate);
                });

                //添加发货详情信息
                orderItem.AddShippingDetail(new Domain.AggregateModel.OrderAggregate.SalesTax(orderDtoItem.ShippingDetail.SalesTaxPercent,
                                               orderDtoItem.ShippingDetail.SalesTaxState,
                                               new Domain.AggregateModel.MoneyValue(orderDtoItem.ShippingDetail.Value, orderDtoItem.ShippingDetail.Currency)),
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
        private Domain.AggregateModel.OrderAggregate.ShippingServiceOption GetShippingServiceOption(ShippingServiceOptionDto shippingServiceOptionDto)
        {
            var shippingServiceOption = new Domain.AggregateModel.OrderAggregate.ShippingServiceOption(shippingServiceOptionDto?.ShippingService,
                                                                                       new Domain.AggregateModel.MoneyValue((shippingServiceOptionDto?.Value).GetValueOrDefault(), shippingServiceOptionDto?.Currency),
                                                                                       shippingServiceOptionDto?.ShippingServicePriority,
                                                                                       shippingServiceOptionDto?.ExpeditedService,
                                                                                       shippingServiceOptionDto?.ShippingTimeMin,
                                                                                       shippingServiceOptionDto?.ShippingTimeMax,
                                                                                       shippingServiceOptionDto?.ShippingPackage.Adapt<Domain.AggregateModel.OrderAggregate.ShippingPackage>());
            return shippingServiceOption;
        }
        #endregion  
    }
}
