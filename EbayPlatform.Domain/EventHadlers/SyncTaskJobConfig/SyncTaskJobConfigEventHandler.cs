using DotNetCore.CAP;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Events.SyncTaskJobConfig;
using EbayPlatform.Domain.IntegrationEvents.SyncTaskJobConfig;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.EventHadlers.SyncTaskJobConfig
{
    public class SyncTaskJobConfigEventHandler : IDomainEventHandler<CreateSyncTaskJobConfigDomainEvent>
    {
        private readonly ICapPublisher _capPublisher;
        public SyncTaskJobConfigEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 同步任务配置创建事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(CreateSyncTaskJobConfigDomainEvent notification, CancellationToken cancellationToken)
        {
            await _capPublisher
                  .PublishAsync("SyncTaskJobConfigCreated",
                                 new SyncTaskJobConfigCreatedIntegrationEvent(notification.SyncTaskJobConfig.Id),
                                 cancellationToken: cancellationToken);
        }


    }
}
