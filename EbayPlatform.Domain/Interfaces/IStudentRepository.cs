using EbayPlatform.Domain.Models;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Interfaces
{
    public interface IStudentRepository : IRepository<Student, long>
    {
        List<Student> GetStudentList();

        Student GetStudentByName(string name);

        Student AddStudent(Student student);
    }
}
