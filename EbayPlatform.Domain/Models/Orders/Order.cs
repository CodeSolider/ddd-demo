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
        /// 店铺名称
        /// </summary>
        public string ShopName { get; private set; }

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
        /// 卖家账号
        /// </summary>
        public string SellerUserID { get; private set; }

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
        /// 发货选项服务
        /// </summary>
        public ShippingServiceOption ShippingServiceSelected { get; private set; }


        protected Order()
        {
            OrderTransactions = new List<OrderTransaction>();
        }


        public Order(string orderID, string orderStatus, string shopName,
            string paymentMethods, string sellerEmail, string sellerUserID,
            MoneyValue adjustmentAmount, MoneyValue amountPaid,
            MoneyValue amountSaved, MoneyValue total, MoneyValue subtotal,
            CheckoutStatus checkoutStatus, Address shippingAddress,
            ShippingServiceOption shippingServiceSelected,
            DateTime? createdTime) : this()
        {
            this.OrderID = orderID;
            this.OrderStatus = orderStatus;
            this.ShopName = shopName;
            this.PaymentMethods = paymentMethods;
            this.SellerEmail = sellerEmail;
            this.SellerUserID = sellerUserID;
            this.AdjustmentAmount = adjustmentAmount;
            this.AmountPaid = amountPaid;
            this.AmountSaved = amountSaved;
            this.AmountSaved = amountSaved;
            this.Total = total;
            this.Subtotal = subtotal;
            this.CheckoutStatus = checkoutStatus;
            this.ShippingAddress = shippingAddress;
            this.ShippingServiceSelected = shippingServiceSelected;
            this.CreatedTime = createdTime;
            this.SyncDate = DateTime.Now;

            ////添加事件
            //this.AddDomainEvent(new CreateOrderDomainEvent(this));
        }


        /// <summary>
        /// 添加订单交易信息
        /// </summary>
        public void AddOrderTransaction(string transactionID, string orderLineItemID,
            string siteCode, string title, int? conditionID, string conditionDisplayName,
            int quantityPurchased, MoneyValue transactionPrice, TransactionStatus status,
            ShippingServiceOption shippingServiceOption, DateTime? createdDate)
        {
            var existsOrderTransaction = this.OrderTransactions.FirstOrDefault(o => o.TransactionID == transactionID && o.OrderLineItemID == orderLineItemID);
            if (existsOrderTransaction != null)
            {
                existsOrderTransaction.ChangeOrderTransaction(quantityPurchased, transactionPrice, status, createdDate);
            }
            else
            {
                this.OrderTransactions.Add(new OrderTransaction(transactionID, orderLineItemID, siteCode, title, conditionID, conditionDisplayName,
                                                              quantityPurchased, transactionPrice, status, shippingServiceOption, createdDate));
            }
        }


        /// <summary>
        /// 添加发货详情信息
        /// </summary>
        /// <param name="salesTax"></param>
        /// <param name="sellingManagerSalesRecordNumber"></param>
        /// <param name="getItFast"></param>
        public void AddShippingDetail(SalesTax salesTax, int? sellingManagerSalesRecordNumber, bool? getItFast,
            List<ShippingServiceOption> shippingServiceOptions)
        {
            this.ShippingDetail = new ShippingDetail(salesTax, sellingManagerSalesRecordNumber, getItFast);

            shippingServiceOptions.ForEach(shippingItem =>
            {
                ShippingDetail.ChangeShippingServiceOption(shippingItem.ShippingService, shippingItem.ShippingServiceCost,
                                                           shippingItem.ShippingServicePriority, shippingItem.ExpeditedService,
                                                           shippingItem.ShippingTimeMin, shippingItem.ShippingTimeMax,
                                                           shippingItem.ShippingPackage);
            });
        }

    }
}
