using EbayPlatform.Application.Dtos;
using EbayPlatform.Application.Services;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Models.Enums;
using Newtonsoft.Json;
using Quartz;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz.Jobs
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
            var syncTaskJobConfigList = await _syncTaskJobAppService.GetListByJobStatusAsync(cancellationToken: context.CancellationToken)
                                              .ConfigureAwait(false);

            syncTaskJobConfigList.ForEach(syncTaskJobConfigItem =>
            {
                if (syncTaskJobConfigItem.JobStatus == JobStatusType.Completed)
                    return;

                //店铺任务
                foreach (var shopTaskItem in syncTaskJobConfigItem.ShopTasks)
                {
                    if (shopTaskItem.ShopTaskStatus == JobStatusType.Completed && !string.IsNullOrEmpty(shopTaskItem.ParamValue))
                    {
                        var paramValueToEntityDto = JsonConvert.DeserializeObject<ParamValueToEntityDto>(shopTaskItem.ParamValue);
                        paramValueToEntityDto.PageIndex = 1; //重置当前页
                        shopTaskItem.ChangeShopTaskParamValue(JsonConvert.SerializeObject(paramValueToEntityDto));
                        shopTaskItem.ChangeShopTaskJobStatus(JobStatusType.UnExecute);
                    }
                }
                syncTaskJobConfigItem.ChangeSyncTaskJobConfigJobStatus(JobStatusType.UnExecute);
            });

            if (syncTaskJobConfigList.Any())
            {
                await _syncTaskJobAppService.UpdateShopTaskAsync(syncTaskJobConfigList).ConfigureAwait(false);
            }
        }
    }
}
