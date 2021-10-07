using EbayPlatform.Domain.Interfaces;
using MediatR;
using EbayPlatform.Domain.Commands.SyncTaskJobConfig;
using System.Threading.Tasks;
using System.Threading;
using System;
using EbayPlatform.Domain.Models;
using EbayPlatform.Infrastructure.Core;
using System.Collections.Generic;
using EbayPlatform.Application.Dtos;
using Mapster;

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
        /// 根据任务状态获取任务配置信息
        /// </summary>
        /// <returns></returns>
        public Task<List<SyncTaskJobConfig>> GetSyncTaskJobConfigListByAsync(CancellationToken cancellationToken = default)
        {
            return _syncTaskJobConfigRepository
                   .GetSyncTaskJobConfigListByAsync(cancellationToken);
        }

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

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns></returns>
        public async Task<int> CreateSyncTaskJobAsync(SyncTaskJobConfigDto syncTaskJobConfigDto,
            CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new CreateSyncTaskJobConfigCommand(syncTaskJobConfigDto.JobName, syncTaskJobConfigDto.JobDesc,
                                                                           syncTaskJobConfigDto.JobAssemblyName, syncTaskJobConfigDto.Cron,
                                                                           syncTaskJobConfigDto.CronDesc, syncTaskJobConfigDto.SyncTaskJobParams.Adapt<List<ShopTask>>()),
                                         cancellationToken)
                                  .ConfigureAwait(false);
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


        /// <summary>
        /// 手动释放
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
