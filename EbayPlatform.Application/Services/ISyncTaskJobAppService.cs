using EbayPlatform.Application.Dtos;
using EbayPlatform.Domain.Models;
using EbayPlatform.Domain.Models.Enums;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 同步任务作业接口
    /// </summary>
    public interface ISyncTaskJobAppService
    {
        /// <summary>
        /// 根据任务状态获取任务配置信息
        /// </summary>
        /// <param name="jobStatus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<SyncTaskJobConfig>> GetListByJobStatusAsync(JobStatusType? jobStatus = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取所有的任务配置数据
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<SyncTaskJobConfig>> GetSyncTaskJobConfigListAsync(bool ignoreQueryFilter = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据任务Id获取任务配置作业数据
        /// </summary>
        /// <param name="syncTaskJobConfigId"></param> 
        /// <returns></returns>
        Task<SyncTaskJobConfig> GetSyncTaskJobConfigByIdAsync(int syncTaskJobConfigId);

        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SyncTaskJobConfig> GetSyncTaskJobConfigByNameAsync(string jobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns></returns>
        Task<int> CreateSyncTaskJobAsync(SyncTaskJobConfigDto syncTaskJobConfigDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新店铺信息
        /// </summary>
        /// <param name="syncTaskJobConfigs"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UpdateShopTaskAsync(List<SyncTaskJobConfig> syncTaskJobConfigs, CancellationToken cancellationToken = default);
    }
}
