using EbayPlatform.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbayPlatform.Domain.Models.Orders
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : Entity<long>, IAggregateRoot
    {
        /// <summary>
        /// eBay 下载的订单ID、唯一
        /// </summary>
        public string OrderID { get; private set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; private set; }

        /// <summary>
        /// 调价
        /// </summary>
        public MoneyValue AdjustmentAmount { get; private set; }

        /// <summary>
        /// 已付金额
        /// </summary>
        public MoneyValue AmountPaid { get; private set; }

        /// <summary>
        /// 节约金额
        /// </summary>
        public MoneyValue AmountSaved { get; private set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentMethods { get; private set; }

        /// <summary>
        /// 卖家邮箱地址
        /// </summary>
        public string SellerEmail { get; private set; }

        /// <summary>
        /// 订单创建日期
        /// </summary>
        public DateTime? CreatedTime { get; private set; }

        /// <summary>
        /// 订单同步日期
        /// </summary>
        public DateTime SyncDate { get; private set; }

        /// <summary>
        /// 订单交易
        /// </summary>
        public virtual ICollection<OrderTransaction> OrderTransactions { get; private set; }

        /// <summary>
        /// 小计
        /// </summary>
        public MoneyValue Subtotal { get; private set; }

        /// <summary>
        /// 总计
        /// </summary>
        public MoneyValue Total { get; private set; }

        /// <summary>
        /// 结账状态
        /// </summary>
        public CheckoutStatus CheckoutStatus { get; private set; }

        /// <summary>
        /// 装运详情
        /// </summary>
        public ShippingDetail ShippingDetail { get; private set; }

        /// <summary>
        /// 发货地址
        /// </summary>
        public Address ShippingAddress { get; private set; }

        /// <summary>
        /// 发货选中服务
        /// </summary>
        public ShippingServiceOption ShippingServiceSelected { get; private set; }


        protected Order()
        {
            OrderTransactions = new List<OrderTransaction>();
        }


        public Order(string orderID, string orderStatus,
            string paymentMethods, string sellerEmail,
            MoneyValue adjustmentAmount, MoneyValue amountPaid,
            MoneyValue amountSaved, MoneyValue total, MoneyValue subtotal,
            Address shippingAddress, DateTime? createdTime)
        {
            this.OrderID = orderID;
            this.OrderStatus = orderStatus;
            this.PaymentMethods = paymentMethods;
            this.SellerEmail = sellerEmail;
            this.AdjustmentAmount = adjustmentAmount;
            this.AmountPaid = amountPaid;
            this.AmountSaved = amountSaved;
            this.AmountSaved = amountSaved;
            this.Total = total;
            this.Subtotal = subtotal;
            this.ShippingAddress = shippingAddress;
            this.CreatedTime = createdTime;
        }

        /// <summary>
        /// 添加结账状态
        /// </summary>
        /// <param name="eBayPaymentStatus"></param>
        /// <param name="paymentMethod"></param>
        /// <param name="status"></param>
        /// <param name="integratedMerchantCreditCardEnabled"></param>
        /// <param name="paymentInstrument"></param>
        /// <param name="lastModifiedTime"></param>
        public void AddCheckoutStatus(string eBayPaymentStatus, string paymentMethod, string status,
            bool? integratedMerchantCreditCardEnabled, string paymentInstrument, DateTime? lastModifiedTime)
        {
            this.CheckoutStatus = new CheckoutStatus(eBayPaymentStatus, paymentMethod, status,
             integratedMerchantCreditCardEnabled, paymentInstrument, lastModifiedTime);
        }

        /// <summary>
        /// 添加发货详情信息
        /// </summary>
        /// <param name="salesTax"></param>
        /// <param name="sellingManagerSalesRecordNumber"></param>
        /// <param name="getItFast"></param>
        /// <param name="shippingServiceOptions"></param>
        public void AddShippingDetail(SalesTax salesTax, int? sellingManagerSalesRecordNumber, bool? getItFast,
            List<ShippingServiceOption> shippingServiceOptions)
        {
            this.ShippingDetail = new ShippingDetail(salesTax, sellingManagerSalesRecordNumber, getItFast);

            shippingServiceOptions.ForEach(shippingItem =>
            {
                ShippingDetail.ChangeShippingServiceOption(shippingItem.ShippingService, shippingItem.ExpeditedService,
                                                           shippingItem.ShippingTimeMin, shippingItem.ShippingTimeMax,
                                                           shippingItem.ShippingPackages.ToList());
            });
        }
    }
}
