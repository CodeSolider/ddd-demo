using EbayPlatform.Domain.AggregateModel;
using EbayPlatform.Domain.AggregateModel.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EbayPlatform.Infrastructure.Mappings
{
    public class SystemLogMap : IEntityTypeConfiguration<SystemLog>
    {
        public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            builder.ToTable(nameof(SystemLog)).HasComment("系统日志表");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.ObjectId).HasMaxLength(50).HasComment("外键Id");
            builder.Property(p => p.LogType).HasConversion(v => v.ToString(), v => (LogType)Enum.Parse(typeof(LogType), v)).HasComment("日志类型").IsRequired(true);
            builder.Property(p => p.Content).HasMaxLength(4000).HasComment("内容").IsRequired(false);
            builder.Property(p => p.CreateDate).HasDefaultValueSql("getDate()").HasComment("创建日期").IsRequired(true);
        }
    }
}
