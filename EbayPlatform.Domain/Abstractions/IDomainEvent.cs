using MediatR;

namespace EbayPlatform.Domain.Abstractions
{
    /// <summary>
    /// 领域事件通知接口
    /// </summary>
    public interface IDomainEvent : INotification
    {
    }
}
