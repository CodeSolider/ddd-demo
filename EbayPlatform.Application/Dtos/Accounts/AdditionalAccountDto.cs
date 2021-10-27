namespace EbayPlatform.Application.Dtos.Accounts
{
    /// <summary>
    /// 额外账单
    /// </summary>
    public class AdditionalAccountDto
    {
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

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 账号编码
        /// </summary>
        public string AccountCode { get; set; }
    }
}
