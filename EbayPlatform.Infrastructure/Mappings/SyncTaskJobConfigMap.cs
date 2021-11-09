using EbayPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayPlatform.Infrastructure.Mappings
{
    public class SyncTaskJobConfigMap : IEntityTypeConfiguration<SyncTaskJobConfig>
    {
        public void Configure(EntityTypeBuilder<SyncTaskJobConfig> builder)
        {
            builder.ToTable(nameof(SyncTaskJobConfig)).HasComment("同步任务作业配置");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.JobName).HasMaxLength(100).HasComment("Job名称").IsRequired(false);
            builder.Property(p => p.JobDesc).HasMaxLength(200).HasComment("Job描述").IsRequired(false);
            builder.Property(p => p.JobAssemblyName).HasMaxLength(150).HasComment("程序集名称").IsRequired(false);
            builder.Property(p => p.Cron).HasMaxLength(50).HasComment("Cron").IsRequired(false);
            builder.Property(p => p.CronDesc).HasMaxLength(300).HasComment("Cron描述").IsRequired(false);
            builder.Property(p => p.JobStatus).IsRequired(true).HasComment("运行状态");
            builder.Property(p => p.CreateDate).HasDefaultValueSql("getDate()").HasComment("创建日期").IsRequired(true);
            builder.Property(p => p.ModifyDate).ValueGeneratedOnUpdate().HasComment("更新日期").IsRequired(false);
            builder.Property(p => p.Enable).HasDefaultValue(false).IsRequired(true);
            builder.Property(p => p.SyncErp).HasDefaultValue(false).IsRequired(true);

            //one to many
            builder.OwnsMany(p => p.ShopTasks, p =>
            {
                p.HasKey(s => s.Id);
                p.Property(s => s.Id).ValueGeneratedOnAdd();
                p.Property(s => s.ShopName).HasMaxLength(100).HasComment("店铺名称").IsRequired(true);
                p.Property(s => s.ParamValue).HasMaxLength(4000).HasComment("参数值").IsRequired(false);
            });

            builder.HasQueryFilter(o => o.Enable);
        }
    }
}
