using EbayPlatform.Domain.Interfaces;
using MediatR;
using EbayPlatform.Infrastructure.Quartz;
using Quartz;
using EbayPlatform.Infrastructure.Core.Dependency;
using EbayPlatform.Application.Dto;
using EbayPlatform.Domain.Commands.SyncTaskJobConfig;
using Mapster;
using System.Threading.Tasks;
using System.Threading;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 同步任务作业服务
    /// </summary>
    public class SyncTaskJobService : ISyncTaskJobService, IDependency
    {
        private readonly IMediator _mediator;
        private readonly IScheduler _scheduler;
        private readonly ISyncTaskJobConfigRepository _syncTaskJobConfigRepository;
        public SyncTaskJobService(IMediator mediator,
            IScheduler scheduler,
            ISyncTaskJobConfigRepository syncTaskJobConfigRepository)
        {
            _mediator = mediator;
            _syncTaskJobConfigRepository = syncTaskJobConfigRepository;
            _scheduler = scheduler;
        }

        /// <summary>
        /// 执行所有任务
        /// </summary>
        public void ExecuteAllTask()
        {
            var syncTaskJobConfigList = _syncTaskJobConfigRepository.GetSyncTaskJobConfigList();
            syncTaskJobConfigList.ForEach(async syncTaskJobConfigItem =>
            {
                await SchedulerProvider.StartJob(_scheduler, syncTaskJobConfigItem);
            });
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns></returns>
        public async Task<int> CreateSyncTaskJobAsync(SyncTaskJobConfigDto syncTaskJobConfigDto,
            CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(syncTaskJobConfigDto.Adapt<CreateSyncTaskJobConfigCommand>(), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteSyncTaskJobAsync(string jobName, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new DeleteSyncTaskJobConfigCommand(jobName), cancellationToken).ConfigureAwait(false);
        }


    }
}
