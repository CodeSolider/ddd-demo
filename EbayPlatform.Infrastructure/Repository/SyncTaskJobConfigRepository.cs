using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Core.Dependency;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Repository
{
    /// <summary>
    /// 同步任务作业配置仓储
    /// </summary>
    public class SyncTaskJobConfigRepository : Repository<SyncTaskJobConfig, int, EbayPlatformDbContext>,
        ISyncTaskJobConfigRepository, IDependency
    {
        public SyncTaskJobConfigRepository(EbayPlatformDbContext dbContext)
            : base(dbContext) { }

        /// <summary>
        /// 获取所有的任务配置作业数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<SyncTaskJobConfig>> GetSyncTaskJobConfigListAsync(CancellationToken cancellationToken)
        {
            return await this.NoTrackingQueryable.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 检查任务名称是否重复
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public bool CheckJobName(string jobName)
        {
            return this.NoTrackingQueryable.Any(o => o.JobName == jobName);
        }

        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>

        public async Task<SyncTaskJobConfig> GetSyncTaskJobConfigByJobNameAsync(string jobName)
        {
            return await this.NoTrackingQueryable
                             .FirstOrDefaultAsync(o => o.JobName.Equals(jobName))
                             .ConfigureAwait(false);
        }

    }
}
