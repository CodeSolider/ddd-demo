using System; 

namespace EbayPlatform.Application.Dtos.Listing
{
    public class ProductDto
    {
        /// <summary>
        /// ItemID 主键唯一
        /// </summary>
        public string ItemID { get; set; }

        /// <summary>
        /// MSKU
        /// </summary>
        public string MSKU { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string SiteCode { get; set; }

        #region StartPrice

        /// <summary>
        /// 值
        /// </summary>
        public decimal StartPriceValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string StartPriceCurrency { get; set; }
        #endregion

        #region BuyerGuaranteePrice
        /// <summary>
        /// 值
        /// </summary>
        public decimal BuyerGuaranteePriceValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string BuyerGuaranteePriceCurrency { get; set; }
        #endregion

        #region BuyItNowPrice
        /// <summary>
        /// 值
        /// </summary>
        public decimal BuyItNowPriceValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string BuyItNowPriceCurrency { get; set; }
        #endregion

        #region ReservePrice
        /// <summary>
        /// 值
        /// </summary>
        public decimal ReservePriceValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string ReservePriceCurrency { get; set; }
        #endregion

        /// <summary>
        /// /数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// QuantityAvailableHint
        /// </summary>
        public string QuantityAvailableHint { get; set; }

        /// <summary>
        /// QuantityThreshold
        /// </summary>
        public int QuantityThreshold { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 点击次数
        /// </summary>
        public long HitCount { get; set; }

        /// <summary>
        /// 计数器
        /// </summary>
        public string HitCounter { get; set; }

        /// <summary>
        /// 库存跟踪方式
        /// </summary>
        public string InventoryTrackingMethod { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ListingType { get; set; }

        /// <summary>
        /// 子类型
        /// </summary>
        public string ListingSubType { get; set; }

        /// <summary>
        /// 定位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentMethods { get; set; }


        #region FreeAddedCategory
        /// <summary>
        /// 类别ID
        /// </summary>
        public string FreeAddedCategoryCategoryID { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string FreeAddedCategoryCategoryName { get; set; }
        #endregion

        #region PrimaryCategory
        /// <summary>
        /// 类别ID
        /// </summary>
        public string PrimaryCategoryID { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string PrimaryCategoryName { get; set; }
        #endregion

        #region SecondaryCategory
        /// <summary>
        /// 类别ID
        /// </summary>
        public string SecondaryCategoryID { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string SecondaryCategoryName { get; set; }
        #endregion

        /// <summary>
        /// PrivateListing
        /// </summary>
        public bool PrivateListing { get; set; }

        /// <summary>
        /// ItemRevised
        /// </summary>
        public bool ItemRevised { get; set; }


        #region SellerStatus
        /// <summary>
        /// Listing 状态
        /// </summary>
        public string ListingStatus { get; set; }

        /// <summary>
        /// 售出数量
        /// </summary>
        public int QuantitySold { get; set; }

        /// <summary>
        /// 如果为true,则表示违背了eBay刊登原则而取消了刊登
        /// </summary>
        public bool AdminEnded { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public decimal ConvertedCurrentPriceValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string ConvertedCurrentPriceCurrency { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public decimal CurrentPriceValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrentPriceCurrency { get; set; }

        /// <summary>
        /// 促销开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public decimal OriginalPriceValue { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string OriginalPriceCurrency { get; set; }

        /// <summary>
        /// 促销结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        #endregion

    }
}
