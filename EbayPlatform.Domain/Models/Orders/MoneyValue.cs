using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Models.Orders
{
    public class MoneyValue : ValueObject
    {
        /// <summary>
        /// 或者值
        /// </summary>
        public decimal Value { get; private set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; private set; }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }



        protected MoneyValue() { }

        public MoneyValue(decimal value, string currency)
        {
            this.Value = value;
            this.Currency = currency;
        }
    }
}
