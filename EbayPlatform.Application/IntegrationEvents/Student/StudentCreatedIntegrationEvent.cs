
namespace EbayPlatform.Application.IntegrationEvents.Student
{
    public class StudentCreatedIntegrationEvent
    {
        public StudentCreatedIntegrationEvent(long studentId) => this.StudentId = studentId;

        public long StudentId { get; }
    }
}
