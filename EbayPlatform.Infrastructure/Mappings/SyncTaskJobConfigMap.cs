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
            builder.HasMany(p => p.SyncTaskJobParams).WithOne(p => p.SyncTaskJobConfig).HasForeignKey(p => p.Id)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.HasQueryFilter(p => p.IsRunning);
        }
    }
}
