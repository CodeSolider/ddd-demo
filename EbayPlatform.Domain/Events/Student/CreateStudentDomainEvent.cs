using EbayPlatform.Domain.Core.Abstractions;

namespace EbayPlatform.Domain.Events.Student
{
    public class CreateStudentDomainEvent : IDomainEvent
    {
        public Models.Student Student { get; set; }

        public CreateStudentDomainEvent(Models.Student student)
        {
            this.Student = student;
        }
    }
}
