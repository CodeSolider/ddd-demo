using EbayPlatform.Domain.Core.Abstractions;

namespace EbayPlatform.Domain.Events.SyncTaskJobConfig
{
    public class CreateSyncTaskJobConfigDomainEvent : IDomainEvent
    {
        public Models.SyncTaskJobConfig SyncTaskJobConfig { get; private set; }

        public CreateSyncTaskJobConfigDomainEvent(Models.SyncTaskJobConfig syncTaskJobConfig)
               => this.SyncTaskJobConfig = syncTaskJobConfig;
    }
}
