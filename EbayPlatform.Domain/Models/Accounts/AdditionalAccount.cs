using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic; 

namespace EbayPlatform.Domain.Models.Accounts
{
    public class AdditionalAccount : ValueObject
    {
        /// <summary>
        /// 余额
        /// </summary>
        public MoneyValue Balance { get; private set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; private set; }

        /// <summary>
        /// 账号编码
        /// </summary>
        public string AccountCode { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Balance;
            yield return CurrencyCode;
            yield return AccountCode;
        }

        public AdditionalAccount() { }

        public AdditionalAccount(MoneyValue balance, string currencyCode, string accountCode)
        {
            this.Balance = balance;
            this.CurrencyCode = currencyCode;
            this.AccountCode = accountCode;
        }

    }
}
