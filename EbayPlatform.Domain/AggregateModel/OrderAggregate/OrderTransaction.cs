using EbayPlatform.Domain.Core.Abstractions;
using System;

namespace EbayPlatform.Domain.AggregateModel.OrderAggregate
{
    /// <summary>
    /// 订单交易信息
    /// </summary>
    public class OrderTransaction : Entity<long>
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        public string TransactionID { get; private set; }

        /// <summary>
        /// OrderLineItemID
        /// </summary>
        public string OrderLineItemID { get; private set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string SiteCode { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// ConditionID
        /// </summary>
        public int? ConditionID { get; private set; }

        /// <summary>
        /// ConditionDisplayName
        /// </summary>
        public string ConditionDisplayName { get; private set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int QuantityPurchased { get; private set; }

        /// <summary>
        /// 交易价
        /// </summary>
        public MoneyValue TransactionPrice { get; private set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public TransactionStatus Status { get; private set; }

        /// <summary>
        /// 发货服务选中项
        /// </summary>
        public ShippingServiceOption ShippingServiceSelected { get; private set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreatedDate { get; private set; }



        protected OrderTransaction() { }

        public OrderTransaction(string transactionID, string orderLineItemID,
            string siteCode, string title, int? conditionID, string conditionDisplayName,
            int quantityPurchased, MoneyValue transactionPrice, TransactionStatus status,
            ShippingServiceOption shippingServiceOption, DateTime? createdDate)
        {
            this.TransactionID = transactionID;
            this.OrderLineItemID = orderLineItemID;
            this.SiteCode = siteCode;
            this.Title = title;
            this.ConditionID = conditionID;
            this.ConditionDisplayName = conditionDisplayName;
            this.QuantityPurchased = quantityPurchased;
            this.TransactionPrice = transactionPrice;
            this.Status = status;
            this.ShippingServiceSelected = shippingServiceOption;
            this.CreatedDate = createdDate;
        }

        /// <summary>
        /// 更新订单交易信息
        /// </summary>
        /// <param name="siteCode"></param>
        /// <param name="title"></param>
        /// <param name="conditionID"></param>
        /// <param name="conditionDisplayName"></param>
        /// <param name="quantityPurchased"></param>
        /// <param name="transactionPrice"></param>
        /// <param name="status"></param>
        /// <param name="createdDate"></param>
        public void ChangeOrderTransaction(int quantityPurchased, MoneyValue transactionPrice,
            TransactionStatus status, DateTime? createdDate)
        {
            this.QuantityPurchased = quantityPurchased;
            this.TransactionPrice = transactionPrice;
            this.Status = status;
            this.CreatedDate = createdDate;
        }



    }
}
