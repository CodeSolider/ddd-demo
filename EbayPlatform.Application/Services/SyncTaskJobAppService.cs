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
using EbayPlatform.Infrastructure.Core.ThirdComponents;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 同步任务作业服务
    /// </summary>
    public class SyncTaskJobAppService : ISyncTaskJobAppService, IDependency
    {
        private readonly IMediator _mediator;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ISyncTaskJobConfigRepository _syncTaskJobConfigRepository;
        public SyncTaskJobAppService(IMediator mediator,
            ISchedulerFactory schedulerFactory,
            IServiceScopeFactory serviceScopeFactory,
            ISyncTaskJobConfigRepository syncTaskJobConfigRepository)
        {
            _mediator = mediator;
            _schedulerFactory = schedulerFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _syncTaskJobConfigRepository = syncTaskJobConfigRepository;
        }

        #region Quartz Job
        /// <summary>
        /// 执行所有任务
        /// </summary>
        public async Task ExecuteAllTaskAysnc(CancellationToken cancellationToken = default)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);
            var syncTaskJobConfigList = await _syncTaskJobConfigRepository.GetUnStartSyncTaskJobConfigListAsync(cancellationToken);
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
        public static async Task StartJob(IScheduler scheduler,
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
                throw new ArgumentNullException($"找不到任务名称[{syncTaskJobConfig.JobName}]");
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
        public async Task<SyncTaskJobConfig> CreateSyncTaskJobAsync(SyncTaskJobConfigDto syncTaskJobConfigDto,
            CancellationToken cancellationToken = default)
        {
            SyncTaskJobConfig syncTaskJobItem = await _mediator
                                                      .Send(new CreateSyncTaskJobConfigCommand(syncTaskJobConfigDto.JobName, syncTaskJobConfigDto.JobDesc,
                                                                                                syncTaskJobConfigDto.JobAssemblyName, syncTaskJobConfigDto.Cron,
                                                                                                syncTaskJobConfigDto.CronDesc, syncTaskJobConfigDto.SyncTaskJobParamsDto.Adapt<List<SyncTaskJobParam>>()),
                                                            cancellationToken)
                                                      .ConfigureAwait(false);

            IScheduler scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);
            await StartJob(scheduler, syncTaskJobItem, cancellationToken).ConfigureAwait(false);
            return syncTaskJobItem;
        }
        #endregion


        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="syncTaskJobConfigId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<SyncTaskJobConfig> GetSyncTaskJobConfigByIdAsync(int syncTaskJobConfigId,
            CancellationToken cancellationToken = default)
        {
            return await _syncTaskJobConfigRepository
                        .GetSyncTaskJobConfigByIdAsync(syncTaskJobConfigId, cancellationToken)
                        .ConfigureAwait(false);
        }

        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<SyncTaskJobConfig> GetSyncTaskJobConfigByNameAsync(string jobName, 
            CancellationToken cancellationToken = default)
        {
            return await _syncTaskJobConfigRepository.GetSyncTaskJobConfigByNameAsync(jobName);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
