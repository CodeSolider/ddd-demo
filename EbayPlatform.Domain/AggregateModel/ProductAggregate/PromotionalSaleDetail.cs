using EbayPlatform.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace EbayPlatform.Domain.AggregateModel.ProductAggregate
{
    /// <summary>
    /// 促销详情
    /// </summary>
    public class PromotionalSaleDetail : ValueObject
    {
        /// <summary>
        /// 促销开始时间
        /// </summary>
        public DateTime? StartTime { get; private set; }

        /// <summary>
        /// 原价
        /// </summary>
        public MoneyValue OriginalPrice { get; private set; }

        /// <summary>
        /// 促销结束时间
        /// </summary>
        public DateTime? EndTime { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartTime;
            yield return OriginalPrice;
            yield return EndTime;
        }

        public PromotionalSaleDetail() { }

        public PromotionalSaleDetail(DateTime? startTime, MoneyValue originalPrice, DateTime? endTime)
        {
            this.StartTime = startTime;
            this.OriginalPrice = originalPrice;
            this.EndTime = endTime;
        }

    }
}
