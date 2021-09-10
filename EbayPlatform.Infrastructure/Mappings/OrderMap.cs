using EbayPlatform.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayPlatform.Infrastructure.Mappings
{
    /// <summary>
    /// 订单映射类
    /// </summary>
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order").HasComment("订单");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.OrderID).HasMaxLength(50).HasComment("eBay下载的订单ID、唯一");
            builder.Property(p => p.OrderStatus).HasMaxLength(20).IsRequired(false).HasComment("订单状态");
            builder.OwnsOne(p => p.AdjustmentAmount, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("AdjustmentAmount");
                v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("AdjustmentAmountCurrency");
            });

            builder.OwnsOne(p => p.AmountPaid, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("AmountPaid");
                v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("AmountPaidCurrency");
            });

            builder.OwnsOne(p => p.AmountSaved, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("AmountSaved");
                v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("AmountSavedCurrency");
            });

            builder.Property(p => p.PaymentMethods).HasMaxLength(300).IsRequired(false).HasComment("付款方式");
            builder.Property(p => p.SellerEmail).HasMaxLength(100).IsRequired(false).HasComment("卖家邮箱地址");
            builder.Property(p => p.CreatedTime).IsRequired(false).HasComment("订单创建日期");
            builder.Property(p => p.SyncDate).HasDefaultValueSql("getDate()").HasComment("同步日期");

            builder.OwnsMany(p => p.OrderTransactions, p =>
            {
                p.HasKey(v => v.Id);
                p.Property(v => v.Id).ValueGeneratedOnAdd().HasComment("主键Id");
                p.Property(v => v.TransactionID).HasMaxLength(50).HasComment("TransactionID");
                p.Property(v => v.OrderLineItemID).HasMaxLength(50).HasComment("OrderLineItemID");
                p.Property(v => v.SiteCode).HasMaxLength(20).HasComment("站点");
                p.Property(v => v.Title).IsRequired(false).HasComment("标题");
                p.Property(v => v.ConditionID).HasMaxLength(100).IsRequired(false).HasComment("条件性ID");
                p.Property(v => v.ConditionDisplayName).HasMaxLength(500).IsRequired(false).HasComment("条件性ID");
                p.Property(v => v.QuantityPurchased).HasComment("购买数");
                p.OwnsOne(v => v.TransactionPrice, v =>
                {
                    v.Property(v => v.Value).HasPrecision(18, 3).HasColumnName("TransactionPrice");
                    v.Property(v => v.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("TransactionPriceCurrency");
                });

                p.OwnsOne(v => v.Status, v =>
                {
                    v.Property(v => v.PaymentHoldStatus).HasMaxLength(20).IsRequired(false).HasComment("待付款状态");
                    v.Property(v => v.InquiryStatus).HasMaxLength(40).IsRequired(false).HasComment("查询状态");
                    v.Property(v => v.ReturnStatus).HasMaxLength(40).IsRequired(false).HasComment("退回状态");
                });

                p.OwnsOne(v => v.ShippingServiceSelected, v =>
                {
                    v.Property(v => v.ShippingService).IsRequired(false).HasMaxLength(300).HasComment("运输服务");
                    v.OwnsOne(v => v.ShippingServiceCost, v =>
                    {
                        v.Property(v => v.Value).HasPrecision(18, 3).HasColumnName("ShippingServiceCost");
                        v.Property(v => v.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("AmountSavedCurrency");
                    });
                    v.Property(p => p.ShippingServicePriority).IsRequired(false);
                    v.Property(v => v.ExpeditedService).IsRequired(false).HasComment("是否加急服务");
                    v.Property(v => v.ShippingTimeMin).IsRequired(false).HasComment("装运时间");
                    v.Property(v => v.ShippingTimeMax).IsRequired(false).HasComment("装运时间");
                    v.OwnsMany(v => v.ShippingPackages, m =>
                    {
                        m.Property(v => v.StoreID).HasMaxLength(100).IsRequired(false);
                        m.Property(v => v.ShippingTrackingEvent).HasMaxLength(200).IsRequired(false);
                        m.Property(v => v.ScheduledDeliveryTimeMin).IsRequired(false);
                        m.Property(v => v.ScheduledDeliveryTimeMax).IsRequired(false);
                        m.Property(v => v.ActualDeliveryTime).IsRequired(false);
                        m.Property(v => v.EstimatedDeliveryTimeMin).IsRequired(false).HasComment("预计发货时间");
                        m.Property(v => v.EstimatedDeliveryTimeMax).IsRequired(false).HasComment("预计发货时间");
                    });
                });
                p.Property(p => p.CreatedDate).IsRequired(false).HasComment("交易创建日期，非系统日期");
            });

            builder.OwnsOne(p => p.Total, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("Total");
                v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("TotalCurrency");
            });

            builder.OwnsOne(p => p.Subtotal, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("Subtotal");
                v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("TotalCurrency");
            });

            builder.OwnsOne(p => p.CheckoutStatus, v =>
            {
                v.Property(p => p.EBayPaymentStatus).HasMaxLength(40).IsRequired(false).HasComment("EBayPaymentStatus");
                v.Property(p => p.PaymentMethod).HasMaxLength(30).IsRequired(false).HasComment("结账方式");
                v.Property(p => p.Status).HasMaxLength(10).IsRequired(false).HasComment("状态");
                v.Property(p => p.IntegratedMerchantCreditCardEnabled).HasMaxLength(10).IsRequired(false);
                v.Property(p => p.PaymentInstrument).HasMaxLength(30).IsRequired(false);
                v.Property(p => p.LastModifiedTime).HasMaxLength(30).IsRequired(false).HasComment("最后更新时间，非系统时间");
            });

            builder.OwnsOne(p => p.ShippingDetail, v =>
            {
                v.OwnsOne(v => v.SalesTax, s =>
                {
                    s.Property(v => v.SalesTaxPercent).HasComment("营业税率");
                    s.Property(v => v.SalesTaxState).HasMaxLength(300).IsRequired(false);
                    s.OwnsOne(p => p.SalesTaxAmount, v =>
                    {
                        v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("SalesTaxAmount");
                        v.Property(p => p.Currency).HasMaxLength(10).IsRequired(false).HasColumnName("SalesTaxAmountCurrency");
                    });
                });
                v.Property(v => v.SellingManagerSalesRecordNumber).IsRequired(false);
                v.Property(v => v.GetItFast).IsRequired(false).HasComment("是否加急");
                v.OwnsMany(v => v.ShippingServiceOptions, v =>
                {
                    v.Property(v => v.ShippingService).IsRequired(false).HasMaxLength(300).HasComment("运输服务");
                    v.OwnsOne(v => v.ShippingServiceCost, v =>
                    {
                        v.Property(v => v.Value).HasPrecision(18, 3).HasColumnName("ShippingServiceCost");
                        v.Property(v => v.Currency).HasMaxLength(10).HasColumnName("AmountSavedCurrency");
                    });
                    v.Property(p => p.ShippingServicePriority).IsRequired(false);
                    v.Property(v => v.ExpeditedService).IsRequired(false).HasComment("是否加急服务");
                    v.Property(v => v.ShippingTimeMin).IsRequired(false).HasComment("装运时间");
                    v.Property(v => v.ShippingTimeMax).IsRequired(false).HasComment("装运时间");
                    v.OwnsMany(v => v.ShippingPackages, m =>
                    {
                        m.Property(v => v.EstimatedDeliveryTimeMin).IsRequired(false).HasComment("预计发货时间");
                        m.Property(v => v.EstimatedDeliveryTimeMax).IsRequired(false).HasComment("预计发货时间");
                    });
                });
            });

            builder.OwnsOne(p => p.ShippingAddress, v =>
            {
                v.Property(v => v.AddressID).HasMaxLength(100).HasComment("地址ID，同样具有唯一性，非系统ID");
                v.Property(v => v.Name).HasMaxLength(300).IsRequired(false).HasComment("地址名称");
                v.Property(v => v.Street).HasMaxLength(300).IsRequired(false).HasComment("街道1");
                v.Property(v => v.Street1).HasMaxLength(300).IsRequired(false).HasComment("街道1");
                v.Property(v => v.Street2).HasMaxLength(300).IsRequired(false).HasComment("街道2");
                v.Property(v => v.CityName).HasMaxLength(300).IsRequired(false).HasComment("城市名称");
                v.Property(v => v.StateOrProvince).HasMaxLength(300).IsRequired(false).HasComment("地址ID，同样具有唯一性，非系统ID");
                v.Property(v => v.Country).HasMaxLength(10).IsRequired(false).HasComment("地址ID，同样具有唯一性，非系统ID");
                v.Property(v => v.CountryName).HasMaxLength(300).IsRequired(false).HasComment("国家名称");
                v.Property(v => v.Phone).HasMaxLength(20).IsRequired(false).HasComment("电话号码");
                v.Property(v => v.PostalCode).HasMaxLength(100).IsRequired(false).HasComment("邮编");
                v.Property(v => v.AddressOwner).HasMaxLength(10).IsRequired(false).HasComment("地址所属者");
            });

            builder.OwnsOne(p => p.ShippingServiceSelected, v =>
            {
                v.Property(v => v.ShippingService).IsRequired(false).HasMaxLength(300).HasComment("运输服务");
                v.OwnsOne(v => v.ShippingServiceCost, v =>
                {
                    v.Property(v => v.Value).HasPrecision(18, 3).HasColumnName("ShippingServiceCost");
                    v.Property(v => v.Currency).HasMaxLength(10).HasColumnName("AmountSavedCurrency");
                });
                v.Property(p => p.ShippingServicePriority).IsRequired(false);
                v.Property(v => v.ExpeditedService).IsRequired(false).HasComment("是否加急服务");
                v.Property(v => v.ShippingTimeMin).IsRequired(false).HasComment("装运时间");
                v.Property(v => v.ShippingTimeMax).IsRequired(false).HasComment("装运时间");
                v.OwnsMany(v => v.ShippingPackages, m =>
                {
                    m.Property(v => v.EstimatedDeliveryTimeMin).IsRequired(false).HasComment("预计发货时间");
                    m.Property(v => v.EstimatedDeliveryTimeMax).IsRequired(false).HasComment("预计发货时间");
                });
            });

        }
    }
}
