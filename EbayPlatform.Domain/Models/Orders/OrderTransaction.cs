using EbayPlatform.Domain.Core.Abstractions;
using System;

namespace EbayPlatform.Domain.Models.Orders
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
        public string ConditionID { get; private set; }

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
    }
}
