using System; 

namespace EbayPlatform.Application.Dtos.Accounts
{
    public class AccountDetailDto
    {
        /// <summary>
        /// 参数
        /// </summary>
        public string RefNumber { get;  set; }

        /// <summary>
        /// ItemID
        /// </summary>
        public string ItemID { get;  set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get;  set; }

        /// <summary>
        /// 账单类型
        /// </summary>
        public string AccountType { get;  set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get;  set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get;  set; }

        #region Balance
        /// <summary>
        /// 余额
        /// </summary>
        public decimal BalanceValue { get; set; }

        /// <summary>
        /// 余额币种
        /// </summary>
        public string BalanceCurrency { get; set; }

        #endregion

        #region GrossDetailAmount
        /// <summary>
        /// 总额
        /// </summary>
        public decimal GrossDetailAmountValue { get; set; }

        /// <summary>
        /// 总额币种
        /// </summary>
        public string GrossDetailAmountCurrency { get; set; }

        #endregion

        #region ConversionRate
        /// <summary>
        /// 转化率
        /// </summary>
        public decimal ConversionRateValue { get; set; }

        /// <summary>
        /// 转化率币种
        /// </summary>
        public string ConversionRateCurrency { get; set; }

        #endregion

        #region NetDetailAmount
        /// <summary>
        /// 净额
        /// </summary>
        public decimal NetDetailAmountValue { get; set; }

        /// <summary>
        /// 净额币种
        /// </summary>
        public string NetDetailAmountCurrency { get; set; }

        #endregion 

        /// <summary>
        /// 百分比增值税
        /// </summary>
        public decimal VATPercent { get;  set; }

        /// <summary>
        /// OrderLineItemID
        /// </summary>
        public string OrderLineItemID { get;  set; }

        /// <summary>
        /// TransactionID
        /// </summary>
        public string TransactionID { get;  set; }

        /// <summary>
        /// 是否收取额外费用
        /// </summary>
        public bool ReceivedTopRatedDiscount { get;  set; }
    }
}
