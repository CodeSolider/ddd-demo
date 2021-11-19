using EbayPlatform.Domain.Core.Abstractions;

namespace EbayPlatform.Domain.Events.SyncTaskJobConfig
{
    public class CreateSyncTaskJobConfigDomainEvent : IDomainEvent
    {
        public AggregateModel.SyncTaskJobConfig SyncTaskJobConfig { get; private set; }

        public CreateSyncTaskJobConfigDomainEvent(AggregateModel.SyncTaskJobConfig syncTaskJobConfig)
               => this.SyncTaskJobConfig = syncTaskJobConfig;
    }
}
