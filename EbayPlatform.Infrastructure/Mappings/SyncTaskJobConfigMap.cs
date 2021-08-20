using EbayPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayPlatform.Infrastructure.Mappings
{
    public class SyncTaskJobConfigMap : IEntityTypeConfiguration<SyncTaskJobConfig>
    {
        public void Configure(EntityTypeBuilder<SyncTaskJobConfig> builder)
        {
            builder.ToTable("SyncTaskJobConfig").HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.JobName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.JobDesc).HasMaxLength(300).IsRequired(false);
            builder.Property(p => p.JobClassFullName).HasMaxLength(300).IsRequired(true);
            builder.Property(p => p.Cron).HasMaxLength(50).IsRequired(true);
            builder.Property(p => p.CronDesc).HasMaxLength(300).IsRequired(false);
            builder.Property(p => p.IsRunning).HasDefaultValue(false).IsRequired();
            builder.Property(p => p.JobStatus).IsRequired(true);
            builder.Property(p => p.CreateDate).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
            builder.Property(p => p.ModifyDate).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnUpdate().IsRequired(false); 
            //one to many
            builder.OwnsMany(p => p.SyncTaskJobParams, p =>
            {
                p.WithOwner();
                p.Property(p => p.ShopName).HasMaxLength(50).IsRequired();
                p.Property(p => p.ParamValue).IsRequired(false);
            });

            builder.HasQueryFilter(p => p.IsRunning);
        }
    }
}
