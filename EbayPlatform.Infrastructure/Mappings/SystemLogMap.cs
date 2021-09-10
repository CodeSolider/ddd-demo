using EbayPlatform.Domain.Models;
using EbayPlatform.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EbayPlatform.Infrastructure.Mappings
{
    public class SystemLogMap : IEntityTypeConfiguration<SystemLog>
    {
        public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            builder.ToTable("SystemLog").HasComment("系统日志表");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.ObjectId).HasComment("外键Id");
            builder.Property(p => p.LogType).HasConversion(v => v.ToString(), v => (LogType)Enum.Parse(typeof(LogType), v))
                   .HasComment("日志类型");
            builder.Property(p => p.Content).HasMaxLength(4000).IsRequired(false).HasComment("内容");
            builder.Property(p => p.CreateDate).HasDefaultValueSql("getDate()").HasComment("创建日期");
        }
    }
}
