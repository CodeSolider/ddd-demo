using System.Collections.Generic;

namespace EbayPlatform.Application.Dtos.Accounts
{
    /// <summary>
    /// 账单
    /// </summary>
    public class AccountDto
    {
        public AccountDto()
        {
            AccountDetails = new List<AccountDetailDto>();
        }

        /// <summary>
        /// 账单ID
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 账单状态
        /// </summary>
        public string AccountState { get; set; }

        #region InvoicePayment
        /// <summary>
        /// 发票付款
        /// </summary>
        public decimal InvoicePaymentValue { get; set; }

        /// <summary>
        /// 发票付款币种
        /// </summary>
        public string InvoicePaymentCurrency { get; set; }

        #endregion

        #region InvoiceCredit
        /// <summary>
        /// 发票信用证
        /// </summary>
        public decimal InvoiceCreditValue { get; set; }

        /// <summary>
        /// 发票信用证币种
        /// </summary>
        public string InvoiceCreditCurrency { get; set; }

        #endregion

        #region InvoiceNewFee
        /// <summary>
        /// 开具新费用发票
        /// </summary>
        public decimal InvoiceNewFeeValue { get; set; }

        /// <summary>
        /// 开具新费用发票
        /// </summary>
        public string InvoiceNewFeeCurrency { get; set; }

        #endregion

        /// <summary>
        /// 额外账户
        /// </summary>
        public AdditionalAccountDto AdditionalAccount { get; set; }

        /// <summary>
        /// 账单明细
        /// </summary>
        public List<AccountDetailDto> AccountDetails { get; set; }
    }
}
