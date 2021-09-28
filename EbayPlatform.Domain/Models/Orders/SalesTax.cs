using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Models.Orders
{
    /// <summary>
    /// 营业税
    /// </summary>
    public class SalesTax : ValueObject
    {
        /// <summary>
        /// 营业税率
        /// </summary>
        public float SalesTaxPercent { get; private set; }

        /// <summary>
        /// SalesTaxState
        /// </summary>
        public string SalesTaxState { get; private set; }

        /// <summary>
        /// 营业税额
        /// </summary>
        public MoneyValue SalesTaxAmount { get; private set; }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return SalesTaxPercent;
            yield return SalesTaxState;
            yield return SalesTaxAmount;
        }


        public SalesTax() { }

        public SalesTax(float salesTaxPercent, string salesTaxState, MoneyValue salesTaxAmount)
        {
            this.SalesTaxPercent = salesTaxPercent;
            this.SalesTaxState = salesTaxState;
            this.SalesTaxAmount = salesTaxAmount;
        }

    }
}
