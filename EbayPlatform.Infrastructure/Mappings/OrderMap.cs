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
            builder.Property(p => p.OrderID).HasMaxLength(50).HasComment("eBay下载的订单ID、唯一").IsRequired(true);
            builder.Property(p => p.OrderStatus).HasMaxLength(20).HasComment("订单状态").IsRequired(false);
            builder.Property(p => p.ShopName).HasMaxLength(50).HasComment("店铺编号，来自ERP").IsRequired(true);
            builder.OwnsOne(p => p.AdjustmentAmount, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("AdjustmentAmount").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("AdjustmentAmountCurrency").IsRequired(false);
            });

            builder.OwnsOne(p => p.AmountPaid, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("AmountPaid").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("AmountPaidCurrency").IsRequired(false);
            });

            builder.OwnsOne(p => p.AmountSaved, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("AmountSaved").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("AmountSavedCurrency").IsRequired(false);
            });

            builder.Property(p => p.PaymentMethods).HasMaxLength(300).HasComment("付款方式").IsRequired(false);
            builder.Property(p => p.SellerEmail).HasMaxLength(100).HasComment("卖家邮箱地址").IsRequired(false);
            builder.Property(p => p.CreatedTime).HasComment("订单创建日期").IsRequired(false);
            builder.Property(p => p.SyncDate).HasDefaultValueSql("getDate()").HasComment("同步日期").IsRequired(true);

            builder.OwnsMany(p => p.OrderTransactions, p =>
            {
                p.HasKey(v => v.Id);
                p.Property(v => v.Id).ValueGeneratedOnAdd().HasComment("主键Id");
                p.Property(v => v.TransactionID).HasMaxLength(50).IsRequired(true);
                p.Property(v => v.OrderLineItemID).HasMaxLength(50).IsRequired(true);
                p.Property(v => v.SiteCode).HasMaxLength(20).HasComment("站点").IsRequired(false);
                p.Property(v => v.Title).HasComment("标题").IsRequired(false);
                p.Property(v => v.ConditionID).HasComment("条件性ID").IsRequired(false);
                p.Property(v => v.ConditionDisplayName).HasMaxLength(500).HasComment("条件性ID").IsRequired(false);
                p.Property(v => v.QuantityPurchased).HasComment("购买数").IsRequired(true);
                p.OwnsOne(v => v.TransactionPrice, v =>
                {
                    v.Property(v => v.Value).HasPrecision(18, 3).HasColumnName("TransactionPrice").IsRequired(true);
                    v.Property(v => v.Currency).HasMaxLength(10).HasColumnName("TransactionPriceCurrency").IsRequired(false);
                });

                p.OwnsOne(v => v.Status, v =>
                {
                    v.Property(v => v.PaymentHoldStatus).HasMaxLength(20).HasComment("待付款状态").IsRequired(false);
                    v.Property(v => v.InquiryStatus).HasMaxLength(40).HasComment("查询状态").IsRequired(false);
                    v.Property(v => v.ReturnStatus).HasMaxLength(40).HasComment("退回状态").IsRequired(false);
                });

                p.OwnsOne(v => v.ShippingServiceSelected, v =>
                {
                    v.Property(v => v.ShippingService).HasMaxLength(300).HasComment("运输服务").IsRequired(false);
                    v.OwnsOne(v => v.ShippingServiceCost, v =>
                    {
                        v.Property(v => v.Value).HasPrecision(18, 3).HasColumnName("ShippingServiceCost").IsRequired(true);
                        v.Property(v => v.Currency).HasMaxLength(10).HasColumnName("ShippingServiceCostCurrency").IsRequired(false);
                    });
                    v.Property(p => p.ShippingServicePriority).IsRequired(false);
                    v.Property(v => v.ExpeditedService).HasComment("是否加急服务").IsRequired(false);
                    v.Property(v => v.ShippingTimeMin).HasComment("装运时间").IsRequired(false);
                    v.Property(v => v.ShippingTimeMax).HasComment("装运时间").IsRequired(false);

                    v.OwnsOne(v => v.ShippingPackage, m =>
                    {
                        m.Property(v => v.StoreID).HasMaxLength(100).IsRequired(false);
                        m.Property(v => v.ShippingTrackingEvent).HasMaxLength(200).IsRequired(false);
                        m.Property(v => v.ScheduledDeliveryTimeMin).IsRequired(false);
                        m.Property(v => v.ScheduledDeliveryTimeMax).IsRequired(false);
                        m.Property(v => v.ActualDeliveryTime).IsRequired(false);
                        m.Property(v => v.EstimatedDeliveryTimeMin).HasComment("预计发货时间").IsRequired(false);
                        m.Property(v => v.EstimatedDeliveryTimeMax).HasComment("预计发货时间").IsRequired(false);
                    });
                });
                p.Property(p => p.CreatedDate).HasComment("交易创建日期，非系统日期").IsRequired(false);
            });

            builder.OwnsOne(p => p.Total, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("Total").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("TotalCurrency").IsRequired(false);
            });

            builder.OwnsOne(p => p.Subtotal, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("Subtotal").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("SubtotalCurrency").IsRequired(false);
            });

            builder.OwnsOne(p => p.CheckoutStatus, v =>
            {
                v.Property(p => p.EBayPaymentStatus).HasMaxLength(40).HasComment("EBayPaymentStatus").IsRequired(false);
                v.Property(p => p.PaymentMethod).HasMaxLength(30).HasComment("结账方式").IsRequired(false);
                v.Property(p => p.Status).HasMaxLength(10).HasComment("状态").IsRequired(false);
                v.Property(p => p.IntegratedMerchantCreditCardEnabled).HasMaxLength(10).IsRequired(false);
                v.Property(p => p.PaymentInstrument).HasMaxLength(30).IsRequired(false);
                v.Property(p => p.LastModifiedTime).HasMaxLength(30).HasComment("最后更新时间，非系统时间").IsRequired(false);
            });

            builder.OwnsOne(p => p.ShippingDetail, v =>
            {
                v.OwnsOne(v => v.SalesTax, s =>
                {
                    s.Property(v => v.SalesTaxPercent).HasComment("营业税率").IsRequired(true);
                    s.Property(v => v.SalesTaxState).HasMaxLength(300).IsRequired(false);
                    s.OwnsOne(p => p.SalesTaxAmount, v =>
                    {
                        v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("SalesTaxAmount").IsRequired(true);
                        v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("SalesTaxAmountCurrency").IsRequired(false);
                    });
                });
                v.Property(v => v.SellingManagerSalesRecordNumber).IsRequired(false);
                v.Property(v => v.GetItFast).HasComment("是否加急").IsRequired(false);
                v.OwnsMany(v => v.ShippingServiceOptions, v =>
                {
                    v.HasKey(v => v.Id);
                    v.Property(v => v.Id).ValueGeneratedOnAdd().HasComment("主键Id");
                    v.Property(v => v.ShippingService).HasMaxLength(300).HasComment("运输服务").IsRequired(false);
                    v.OwnsOne(v => v.ShippingServiceCost, v =>
                    {
                        v.Property(v => v.Value).HasPrecision(18, 3).HasDefaultValue(0M).HasColumnName("ShippingServiceCost").IsRequired(true);
                        v.Property(v => v.Currency).HasMaxLength(10).HasColumnName("ShippingServiceCostCurrency").IsRequired(false);
                    });
                    v.Property(p => p.ShippingServicePriority).IsRequired(false);
                    v.Property(v => v.ExpeditedService).HasComment("是否加急服务").IsRequired(false);
                    v.Property(v => v.ShippingTimeMin).HasComment("装运时间").IsRequired(false);
                    v.Property(v => v.ShippingTimeMax).HasComment("装运时间").IsRequired(false);

                    v.OwnsOne(v => v.ShippingPackage, m =>
                    {
                        m.Property(v => v.StoreID).HasMaxLength(100).IsRequired(false);
                        m.Property(v => v.ShippingTrackingEvent).HasMaxLength(200).IsRequired(false);
                        m.Property(v => v.ScheduledDeliveryTimeMin).IsRequired(false);
                        m.Property(v => v.ScheduledDeliveryTimeMax).IsRequired(false);
                        m.Property(v => v.ActualDeliveryTime).IsRequired(false);
                        m.Property(v => v.EstimatedDeliveryTimeMin).HasComment("预计发货时间").IsRequired(false);
                        m.Property(v => v.EstimatedDeliveryTimeMax).HasComment("预计发货时间").IsRequired(false);
                    });

                });
            });

            builder.OwnsOne(p => p.ShippingAddress, v =>
            {
                v.Property(v => v.AddressID).HasMaxLength(100).HasComment("地址ID，同样具有唯一性，非系统ID").IsRequired(true);
                v.Property(v => v.Name).HasMaxLength(300).HasComment("地址名称").IsRequired(false);
                v.Property(v => v.Street).HasMaxLength(300).HasComment("街道1").IsRequired(false);
                v.Property(v => v.Street1).HasMaxLength(300).HasComment("街道1").IsRequired(false);
                v.Property(v => v.Street2).HasMaxLength(300).HasComment("街道2").IsRequired(false);
                v.Property(v => v.CityName).HasMaxLength(300).HasComment("城市名称").IsRequired(false);
                v.Property(v => v.StateOrProvince).HasMaxLength(300).HasComment("地址ID，同样具有唯一性，非系统ID").IsRequired(false);
                v.Property(v => v.Country).HasMaxLength(10).HasComment("地址ID，同样具有唯一性，非系统ID").IsRequired(false);
                v.Property(v => v.CountryName).HasMaxLength(300).HasComment("国家名称").IsRequired(false);
                v.Property(v => v.Phone).HasMaxLength(20).HasComment("电话号码").IsRequired(false);
                v.Property(v => v.PostalCode).HasMaxLength(100).HasComment("邮编").IsRequired(false);
                v.Property(v => v.AddressOwner).HasMaxLength(10).HasComment("地址所属者").IsRequired(false);
            });

            builder.OwnsOne(p => p.ShippingServiceSelected, v =>
            {
                v.Property(v => v.ShippingService).HasMaxLength(300).HasComment("运输服务").IsRequired(false);
                v.OwnsOne(v => v.ShippingServiceCost, v =>
                {
                    v.Property(v => v.Value).HasPrecision(18, 3).HasColumnName("ShippingServiceCost").IsRequired(true);
                    v.Property(v => v.Currency).HasMaxLength(10).HasColumnName("AmountSavedCurrency").IsRequired(false);
                });
                v.Property(p => p.ShippingServicePriority).IsRequired(false);
                v.Property(v => v.ExpeditedService).HasComment("是否加急服务").IsRequired(false);
                v.Property(v => v.ShippingTimeMin).HasComment("装运时间").IsRequired(false);
                v.Property(v => v.ShippingTimeMax).HasComment("装运时间").IsRequired(false);

                v.OwnsOne(v => v.ShippingPackage, m =>
                {
                    m.Property(v => v.StoreID).HasMaxLength(100).IsRequired(false);
                    m.Property(v => v.ShippingTrackingEvent).HasMaxLength(200).IsRequired(false);
                    m.Property(v => v.ScheduledDeliveryTimeMin).IsRequired(false);
                    m.Property(v => v.ScheduledDeliveryTimeMax).IsRequired(false);
                    m.Property(v => v.ActualDeliveryTime).IsRequired(false);
                    m.Property(v => v.EstimatedDeliveryTimeMin).HasComment("预计发货时间 Min").IsRequired(false);
                    m.Property(v => v.EstimatedDeliveryTimeMax).HasComment("预计发货时间 Max").IsRequired(false);
                });

            });

        }
    }
}
