using MediatR; 

namespace EbayPlatform.Domain.Abstractions
{
    /// <summary>
    /// 领域事件执行接口
    /// </summary>
    public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
    }
}
