using DotNetCore.CAP;
using EbayPlatform.Application.IntegrationEvents.Student;
using EbayPlatform.Domain.Abstractions;
using EbayPlatform.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.DomainEventHandlers.Student
{
    /// <summary>
    /// 被创建后事件处理程序
    /// </summary>
    public class StudentCreatedEventHandler : IDomainEventHandler<CreateStudentDomainEvent>
    {
        private readonly ICapPublisher _capPublisher;
        public StudentCreatedEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(CreateStudentDomainEvent notification, CancellationToken cancellationToken)
        {
            await _capPublisher.PublishAsync("StudentCreated", new StudentCreatedIntegrationEvent(notification.Student.Id), cancellationToken: cancellationToken);
        }
    }
}
