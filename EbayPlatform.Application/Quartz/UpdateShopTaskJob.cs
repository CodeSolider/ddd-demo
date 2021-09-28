using EbayPlatform.Application.Dtos;
using EbayPlatform.Application.Services;
using EbayPlatform.Domain.Models.Enums;
using EbayPlatform.Infrastructure.Core;
using Newtonsoft.Json;
using Quartz;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz
{
    /// <summary>
    /// 更新店铺任务信息
    /// </summary>
    [DisallowConcurrentExecution]
    public class UpdateShopTaskJob : IJob, IDependency
    {
        private readonly ISyncTaskJobAppService _syncTaskJobAppService;
        public UpdateShopTaskJob(ISyncTaskJobAppService syncTaskJobAppService)
        {
            _syncTaskJobAppService = syncTaskJobAppService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var syncTaskJobConfigList = await _syncTaskJobAppService
                                             .GetSyncTaskJobConfigListByAsync(context.CancellationToken)
                                             .ConfigureAwait(false);

            syncTaskJobConfigList.ForEach(syncTaskJobConfigItem =>
            {
                //店铺任务
                foreach (var shopTaskItem in syncTaskJobConfigItem.ShopTasks)
                {
                    if (shopTaskItem.ShopTaskStatus == JobStatusType.UnExecute)
                    {
                        return;
                    }

                    if (shopTaskItem.ShopTaskStatus == JobStatusType.Completed && !string.IsNullOrEmpty(shopTaskItem.ParamValue))
                    {
                        var paramValueToEntityDto = JsonConvert.DeserializeObject<ParamValueToEntityDto>(shopTaskItem.ParamValue);
                        paramValueToEntityDto.PageIndex = 0; //重置页码
                        shopTaskItem.ChangeShopTaskParamValue(JsonConvert.SerializeObject(paramValueToEntityDto));
                    }
                    shopTaskItem.ChangeShopTaskJobStatus(JobStatusType.UnExecute);
                }
                syncTaskJobConfigItem.ChangeSyncTaskJobConfigJobStatus(JobStatusType.UnExecute);
            });

            await _syncTaskJobAppService
                 .UpdateShopTaskAsync(syncTaskJobConfigList)
                 .ConfigureAwait(false);

        }
    }
}
