using DotNetCore.CAP;
using EbayPlatform.Domain.IntegrationEvents.SyncTaskJobConfig;
using System;

namespace EbayPlatform.Application.IntegrationEvents
{
    /// <summary>
    /// 订阅服务
    /// </summary>
    public class SubscriberService : ICapSubscribe, ISubscriberService
    {
        /// <summary>
        /// 创建同步任务配置信息
        /// </summary>
        /// <param name="event"></param>
        [CapSubscribe(nameof(SyncTaskJobConfigCreated))]
        public void SyncTaskJobConfigCreated(SyncTaskJobConfigCreatedIntegrationEvent @event)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }
    }
}
