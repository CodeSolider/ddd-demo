using EbayPlatform.Application.Dtos;
using EbayPlatform.Domain.Models;
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
        /// 执行所有任务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAllTaskAysnc(CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加一个新任务
        /// </summary>
        /// <returns></returns>
        Task<SyncTaskJobConfig> CreateSyncTaskJobAsync(SyncTaskJobConfigDto syncTaskJobConfigDto,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="syncTaskJobConfigId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SyncTaskJobConfig> GetSyncTaskJobConfigByIdAsync(int syncTaskJobConfigId,
          CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SyncTaskJobConfig> GetSyncTaskJobConfigByNameAsync(string jobName,
         CancellationToken cancellationToken = default);
    }
}
