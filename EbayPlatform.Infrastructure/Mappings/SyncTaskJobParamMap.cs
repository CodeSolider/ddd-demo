using EbayPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayPlatform.Infrastructure.Mappings
{
    /// <summary>
    /// 参数配置
    /// </summary>
    public class SyncTaskJobParamMap : IEntityTypeConfiguration<SyncTaskJobParam>
    {
        public void Configure(EntityTypeBuilder<SyncTaskJobParam> builder)
        {
            builder.ToTable("SyncTaskJobParam").HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.ShopName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.ParamValue).IsRequired(false);
        }
    }
}
