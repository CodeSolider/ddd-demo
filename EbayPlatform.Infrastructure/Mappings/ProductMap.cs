using EbayPlatform.Domain.AggregateModel.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayPlatform.Infrastructure.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product)).HasComment("产品");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.ItemID).HasMaxLength(50).HasComment("eBay下载的产品ID、唯一").IsRequired(true);
            builder.Property(p => p.MSKU).HasMaxLength(50).HasComment("渠道SKU").IsRequired(false);
            builder.Property(p => p.ShopName).HasMaxLength(50).HasComment("店铺编号，来自ERP").IsRequired(true);
            builder.Property(v => v.SiteCode).HasMaxLength(20).HasComment("站点").IsRequired(false);
            builder.OwnsOne(p => p.StartPrice, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("StartPrice").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("StartPriceCurrency").IsRequired(false);
            });
            builder.OwnsOne(p => p.BuyerGuaranteePrice, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("BuyerGuaranteePrice").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("BuyerGuaranteePriceCurrency").IsRequired(false);
            });
            builder.OwnsOne(p => p.BuyItNowPrice, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("BuyItNowPrice").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("BuyItNowPriceCurrency").IsRequired(false);
            });
            builder.OwnsOne(p => p.ReservePrice, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("ReservePrice").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("ReservePriceCurrency").IsRequired(false);
            });
            builder.Property(p => p.Quantity).HasDefaultValue(0).HasComment("数量").IsRequired(true);
            builder.Property(p => p.QuantityAvailableHint).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.QuantityThreshold).HasDefaultValue(0).IsRequired(true);
            builder.Property(p => p.Country).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.CurrencyCode).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.Title).HasMaxLength(4000).IsRequired(false);
            builder.Property(p => p.Description).HasMaxLength(4000).IsRequired(false);
            builder.Property(p => p.HitCount).HasDefaultValue(0).IsRequired(true);
            builder.Property(p => p.HitCounter).HasMaxLength(50).IsRequired(false);
            builder.Property(p => p.InventoryTrackingMethod).HasMaxLength(20).IsRequired(false);
            builder.Property(p => p.ListingType).HasMaxLength(50).IsRequired(false);
            builder.Property(p => p.ListingSubType).HasMaxLength(50).IsRequired(false);
            builder.Property(p => p.Location).HasMaxLength(50).IsRequired(false);
            builder.Property(p => p.PaymentMethods).HasMaxLength(300).IsRequired(false);
            //销售状态
            builder.OwnsOne(p => p.SellingStatus, v =>
            {
                v.OwnsOne(p => p.ConvertedCurrentPrice, v =>
                {
                    v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("ConvertedCurrentPrice").IsRequired(true);
                    v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("ConvertedCurrentPriceCurrency").IsRequired(false);
                });
                v.OwnsOne(p => p.CurrentPrice, v =>
                {
                    v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("CurrentPrice").IsRequired(true);
                    v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("CurrentPriceCurrency").IsRequired(false);
                });
                v.Property(p => p.ListingStatus).HasMaxLength(50).IsRequired(false);
                v.Property(p => p.QuantitySold).HasDefaultValue(0).IsRequired(true);
                v.Property(p => p.AdminEnded).HasDefaultValue(false).IsRequired(true);
                v.OwnsOne(p => p.PromotionalSaleDetail, v =>
                {
                    v.Property(p => p.StartTime).HasDefaultValue(null).IsRequired(true);
                    v.OwnsOne(p => p.OriginalPrice, v =>
                    {
                        v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("OriginalPrice").IsRequired(true);
                        v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("OriginalPriceCurrency").IsRequired(false);
                    });
                    v.Property(p => p.EndTime).IsRequired(false);
                });
            });

            builder.OwnsOne(p => p.FreeAddedCategory, v =>
            {
                v.Property(p => p.CategoryID).HasMaxLength(50).HasColumnName("FreeAddedCategoryID").IsRequired(false);
                v.Property(p => p.CategoryName).HasMaxLength(300).HasColumnName("FreeAddedCategoryName").IsRequired(false);
            });
            builder.OwnsOne(p => p.PrimaryCategory, v =>
            {
                v.Property(p => p.CategoryID).HasMaxLength(50).HasColumnName("PrimaryCategoryID").IsRequired(false);
                v.Property(p => p.CategoryName).HasMaxLength(300).HasColumnName("PrimaryCategoryName").IsRequired(false);
            });
            builder.OwnsOne(p => p.SecondaryCategory, v =>
            {
                v.Property(p => p.CategoryID).HasMaxLength(50).HasColumnName("SecondaryCategoryID").IsRequired(false);
                v.Property(p => p.CategoryName).HasMaxLength(300).HasColumnName("SecondaryCategoryName").IsRequired(false);
            });
            builder.Property(p => p.PrivateListing).HasDefaultValue(false).IsRequired(true);
            builder.Property(p => p.ItemRevised).HasDefaultValue(false).IsRequired(true);
            builder.Property(p => p.SyncDate).HasDefaultValueSql("getDate()").HasComment("同步日期").IsRequired(true);
        }
    }
}
