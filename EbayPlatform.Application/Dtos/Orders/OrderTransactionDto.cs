using System;

namespace EbayPlatform.Application.Dtos.Orders
{
    public class OrderTransactionDto
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// OrderLineItemID
        /// </summary>
        public string OrderLineItemID { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ConditionID
        /// </summary>
        public int? ConditionID { get; set; }

        /// <summary>
        /// ConditionDisplayName
        /// </summary>
        public string ConditionDisplayName { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int QuantityPurchased { get; set; }

        #region TransactionPrice
        /// <summary>
        /// 或者值
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; set; }
        #endregion

        #region TransactionStatus
        /// <summary>
        /// 待付款状态
        /// </summary>
        public string PaymentHoldStatus { get; set; }

        /// <summary>
        /// 查询状态
        /// </summary>
        public string InquiryStatus { get; set; }

        /// <summary>
        /// 退回状态
        /// </summary>
        public string ReturnStatus { get; set; }
        #endregion

        /// <summary>
        /// 发货服务选中项
        /// </summary>
        public ShippingServiceOptionDto ShippingServiceSelected { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreatedDate { get; set; }

    }
}
