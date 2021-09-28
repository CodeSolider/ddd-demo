using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Events.SyncTaskJobConfig;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.EventHadlers.SyncTaskJobConfig
{
    public class SyncTaskJobConfigEventHandler : IDomainEventHandler<CreateSyncTaskJobConfigDomainEvent>
    {
        public SyncTaskJobConfigEventHandler()
        {

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

    }
}
