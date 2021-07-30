using EbayPlatform.Domain.Abstractions;
using EbayPlatform.Domain.Models;

namespace EbayPlatform.Domain.Events
{
    public class CreateStudentDomainEvent : IDomainEvent
    {
        public Student Student { get; set; }

        public CreateStudentDomainEvent(Student student)
        {
            this.Student = student;
        }
    }
}
