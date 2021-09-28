using EbayPlatform.Domain.Models.Listing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayPlatform.Infrastructure.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product").HasComment("产品");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.ItemID).HasMaxLength(50).IsRequired(true).HasComment("eBay下载的产品ID、唯一");
            builder.Property(p => p.MSKU).HasMaxLength(50).IsRequired(false).HasColumnName("渠道SKU");
            builder.Property(v => v.SiteCode).HasMaxLength(20).IsRequired(false).HasComment("站点");
            builder.OwnsOne(p => p.StartPrice, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).IsRequired(true).HasColumnName("StartPrice");
                v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("StartPriceCurrency");
            });
            builder.OwnsOne(p => p.BuyerGuaranteePrice, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).IsRequired(true).HasColumnName("BuyerGuaranteePrice");
                v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("BuyerGuaranteePriceCurrency");
            });
            builder.OwnsOne(p => p.BuyItNowPrice, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).IsRequired(true).HasColumnName("BuyItNowPrice");
                v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("BuyItNowPriceCurrency");
            });
            builder.OwnsOne(p => p.ReservePrice, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).IsRequired(true).HasColumnName("ReservePrice");
                v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("ReservePriceCurrency");
            });
            builder.Property(p => p.Quantity).IsRequired(true).HasDefaultValue(0).HasComment("数量");
            builder.Property(p => p.QuantityAvailableHint).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.QuantityThreshold).IsRequired(true).HasDefaultValue(0);
            builder.Property(p => p.Country).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.CurrencyCode).HasMaxLength(10).IsRequired(false);
            builder.Property(p => p.Title).HasMaxLength(4000).IsRequired(false);
            builder.Property(p => p.Description).HasMaxLength(4000).IsRequired(false);
            builder.Property(p => p.HitCount).IsRequired(true).HasDefaultValue(0);
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
                    v.Property(p => p.Value).HasPrecision(18, 3).IsRequired(true).HasColumnName("ConvertedCurrentPrice");
                    v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("ConvertedCurrentPriceCurrency");
                });
                v.OwnsOne(p => p.CurrentPrice, v =>
                {
                    v.Property(p => p.Value).HasPrecision(18, 3).IsRequired(true).HasColumnName("CurrentPrice");
                    v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("CurrentPriceCurrency");
                });
                v.Property(p => p.ListingStatus).HasMaxLength(50).IsRequired(false);
                v.Property(p => p.QuantitySold).IsRequired(true).HasDefaultValue(0);
                v.Property(p => p.AdminEnded).IsRequired(true).HasDefaultValue(false);
                v.OwnsOne(p => p.PromotionalSaleDetail, v =>
                {
                    v.Property(p => p.StartTime).IsRequired(false);
                    v.OwnsOne(p => p.OriginalPrice, v =>
                    {
                        v.Property(p => p.Value).HasPrecision(18, 3).IsRequired(true).HasColumnName("OriginalPrice");
                        v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("OriginalPriceCurrency");
                    });
                    v.Property(p => p.EndTime).IsRequired(false);
                });
            });

            builder.OwnsOne(p => p.FreeAddedCategory, v =>
            {
                v.Property(p => p.CategoryID).HasMaxLength(50).IsRequired(false).HasColumnName("FreeAddedCategoryID");
                v.Property(p => p.CategoryName).HasMaxLength(300).IsRequired(false).HasColumnName("FreeAddedCategoryName");
            });
            builder.OwnsOne(p => p.PrimaryCategory, v =>
            {
                v.Property(p => p.CategoryID).HasMaxLength(50).IsRequired(false).HasColumnName("PrimaryCategoryID");
                v.Property(p => p.CategoryName).HasMaxLength(300).IsRequired(false).HasColumnName("PrimaryCategoryName");
            });
            builder.OwnsOne(p => p.SecondaryCategory, v =>
            {
                v.Property(p => p.CategoryID).HasMaxLength(50).IsRequired(false).HasColumnName("SecondaryCategoryID");
                v.Property(p => p.CategoryName).HasMaxLength(300).IsRequired(false).HasColumnName("SecondaryCategoryName");
            });
            builder.Property(p => p.PrivateListing).IsRequired(true).HasDefaultValue(false);
            builder.Property(p => p.ItemRevised).IsRequired(true).HasDefaultValue(false);
            builder.Property(p => p.SyncDate).IsRequired(true).HasDefaultValueSql("getDate()").HasComment("同步日期");
        }
    }
}
