using EbayPlatform.Domain.IntegrationEvents.SyncTaskJobConfig;

namespace EbayPlatform.Application.IntegrationEvents
{
    public interface ISubscriberService
    {
        /// <summary>
        /// 创建同步任务配置信息
        /// </summary>
        /// <param name="event"></param>
        void SyncTaskJobConfigCreated(SyncTaskJobConfigCreatedIntegrationEvent @event);
    }
}
