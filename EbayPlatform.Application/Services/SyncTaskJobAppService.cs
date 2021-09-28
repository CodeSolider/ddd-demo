using EbayPlatform.Domain.Interfaces;
using MediatR;
using Quartz;
using EbayPlatform.Domain.Commands.SyncTaskJobConfig;
using Mapster;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Reflection;
using EbayPlatform.Domain.Models;
using EbayPlatform.Infrastructure.Core;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using EbayPlatform.Application.Dtos;
using Microsoft.Extensions.Logging;
using EbayPlatform.Infrastructure.Core.Quartz;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 同步任务作业服务
    /// </summary>
    public class SyncTaskJobAppService : ISyncTaskJobAppService, IDependency
    {
        private readonly IMediator _mediator;
        private readonly ISyncTaskJobConfigRepository _syncTaskJobConfigRepository;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<SyncTaskJobAppService> _logger;
        public SyncTaskJobAppService(IMediator mediator,
            ISyncTaskJobConfigRepository syncTaskJobConfigRepository,
            ISchedulerFactory schedulerFactory,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<SyncTaskJobAppService> logger)
        {
            _mediator = mediator;
            _schedulerFactory = schedulerFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _syncTaskJobConfigRepository = syncTaskJobConfigRepository;
            _logger = logger;
        }

        /// <summary>
        /// 根据任务状态获取任务配置信息
        /// </summary>
        /// <returns></returns>
        public Task<List<SyncTaskJobConfig>> GetSyncTaskJobConfigListByAsync(CancellationToken cancellationToken = default)
        {
            return _syncTaskJobConfigRepository
                   .GetSyncTaskJobConfigListByAsync(cancellationToken);
        }


        #region Quartz Job
        /// <summary>
        /// 执行所有任务
        /// </summary>
        public async Task ExecuteAllTaskAysnc(CancellationToken cancellationToken = default)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);
            var syncTaskJobConfigList = await GetSyncTaskJobConfigListByAsync(cancellationToken).ConfigureAwait(false);
            syncTaskJobConfigList.ForEach(async syncTaskJobConfigItem =>
            {
                await StartJob(scheduler, syncTaskJobConfigItem)
                      .ConfigureAwait(false);
            });
            scheduler.JobFactory = new JobFactory(_serviceScopeFactory);
            await scheduler.Start(cancellationToken).ConfigureAwait(false);
        }


        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="scheduler">调度器</param>
        /// <param name="syncTaskJobConfig">任务配置信息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task StartJob(IScheduler scheduler,
           SyncTaskJobConfig syncTaskJobConfig,
           CancellationToken cancellationToken = default)
        {
            Assembly assembly = Assembly.Load(syncTaskJobConfig.JobAssemblyName);
            if (assembly == null)
            {
                throw new ArgumentNullException($"未能加载程序集[{syncTaskJobConfig.JobAssemblyName}]");
            }

            TypeInfo typeInfo = assembly.DefinedTypes.FirstOrDefault(o => o.Name == syncTaskJobConfig.JobName);
            if (typeInfo == null)
            {
                _logger.LogWarning($"找不到任务名称[{syncTaskJobConfig.JobName}]");
                return;
            }

            IJobDetail job = JobBuilder.Create(typeInfo)
                                       .WithIdentity(syncTaskJobConfig.JobName)
                                       .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                             .WithIdentity($"{syncTaskJobConfig.JobName}.trigger")
                                             .StartNow()
                                             .WithCronSchedule(syncTaskJobConfig.Cron)
                                             .Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken)
                           .ConfigureAwait(false);

        }


        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns></returns>
        public async Task<int> CreateSyncTaskJobAsync(SyncTaskJobConfigDto syncTaskJobConfigDto,
            CancellationToken cancellationToken = default)
        {
            int syncTaskJobConfigId = await _mediator
                                             .Send(new CreateSyncTaskJobConfigCommand(syncTaskJobConfigDto.JobName, syncTaskJobConfigDto.JobDesc,
                                                                                      syncTaskJobConfigDto.JobAssemblyName, syncTaskJobConfigDto.Cron,
                                                                                      syncTaskJobConfigDto.CronDesc, syncTaskJobConfigDto.SyncTaskJobParams.Adapt<List<ShopTask>>()),
                                                   cancellationToken)
                                              .ConfigureAwait(false);



            IScheduler scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);
            await StartJob(scheduler, await GetSyncTaskJobConfigByIdAsync(syncTaskJobConfigId).ConfigureAwait(false), cancellationToken)
                  .ConfigureAwait(false);
            return syncTaskJobConfigId;
        }
        #endregion


        /// <summary>
        /// 根据任务Id获取任务配置作业数据
        /// </summary>
        /// <param name="syncTaskJobConfigId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<SyncTaskJobConfig> GetSyncTaskJobConfigByIdAsync(int syncTaskJobConfigId)
        {
            return _syncTaskJobConfigRepository
                   .GetSyncTaskJobConfigByIdAsync(syncTaskJobConfigId);
        }

        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<SyncTaskJobConfig> GetSyncTaskJobConfigByName(string jobName,
           CancellationToken cancellationToken = default)
        {
            return _syncTaskJobConfigRepository
                    .GetSyncTaskJobConfigByNameAsync(jobName, cancellationToken);
        }


        public Task<bool> UpdateShopTaskAsync(List<SyncTaskJobConfig> syncTaskJobConfigs, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new UpdateSyncTaskJobConfigCommand(syncTaskJobConfigs), cancellationToken);
        }


        /// <summary>
        /// 手动释放
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
