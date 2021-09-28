using EbayPlatform.Application.Dtos;

namespace EbayPlatform.WebApi.RequestModel
{
    /// <summary>
    /// 创建同步任务请求参数
    /// </summary>
    public class CreateSyncTaskJobRequestModel
    {
        /// <summary>
        /// 同步配置任务
        /// </summary>
        public SyncTaskJobConfigDto SyncTaskJobConfig { get; set; }
    }
}
