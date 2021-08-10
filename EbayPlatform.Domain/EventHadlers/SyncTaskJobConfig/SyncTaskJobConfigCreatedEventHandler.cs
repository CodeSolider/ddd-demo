using DotNetCore.CAP;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Events.SyncTaskJobConfig;
using EbayPlatform.Domain.IntegrationEvents.SyncTaskJobConfig;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.EventHadlers.SyncTaskJobConfig
{
    /// <summary>
    /// 同步任务配置创建事件
    /// </summary>
    public class SyncTaskJobConfigCreatedEventHandler : IDomainEventHandler<CreateSyncTaskJobConfigDomainEvent>
    {
        private readonly ICapPublisher _capPublisher;
        public SyncTaskJobConfigCreatedEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(CreateSyncTaskJobConfigDomainEvent notification, CancellationToken cancellationToken)
        {
            await _capPublisher.PublishAsync("SyncTaskJobConfigCreated",
                new SyncTaskJobConfigCreatedIntegrationEvent(notification.SyncTaskJobConfig.Id), cancellationToken: cancellationToken);
        }
    }
}
