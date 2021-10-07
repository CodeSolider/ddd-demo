using DotNetCore.CAP;
using EbayPlatform.Application.Dtos;
using EbayPlatform.Application.Services;
using EbayPlatform.Domain.IntegrationEvents;
using EbayPlatform.Domain.Models.Enums;
using EbayPlatform.Infrastructure.Core;
using Newtonsoft.Json;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz.Commons
{
    /// <summary>
    /// Task Job基类
    /// </summary>
    [DisallowConcurrentExecution]
    public abstract class BaseTaskJob : IJob
    {
        protected readonly ISyncTaskJobAppService _syncTaskJobAppService;
        protected readonly ICapPublisher _capPublisher;
        public BaseTaskJob(ISyncTaskJobAppService syncTaskJobAppService,
            ICapPublisher capPublisher)
        {
            _syncTaskJobAppService = syncTaskJobAppService;
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 数据任务发布
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            //任务名称
            string jobName = context.JobDetail.Key.Name;
            var syncTaskJobConfigItem = await _syncTaskJobAppService
                                              .GetSyncTaskJobConfigByName(jobName, context.CancellationToken)
                                              .ConfigureAwait(false);

            foreach (var syncTaskJobParamItem in syncTaskJobConfigItem.ShopTasks)
            {
                await _capPublisher.PublishAsync(jobName, new CollectionIntegrationEvent(syncTaskJobParamItem.ShopName, syncTaskJobParamItem.ParamValue),
                                                 cancellationToken: context.CancellationToken);
                syncTaskJobParamItem.ChangeShopTaskJobStatus(JobStatusType.Executing);
            }
            //更新状态
            syncTaskJobConfigItem.ChangeSyncTaskJobConfigJobStatus(JobStatusType.Executing);
            await _syncTaskJobAppService
                  .UpdateShopTaskAsync(new List<Domain.Models.SyncTaskJobConfig> { syncTaskJobConfigItem })
                  .ConfigureAwait(false);
        }

        /// <summary>
        /// 保存结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobName"></param>
        /// <param name="shopName"></param>
        /// <param name="apiResult"></param>
        /// <returns></returns>
        protected virtual async Task SaveResultAsync<T>(string jobName, string shopName, ApiResult apiResult)
        {
            if (apiResult.Code == 200 && apiResult is ApiResult<ParamValueToEntityDto<T>> responseData)
            {
                var syncTaskJobConfigItem = await _syncTaskJobAppService.GetSyncTaskJobConfigByName(jobName).ConfigureAwait(false);
                foreach (var shopTask in syncTaskJobConfigItem.ShopTasks.Where(o => o.ShopName == shopName))
                {
                    shopTask.ChangeShopTaskParamValue(JsonConvert.SerializeObject(new
                    {
                        responseData.Data.PageIndex,
                        responseData.Data.PageSize,
                        responseData.Data.FromDate,
                        responseData.Data.ToDate,
                    }));
                    shopTask.ChangeShopTaskJobStatus(JobStatusType.Completed);
                }
                //更新状态
                syncTaskJobConfigItem.ChangeSyncTaskJobConfigJobStatus(JobStatusType.Completed);
                await _syncTaskJobAppService
                      .UpdateShopTaskAsync(new List<Domain.Models.SyncTaskJobConfig> { syncTaskJobConfigItem })
                      .ConfigureAwait(false);
            }

        }
    }
}
