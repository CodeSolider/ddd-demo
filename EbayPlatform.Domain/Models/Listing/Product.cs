using EbayPlatform.Domain.Core.Abstractions;
using System;

namespace EbayPlatform.Domain.Models.Listing
{
    /// <summary>
    /// 订单产品
    /// </summary>
    public class Product : Entity<long>, IAggregateRoot
    {
        /// <summary>
        /// ItemID 主键唯一
        /// </summary>
        public string ItemID { get; private set; }

        /// <summary>
        /// MSKU
        /// </summary>
        public string MSKU { get; private set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string SiteCode { get; private set; }

        /// <summary>
        /// StartPrice
        /// </summary>
        public MoneyValue StartPrice { get; private set; }

        /// <summary>
        /// 买方担保费
        /// </summary>
        public MoneyValue BuyerGuaranteePrice { get; private set; }

        /// <summary>
        /// 购买价格
        /// </summary>
        public MoneyValue BuyItNowPrice { get; private set; }

        /// <summary>
        /// ReservePrice
        /// </summary>
        public MoneyValue ReservePrice { get; private set; }

        /// <summary>
        /// /数量
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// QuantityAvailableHint
        /// </summary>
        public string QuantityAvailableHint { get; private set; }

        /// <summary>
        /// QuantityThreshold
        /// </summary>
        public int QuantityThreshold { get; private set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; private set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 点击次数
        /// </summary>
        public long HitCount { get; private set; }

        /// <summary>
        /// 计数器
        /// </summary>
        public string HitCounter { get; private set; }

        /// <summary>
        /// 库存跟踪方式
        /// </summary>
        public string InventoryTrackingMethod { get; private set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ListingType { get; private set; }

        /// <summary>
        /// 子类型
        /// </summary>
        public string ListingSubType { get; private set; }

        /// <summary>
        /// 定位
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentMethods { get; private set; }

        /// <summary>
        /// 销售状态
        /// </summary>
        public SellingStatus SellingStatus { get; private set; }

        /// <summary>
        /// 符加类别
        /// </summary>
        public ProductCategory FreeAddedCategory { get; private set; }

        /// <summary>
        /// 主类别
        /// </summary>
        public ProductCategory PrimaryCategory { get; private set; }

        /// <summary>
        /// 第二类别
        /// </summary>
        public ProductCategory SecondaryCategory { get; private set; }

        /// <summary>
        /// PrivateListing
        /// </summary>
        public bool PrivateListing { get; private set; }

        /// <summary>
        /// ItemRevised
        /// </summary>
        public bool ItemRevised { get; private set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime SyncDate { get; private set; }


        protected Product() { }

        public Product(string itemID, string msku, string siteCode, MoneyValue startPrice,
          MoneyValue buyerGuaranteePrice, MoneyValue buyItNowPrice, MoneyValue reservePrice,
          int quantity, string quantityAvailableHint, int quantityThreshold, string country,
          string currencyCode, string title, string description, long hitCount, string hitCounter,
          string inventoryTrackingMethod, string listingType, string listingSubType, string location,
          string paymentMethods, SellingStatus sellingStatus, ProductCategory freeAddedCategory, ProductCategory primaryCategory,
         ProductCategory secondaryCategory, bool privateListing, bool itemRevised
          )
        {
            this.ItemID = itemID;
            this.MSKU = msku;
            this.SiteCode = siteCode;
            this.StartPrice = startPrice;
            this.BuyerGuaranteePrice = buyerGuaranteePrice;
            this.BuyItNowPrice = buyItNowPrice;
            this.ReservePrice = reservePrice;
            this.Quantity = quantity;
            this.QuantityAvailableHint = quantityAvailableHint;
            this.QuantityThreshold = quantityThreshold;
            this.Country = country;
            this.CurrencyCode = currencyCode;
            this.Title = title;
            this.Description = description;
            this.HitCount = hitCount;
            this.HitCounter = hitCounter;
            this.InventoryTrackingMethod = inventoryTrackingMethod;
            this.ListingType = listingType;
            this.ListingSubType = listingSubType;
            this.Location = location;
            this.PaymentMethods = paymentMethods;
            this.SellingStatus = sellingStatus;
            this.FreeAddedCategory = freeAddedCategory;
            this.PrimaryCategory = primaryCategory;
            this.SecondaryCategory = secondaryCategory;
            this.PrivateListing = privateListing;
            this.ItemRevised = itemRevised;
            this.SyncDate = DateTime.Now;
        }

    }
}
