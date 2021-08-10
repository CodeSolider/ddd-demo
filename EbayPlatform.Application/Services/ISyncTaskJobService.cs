using EbayPlatform.Application.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 同步任务作业接口
    /// </summary>
    public interface ISyncTaskJobService
    {
        /// <summary>
        /// 执行所有任务
        /// </summary>
        void ExecuteAllTask();

        /// <summary>
        /// 添加一个新任务
        /// </summary>
        /// <returns></returns>
        Task<int> CreateSyncTaskJobAsync(SyncTaskJobConfigDto syncTaskJobConfigDto, CancellationToken cancellationToken);
    }
}
