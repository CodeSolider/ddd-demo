using EbayPlatform.Domain.AggregateModel.AccountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayPlatform.Infrastructure.Mappings
{
    public class AccountMap : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable(nameof(Account)).HasComment("账单");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.AccountID).HasMaxLength(50).HasComment("账单 ID").IsRequired(true);
            builder.Property(p => p.ShopName).HasMaxLength(50).HasComment("店铺编号，来自ERP").IsRequired(true);
            builder.Property(p => p.CurrencyCode).HasMaxLength(10).HasComment("币种").IsRequired(true);
            builder.Property(p => p.AccountState).HasMaxLength(10).HasComment("账单状态").IsRequired(true);
            builder.OwnsOne(p => p.InvoicePayment, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("InvoicePayment").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("InvoicePaymentCurrency").IsRequired(false);
            });
            builder.OwnsOne(p => p.InvoiceCredit, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("InvoiceCredit").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("InvoiceCreditCurrency").IsRequired(false);
            });
            builder.OwnsOne(p => p.InvoiceNewFee, v =>
            {
                v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("InvoiceNewFee").IsRequired(true);
                v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("InvoiceNewFeeCurrency").IsRequired(false);
            });
            builder.OwnsOne(p => p.AdditionalAccount, v =>
            {
                v.OwnsOne(p => p.Balance, v =>
                {
                    v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("Balance").IsRequired(true);
                    v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("BalanceCurrency").IsRequired(false);
                });
                v.Property(p => p.CurrencyCode).HasMaxLength(10).HasComment("币种").HasDefaultValue(null).IsRequired(true);
                v.Property(p => p.AccountCode).HasMaxLength(50).HasComment("账户编号").HasDefaultValue(null).IsRequired(false);
            });
            builder.OwnsMany(p => p.AccountDetails, p =>
            {
                p.HasKey(v => v.Id);
                p.Property(v => v.Id).ValueGeneratedOnAdd().HasComment("主键Id");
                p.Property(v => v.RefNumber).HasMaxLength(50).IsRequired(true);
                p.Property(v => v.ItemID).HasMaxLength(50).IsRequired(true);
                p.Property(v => v.Date).HasColumnType("Date").IsRequired(true);
                p.Property(v => v.AccountType).HasMaxLength(50).IsRequired(true);
                p.Property(v => v.Title).HasMaxLength(50).IsRequired(false);
                p.Property(v => v.Description).IsRequired(false);
                p.OwnsOne(p => p.Balance, v =>
                {
                    v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("Balance").IsRequired(true);
                    v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("BalanceCurrency").IsRequired(false);
                });
                p.OwnsOne(p => p.GrossDetailAmount, v =>
                {
                    v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("GrossDetailAmount").IsRequired(true);
                    v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("GrossDetailAmountCurrency").IsRequired(false);
                });
                p.OwnsOne(p => p.ConversionRate, v =>
                {
                    v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("ConversionRate").IsRequired(true);
                    v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("ConversionRateCurrency").IsRequired(false);
                });
                p.OwnsOne(p => p.NetDetailAmount, v =>
                {
                    v.Property(p => p.Value).HasPrecision(18, 3).HasColumnName("NetDetailAmount").IsRequired(true);
                    v.Property(p => p.Currency).HasMaxLength(10).HasColumnName("NetDetailAmountCurrency").IsRequired(false);
                });
                p.Property(v => v.VATPercent).HasPrecision(18, 3).IsRequired(true);
                p.Property(v => v.OrderLineItemID).HasMaxLength(50).IsRequired(false);
                p.Property(v => v.TransactionID).HasMaxLength(50).IsRequired(false);
                p.Property(v => v.ReceivedTopRatedDiscount).HasDefaultValue(false).IsRequired(true);
                p.HasIndex(v => new { v.RefNumber, v.ItemID, v.Date, v.AccountType });
            });
            builder.Property(p => p.SyncDate).HasDefaultValueSql("getDate()").HasComment("同步日期").IsRequired(true);
        }
    }
}
