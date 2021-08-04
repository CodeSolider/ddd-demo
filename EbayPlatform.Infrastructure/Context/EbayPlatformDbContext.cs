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

        /// <summary>
        /// 学生类
        /// </summary>
       // public DbSet<Student> Students { get; set; }


        #region 同步任务
        /// <summary>
        /// 同步任务作业配置类
        /// </summary>
        public DbSet<SyncTaskJobConfig> SyncTaskJobConfigs { get; set; }

        public DbSet<SyncTaskJobParam> SyncTaskJobParams { get; set; }
        #endregion

        /// <summary>
        /// 应用创建
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Student>();

            //modelBuilder.ApplyConfiguration(new StudentMap());
            #region 同步任务
            modelBuilder.ApplyConfiguration(new SyncTaskJobConfigMap());
            modelBuilder.ApplyConfiguration(new SyncTaskJobParamMap());
            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
