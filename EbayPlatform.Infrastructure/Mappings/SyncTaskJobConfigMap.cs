﻿using EbayPlatform.Domain.Models;
using EbayPlatform.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayPlatform.Infrastructure.Mappings
{
    public class SyncTaskJobConfigMap : IEntityTypeConfiguration<SyncTaskJobConfig>
    {
        public void Configure(EntityTypeBuilder<SyncTaskJobConfig> builder)
        {
            builder.ToTable("SyncTaskJobConfig").HasComment("同步任务作业配置");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.JobName).HasMaxLength(100).HasComment("Job名称");
            builder.Property(p => p.JobDesc).HasMaxLength(200).IsRequired(false).HasComment("Job描述");
            builder.Property(p => p.JobAssemblyName).HasMaxLength(300).HasComment("程序集名称");
            builder.Property(p => p.Cron).HasMaxLength(50).HasComment("Cron");
            builder.Property(p => p.CronDesc).HasMaxLength(300).IsRequired(false).HasComment("Cron描述");
            builder.Property(p => p.JobStatus).IsRequired(true).HasComment("运行状态");
            builder.Property(p => p.CreateDate).HasDefaultValueSql("getDate()").IsRequired(true).HasComment("创建日期");
            builder.Property(p => p.ModifyDate).ValueGeneratedOnUpdate().IsRequired(false).HasComment("更新日期").ValueGeneratedOnUpdate();
            //one to many
            builder.OwnsMany(p => p.ShopTasks, p =>
            {
                p.HasKey(s => s.Id);
                p.Property(s => s.Id).ValueGeneratedOnAdd();
                p.Property(s => s.ShopName).HasMaxLength(100).IsRequired(true).HasComment("店铺名称");
                p.Property(s => s.ParamValue).HasMaxLength(4000).IsRequired(false).HasComment("参数值");
            });

            builder.HasQueryFilter(o => o.JobStatus == JobStatusType.UnExecute);
        }
    }
}
