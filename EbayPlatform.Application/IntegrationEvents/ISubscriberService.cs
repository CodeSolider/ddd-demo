using EbayPlatform.Application.IntegrationEvents.Student;

namespace EbayPlatform.Application.IntegrationEvents
{
    public interface ISubscriberService
    {
        /// <summary>
        /// 创建学生后
        /// </summary>
        /// <param name="event"></param>
        void StudentCreated(StudentCreatedIntegrationEvent @event);
    }
}
