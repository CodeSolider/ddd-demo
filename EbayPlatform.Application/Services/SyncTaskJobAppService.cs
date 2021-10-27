using EbayPlatform.Domain.Interfaces;
using MediatR;
using EbayPlatform.Domain.Commands.SyncTaskJobConfig;
using System.Threading.Tasks;
using System.Threading;
using System;
using EbayPlatform.Domain.Models;
using System.Collections.Generic;
using EbayPlatform.Application.Dtos;
using Mapster;
using EbayPlatform.Domain.Models.Enums;
using EbayPlatform.Domain.Core.Abstractions;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 同步任务作业服务
    /// </summary>
    public class SyncTaskJobAppService : ISyncTaskJobAppService, IDependency
    {
        private readonly IMediator _mediator;
        private readonly ISyncTaskJobConfigRepository _syncTaskJobConfigRepository;
        public SyncTaskJobAppService(IMediator mediator,
            ISyncTaskJobConfigRepository syncTaskJobConfigRepository)
        {
            _mediator = mediator;
            _syncTaskJobConfigRepository = syncTaskJobConfigRepository;
        }

        /// <summary>
        /// 根据状态获取任务配置信息
        /// </summary>
        /// <returns></returns>
        public Task<List<SyncTaskJobConfig>> GetListByJobStatusAsync(JobStatusType? jobStatus = null,
            CancellationToken cancellationToken = default)
        {
            return _syncTaskJobConfigRepository.GetListByJobStatusAsync(jobStatus, cancellationToken);
        }

        /// <summary>
        /// 获取所有的任务配置信息
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<SyncTaskJobConfig>> GetSyncTaskJobConfigListAsync(CancellationToken cancellationToken = default)
        {
            return _syncTaskJobConfigRepository.GetSyncTaskJobConfigListAsync(cancellationToken);
        }

        /// <summary>
        /// 根据任务Id获取任务配置作业数据
        /// </summary>
        /// <param name="syncTaskJobConfigId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ValueTask<SyncTaskJobConfig> GetSyncTaskJobConfigByIdAsync(int syncTaskJobConfigId)
        {
            return _syncTaskJobConfigRepository.GetByIdAsync(syncTaskJobConfigId);
        }

        /// <summary>
        /// 根据任务名称获取任务配置作业数据
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<SyncTaskJobConfig> GetSyncTaskJobConfigByNameAsync(string jobName,
           CancellationToken cancellationToken = default)
        {
            return _syncTaskJobConfigRepository.GetByNameAsync(jobName, cancellationToken);
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns></returns>
        public Task<int> CreateSyncTaskJobAsync(SyncTaskJobConfigDto syncTaskJobConfigDto,
            CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new CreateSyncTaskJobConfigCommand(syncTaskJobConfigDto.JobName, syncTaskJobConfigDto.JobDesc,
                                  syncTaskJobConfigDto.JobAssemblyName, syncTaskJobConfigDto.Cron,
                                  syncTaskJobConfigDto.CronDesc, syncTaskJobConfigDto.SyncTaskJobParams.Adapt<List<ShopTask>>()),
                            cancellationToken);
        }


        /// <summary>
        /// 更新店铺信息
        /// </summary>
        /// <param name="syncTaskJobConfigs"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> UpdateShopTaskAsync(List<SyncTaskJobConfig> syncTaskJobConfigs, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new UpdateSyncTaskJobConfigCommand(syncTaskJobConfigs), cancellationToken);
        }

#pragma warning disable CA1816 // Dispose 方法应调用 SuppressFinalize
        public void Dispose() => GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose 方法应调用 SuppressFinalize
    }
}
