using EbayPlatform.Domain.Models;
using EbayPlatform.Domain.Models.Accounts;
using EbayPlatform.Domain.Models.Listing;
using EbayPlatform.Domain.Models.Orders;
using EbayPlatform.Infrastructure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EbayPlatform.Infrastructure.Context
{
    public class EbayPlatformDbContext : EFContext
    {
        public EbayPlatformDbContext(DbContextOptions<EbayPlatformDbContext> options, IMediator mediator) : base(options, mediator)
        {
        }

        #region 配置、日志信息
        /// <summary>
        /// 同步任务作业配置类
        /// </summary>
        public DbSet<SyncTaskJobConfig> SyncTaskJobConfigs { get; set; }

        /// <summary>
        /// 系统日志
        /// </summary>
        public DbSet<SystemLog> SystemLogs { get; set; }
        #endregion

        #region  Task

        /// <summary>
        /// 订单
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Account> Accounts { get; set; }

        #endregion

        /// <summary>
        /// 应用创建
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EbayPlatformDbContext).Assembly);
        }
    }
}
