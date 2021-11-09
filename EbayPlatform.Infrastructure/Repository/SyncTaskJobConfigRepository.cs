using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models;
using EbayPlatform.Domain.Models.Enums;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Core.Extensions;
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
        /// 根据状态获取任务配置信息
        /// </summary>
        /// <returns></returns>
        public Task<List<SyncTaskJobConfig>> GetListByJobStatusAsync(JobStatusType? jobStatus = null,
            CancellationToken cancellationToken = default)
        {
            return this.DbContext.SyncTaskJobConfigs
                       .WhereIf(jobStatus.HasValue, o => o.JobStatus == jobStatus)
                       .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 获取所有的任务配置数据
        /// </summary>
        /// <param name="ignoreQueryFilter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<SyncTaskJobConfig>> GetSyncTaskJobConfigListAsync(bool ignoreQueryFilter = true, CancellationToken cancellationToken = default)
        {
            return ignoreQueryFilter ? this.DbContext.SyncTaskJobConfigs
                                                .IgnoreQueryFilters()
                                                .ToListAsync(cancellationToken)
                                    : this.DbContext.SyncTaskJobConfigs
                                          .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 检查任务名称是否重复
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public bool CheckJobName(string jobName)
        {
            return this.DbContext.SyncTaskJobConfigs.Any(o => o.JobName == jobName);
        }

        /// <summary>
        /// 根据Id获取任务配置作业数据
        /// </summary>
        /// <param name="syncTaskJobConfigId"></param>
        /// <returns></returns>
        public Task<SyncTaskJobConfig> GetByIdAsync(int syncTaskJobConfigId)
        {
            return this.GetAsync(syncTaskJobConfigId);
        }

        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<SyncTaskJobConfig> GetByNameAsync(string jobName, CancellationToken cancellationToken = default)
        {
            return this.DbContext.SyncTaskJobConfigs.IgnoreQueryFilters().FirstOrDefaultAsync(o => o.JobName.Equals(jobName), cancellationToken);
        }
    }
}
