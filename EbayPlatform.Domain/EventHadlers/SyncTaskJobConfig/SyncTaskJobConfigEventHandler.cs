using DotNetCore.CAP;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Events.SyncTaskJobConfig;
using EbayPlatform.Domain.IntegrationEvents;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.EventHadlers.SyncTaskJobConfig
{
    public class SyncTaskJobConfigEventHandler : IDomainEventHandler<CreateSyncTaskJobConfigDomainEvent>,
        IDomainEventHandler<CollectionSyncTaskJobDomainEvent>
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
        public Task Handle(CreateSyncTaskJobConfigDomainEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// MQ队列发布
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task Handle(CollectionSyncTaskJobDomainEvent notification, CancellationToken cancellationToken)
        {
            return _capPublisher.PublishAsync(notification.ShopName, new CollectionSyncTaskJobIntegrationEvent(notification.ShopName, notification.ParamValue), cancellationToken: cancellationToken);
        }
    }
}
