using EbayPlatform.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Models.Listing
{
    /// <summary>
    /// 销售状态
    /// </summary>
    public class SellingStatus : ValueObject
    {
        /// <summary>
        /// 折算当前价格
        /// </summary>
        public MoneyValue ConvertedCurrentPrice { get; private set; }

        /// <summary>
        /// 当前价格
        /// </summary>
        public MoneyValue CurrentPrice { get; private set; }

        /// <summary>
        /// Listing 状态
        /// </summary>
        public string ListingStatus { get; private set; }

        /// <summary>
        /// 售出数量
        /// </summary>
        public int QuantitySold { get; private set; }

        /// <summary>
        /// 如果为true,则表示违背了eBay刊登原则而取消了刊登
        /// </summary>
        public bool AdminEnded { get; private set; }

        /// <summary>
        /// 促销详情
        /// </summary>
        public PromotionalSaleDetail PromotionalSaleDetail { get; private set; }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ConvertedCurrentPrice;
            yield return CurrentPrice;
            yield return ListingStatus;
            yield return PromotionalSaleDetail;
            yield return QuantitySold;
        }

        public SellingStatus() { }

        public SellingStatus(MoneyValue convertedCurrentPrice, MoneyValue currentPrice,
            string listingStatus, int quantitySold, bool adminEnded,
            PromotionalSaleDetail promotionalSaleDetail
            )
        {
            this.ConvertedCurrentPrice = convertedCurrentPrice;
            this.CurrentPrice = currentPrice;
            this.ListingStatus = listingStatus;
            this.QuantitySold = quantitySold;
            this.AdminEnded = adminEnded;
            this.PromotionalSaleDetail = promotionalSaleDetail;
        }

    }
}
