using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Core.Dependency;
using System.Collections.Generic;
using System.Linq;

namespace EbayPlatform.Infrastructure.Repository
{
    /// <summary>
    /// 同步任务作业配置仓储
    /// </summary>
    public class SyncTaskJobConfigRepository : Repository<SyncTaskJobConfig, int, EbayPlatformDbContext>, ISyncTaskJobConfigRepository, IDependency
    {
        public SyncTaskJobConfigRepository(EbayPlatformDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取所有的任务配置作业数据
        /// </summary>
        /// <returns></returns>
        public List<SyncTaskJobConfig> GetSyncTaskJobConfigList()
        {
            return this.NoTrackingQueryable.ToList();
        }
    }
}
