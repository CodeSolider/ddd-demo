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
using System;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 同步任务作业服务
    /// </summary>
    public class SyncTaskJobService : ISyncTaskJobService, IDependency
    {
        private readonly IMediator _mediator;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISyncTaskJobConfigRepository _syncTaskJobConfigRepository;
        public SyncTaskJobService(IMediator mediator,
            ISchedulerFactory schedulerFactory,
            IServiceProvider serviceProvider,
            ISyncTaskJobConfigRepository syncTaskJobConfigRepository)
        {
            _mediator = mediator;
            _schedulerFactory = schedulerFactory;
            _serviceProvider = serviceProvider;
            _syncTaskJobConfigRepository = syncTaskJobConfigRepository;
        }

        /// <summary>
        /// 执行所有任务
        /// </summary>
        public async Task ExecuteAllTaskAysnc(CancellationToken cancellationToken)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);
            var syncTaskJobConfigList = await _syncTaskJobConfigRepository.GetSyncTaskJobConfigListAsync(cancellationToken);
            syncTaskJobConfigList.ForEach(async syncTaskJobConfigItem =>
            {
                await SchedulerProvider
                      .StartJob(scheduler, syncTaskJobConfigItem)
                      .ConfigureAwait(false);
            });
            scheduler.JobFactory = new JobFactory(_serviceProvider);
            await scheduler.Start(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns></returns>
        public async Task<int> CreateSyncTaskJobAsync(SyncTaskJobConfigDto syncTaskJobConfigDto,
            CancellationToken cancellationToken = default)
        {
            return await _mediator
                        .Send(syncTaskJobConfigDto.Adapt<CreateSyncTaskJobConfigCommand>(),
                              cancellationToken)
                        .ConfigureAwait(false);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteSyncTaskJobAsync(string jobName, CancellationToken cancellationToken = default)
        {
            return await _mediator
                        .Send(new DeleteSyncTaskJobConfigCommand(jobName),
                              cancellationToken)
                        .ConfigureAwait(false);
        }


    }
}
