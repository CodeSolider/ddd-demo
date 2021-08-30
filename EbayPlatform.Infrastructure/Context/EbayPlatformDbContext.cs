using EbayPlatform.Domain.Models;
using EbayPlatform.Infrastructure.Core;
using EbayPlatform.Infrastructure.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EbayPlatform.Infrastructure.Context
{
    public class EbayPlatformDbContext : EFContext
    {

        public EbayPlatformDbContext(DbContextOptions options,
            IMediator mediator)
            : base(options, mediator)
        {
        }

        #region 同步任务
        /// <summary>
        /// 同步任务作业配置类
        /// </summary>
        public DbSet<SyncTaskJobConfig> SyncTaskJobConfigs { get; set; }
        #endregion

        /// <summary>
        /// 系统日志
        /// </summary>
        public DbSet<SystemLog> SystemLogs { get; set; }

        /// <summary>
        /// 应用创建
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 同步任务
            modelBuilder.ApplyConfiguration(new SyncTaskJobConfigMap());
            #endregion
            modelBuilder.ApplyConfiguration(new SystemLogMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
