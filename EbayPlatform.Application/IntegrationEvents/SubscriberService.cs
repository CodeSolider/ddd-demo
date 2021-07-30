using DotNetCore.CAP;
using EbayPlatform.Application.IntegrationEvents.Student;
using System;

namespace EbayPlatform.Application.IntegrationEvents
{
    /// <summary>
    /// 订阅服务
    /// </summary>
    public class SubscriberService : ICapSubscribe, ISubscriberService
    {

        [CapSubscribe("StudentCreated")]
        public void StudentCreated(StudentCreatedIntegrationEvent @event)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }

    }
}
