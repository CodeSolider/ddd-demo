using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.AggregateModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.Interfaces
{
    /// <summary>
    ///  同步任务作业配置仓储接口
    /// </summary>
    public interface ISyncTaskJobConfigRepository : IRepository<SyncTaskJobConfig, int>
    {
        /// <summary>
        /// 获取所有的任务配置信息
        /// </summary>
        /// <param name="ignoreQueryFilter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<SyncTaskJobConfig>> GetSyncTaskJobConfigListAsync(bool ignoreQueryFilter = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 检查任务名称是否重复
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        bool CheckJobName(string jobName);

        /// <summary>
        /// 根据Id获取任务配置作业数据
        /// </summary>
        /// <param name="syncTaskJobConfigId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SyncTaskJobConfig> GetByIdAsync(int syncTaskJobConfigId);

        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SyncTaskJobConfig> GetByNameAsync(string jobName,
          CancellationToken cancellationToken = default);
    }
}
