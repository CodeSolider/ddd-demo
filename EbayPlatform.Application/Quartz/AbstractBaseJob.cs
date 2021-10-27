using DotNetCore.CAP;
using eBay.Service.Core.Sdk;
using EbayPlatform.Application.Dtos;
using EbayPlatform.Application.Services;
using EbayPlatform.Domain.IntegrationEvents;
using EbayPlatform.Infrastructure.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using eBay.Service.Core.Soap;
using Microsoft.Extensions.Configuration;
using EbayPlatform.Infrastructure.Core.Engines;

namespace EbayPlatform.Application.Quartz
{
    /// <summary>
    /// 父类Job
    /// </summary>
    public abstract class AbstractBaseJob
    {
        private readonly ISyncTaskJobAppService _syncTaskJobAppService;
        private readonly ICapPublisher _capPublisher;
        private readonly EbayOptionsDto _ebayOptions;
        protected AbstractBaseJob(ISyncTaskJobAppService syncTaskJobAppService)
        {
            _syncTaskJobAppService = syncTaskJobAppService;
            _capPublisher = EngineContext.Current.Resolve<ICapPublisher>();
            _ebayOptions = EngineContext.Current.Resolve<IConfiguration>()?.GetSection("EbayOptions").Get<EbayOptionsDto>();
        }

        /// <summary>
        /// 发布队列信息
        /// </summary>
        /// <param name="jobName"></param>
        protected virtual async Task PublishQueueAsync(string jobName, CancellationToken cancellationToken = default)
        {
            var syncTaskJobConfigItem = await _syncTaskJobAppService
                                              .GetSyncTaskJobConfigByNameAsync(jobName, cancellationToken)
                                              .ConfigureAwait(false);

            if (syncTaskJobConfigItem == null)
            {
                return;
            }

            foreach (var shopTaskItem in syncTaskJobConfigItem.ShopTasks.Where(o => o.ShopTaskStatus != Domain.Models.Enums.JobStatusType.Completed))
            {
                await _capPublisher.PublishAsync(syncTaskJobConfigItem.JobName, new CollectionIntegrationEvent(shopTaskItem.ShopName, shopTaskItem.ParamValue), cancellationToken: cancellationToken)
                                   .ConfigureAwait(false);

                shopTaskItem.ChangeShopTaskJobStatus(Domain.Models.Enums.JobStatusType.Executing);
            }
            //更新状态
            syncTaskJobConfigItem.ChangeSyncTaskJobConfigJobStatus(Domain.Models.Enums.JobStatusType.Executing);
            await _syncTaskJobAppService.UpdateShopTaskAsync(new List<Domain.Models.SyncTaskJobConfig> { syncTaskJobConfigItem }, cancellationToken)
                                        .ConfigureAwait(false);
        }


        /// <summary>
        /// 处理队列信息
        /// </summary>
        /// <returns></returns> 
        protected virtual Task ProcessQueueDataAsync(CollectionIntegrationEvent integrationEvent)
        {
            DownloaderProvider downloaderProvider = new(integrationEvent.ShopName, integrationEvent.ParamValue);
            downloaderProvider.DownloadingPre += AbstractBaseJob_DownloadingPre;
            downloaderProvider.Downloading += DownloaderProvider_Downloading;
            downloaderProvider.DownloadingEnd += DownloaderProvider_DownloadingEnd;
            //启用 下载
            return downloaderProvider.StartAsync();
        }

        /// <summary>
        /// 获取Ebay 下载上下文链接
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ApiContext GetApiContext(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new NullReferenceException($"{nameof(token)}不能为空");
            }

            return new ApiContext
            {
                EPSServerUrl = _ebayOptions.EPSServerUrl,
                SoapApiServerUrl = _ebayOptions.SoapApiServerUrl,
                ApiCredential = new ApiCredential
                {
                    eBayToken = token
                },
                Site = SiteCodeType.US,
                ErrorLanguage = ErrorLanguageCodeType.zh_CN,
                CallRetry = new CallRetry
                {
                    DelayTime = 1,
                    MaximumRetries = 3, //最多重试3次
                    TriggerErrorCodes = new List<string>
                    {
                        "10007",//应用程序内部错误...一般错误
                        "2",// 不支持的动词错误
                        "251"//eBay结构异常...一般错误
                     },
                    //设置要重试的异常类型
                    TriggerExceptions = new List<Type>
                    {
                        typeof(System.Net.ProtocolViolationException),
                        typeof(ApiException)
                    },
                    // 设置要重试的HTTP错误代码
                    TriggerHttpStatusCodes = new List<int>
                    {
                        404,
                        502,
                        500
                    }
                }
            };
        }


        #region  Download Event

        /// <summary>
        /// 下载前
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected abstract ApiCall AbstractBaseJob_DownloadingPre(string arg);

        /// <summary>
        /// 下载中
        /// </summary>
        /// <param name="apiCall"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        protected abstract Task<ApiResult> DownloaderProvider_Downloading(ApiCall apiCall, string shopName);

        /// <summary>
        /// 下载完成
        /// </summary>
        /// <param name="apiResult"></param>
        /// <param name="shopName"></param>
        /// <returns></returns>
        protected abstract Task DownloaderProvider_DownloadingEnd(ApiResult apiResult, string shopName);

        #endregion

        #region 更新下载状态

        /// <summary>
        /// 更新下载状态
        /// </summary>
        protected virtual async Task ModifySyncTaskJobConfigStatusAsync<T>(ApiResult apiResult, string jobName, string shopName)
        {
            if (apiResult.Code == 200 && apiResult is ApiResult<ParamValueToEntityDto<T>> responseData)
            {
                var syncTaskJobConfigItem = await _syncTaskJobAppService.GetSyncTaskJobConfigByNameAsync(jobName).ConfigureAwait(false);
                if (syncTaskJobConfigItem == null)
                {
                    return;
                }

                foreach (var shopTask in syncTaskJobConfigItem.ShopTasks.Where(o => o.ShopName == shopName))
                {
                    shopTask.ChangeShopTaskParamValue(JsonConvert.SerializeObject(new
                    {
                        responseData.Data.PageIndex,
                        responseData.Data.PageSize,
                        responseData.Data.FromDate,
                        responseData.Data.ToDate,
                    }));
                    shopTask.ChangeShopTaskJobStatus(Domain.Models.Enums.JobStatusType.Completed);
                }
                //更新状态
                syncTaskJobConfigItem.ChangeSyncTaskJobConfigJobStatus(Domain.Models.Enums.JobStatusType.Completed);
                await _syncTaskJobAppService
                      .UpdateShopTaskAsync(new List<Domain.Models.SyncTaskJobConfig> { syncTaskJobConfigItem })
                      .ConfigureAwait(false);
            }
        }
        #endregion
    }
}
