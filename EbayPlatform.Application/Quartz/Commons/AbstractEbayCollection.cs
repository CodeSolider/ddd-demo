using eBay.Service.Core.Sdk;
using EbayPlatform.Application.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz.Commons
{
    /// <summary>
    /// Ebay 采集任务父类
    /// </summary>
    public abstract class AbstractEbayCollection
    {
        protected readonly ISyncTaskJobAppService _syncTaskJobAppService;
        public AbstractEbayCollection(ISyncTaskJobAppService syncTaskJobAppService)
        {
            _syncTaskJobAppService = syncTaskJobAppService;
        }

        /// <summary>
        /// 运行Job下载
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RunJobAsync(string jobName, CancellationToken cancellationToken = default)
        {
            var syncTaskJobConfigItem = await _syncTaskJobAppService
                                             .GetSyncTaskJobConfigByNameAsync(jobName, cancellationToken)
                                             .ConfigureAwait(false);


            foreach (var syncTaskJobParamItem in syncTaskJobConfigItem.SyncTaskJobParams)
            {
                 ////to do MQ
                //ApiCall apiCall = GetRequestApiCall(syncTaskJobParamItem.ParamValue);
               
                //syncTaskJobConfigItem.ChangeSyncTaskJobParamValue(syncTaskJobParamItem.ShopName,
                //                                                  await SaveResponseResultAsync(apiCall, await GetResponseListAsync(apiCall)
                                                                                                               //.ConfigureAwait(false)));



            }
        }


        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        protected abstract ApiCall GetRequestApiCall(string paramValue);

        /// <summary>
        /// 获取下载的数据
        /// </summary>
        /// <param name="apiCall"></param>
        /// <param name="syncTaskJobParam"></param>
        /// <returns></returns>
        protected abstract Task<List<eBay.Service.Core.Soap.AbstractResponseType>> GetResponseListAsync(ApiCall apiCall);

        /// <summary>
        /// 保存下载的结果数据
        /// </summary>
        /// <param name="apiCall"></param>
        /// <param name="abstractResponseList"></param>
        /// <returns></returns>
        protected abstract Task<string> SaveResponseResultAsync(ApiCall apiCall, List<eBay.Service.Core.Soap.AbstractResponseType> abstractResponseList);
    }
}
