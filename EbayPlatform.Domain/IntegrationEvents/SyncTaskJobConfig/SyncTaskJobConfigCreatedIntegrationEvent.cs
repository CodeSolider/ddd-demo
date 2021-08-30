namespace EbayPlatform.Domain.IntegrationEvents.SyncTaskJobConfig
{
    public class SyncTaskJobConfigCreatedIntegrationEvent
    {
        public SyncTaskJobConfigCreatedIntegrationEvent(int syncTaskJobConfigId) => this.SyncTaskJobConfigId = syncTaskJobConfigId;

        public int SyncTaskJobConfigId { get; }
    }
}
