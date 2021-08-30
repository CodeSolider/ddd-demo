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
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.JobName).HasMaxLength(100).IsRequired().HasComment("Job名称");
            builder.Property(p => p.JobDesc).HasMaxLength(200).IsRequired(false).HasComment("Job描述");
            builder.Property(p => p.JobClassFullName).HasMaxLength(300).IsRequired(true).HasComment("Jo类全名");
            builder.Property(p => p.Cron).HasMaxLength(50).IsRequired(true).HasComment("Cron");
            builder.Property(p => p.CronDesc).HasMaxLength(300).IsRequired(false).HasComment("Cron描述");
            builder.Property(p => p.IsRunning).HasDefaultValue(false).IsRequired().HasComment("是否运行中");
            builder.Property(p => p.JobStatus).IsRequired(true).HasComment("运行状态");
            builder.Property(p => p.CreateDate).IsRequired().HasComment("创建日期");
            builder.Property(p => p.ModifyDate).ValueGeneratedOnUpdate().IsRequired(false).HasComment("更新日期");
            //one to many
            builder.OwnsMany(p => p.SyncTaskJobParams, c =>
            {
                c.WithOwner();
                c.Property(p => p.ShopName).HasMaxLength(50).IsRequired().HasComment("店铺名称");
                c.Property(p => p.ParamValue).IsRequired(false).HasComment("参数值");
            }).HasComment("同步任务作业配置表");
            //init query filter 
            builder.HasQueryFilter(p => p.IsRunning);
        }
    }
}
