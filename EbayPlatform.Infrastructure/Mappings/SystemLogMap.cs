using EbayPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EbayPlatform.Infrastructure.Mappings
{
    public class SystemLogMap : IEntityTypeConfiguration<SystemLog>
    {
        public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            builder.ToTable("SystemLog").HasComment("系统日志表").HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasComment("主键Id");
            builder.Property(p => p.ObjectId).IsRequired(true).HasComment("外键Id");
            builder.Property(p => p.LogType).IsRequired(true).HasComment("日志类型");
            builder.Property(p => p.Content).HasMaxLength(4000).IsRequired(false).HasComment("内容");
            builder.Property(p => p.CreateDate).IsRequired(true).HasComment("创建日期");
        }
    }
}
