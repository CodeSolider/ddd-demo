using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Domain.Models;
using EbayPlatform.Infrastructure.Context;
using EbayPlatform.Infrastructure.Core.Dependency;
using System.Collections.Generic;
using System.Linq;

namespace EbayPlatform.Infrastructure.Repository
{
    public class StudentRepository : Repository<Student, long, EbayPlatformDbContext>, IStudentRepository, IDependency
    {
        public StudentRepository(EbayPlatformDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns></returns>
        public List<Student> GetStudentList()
        {
            return NoTrackingQueryable.ToList();
        }

        /// <summary>
        /// 根据Id获取学生信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Student GetStudentByName(string name)
        {
            return NoTrackingQueryable.FirstOrDefault(o => o.Name == name);
        }

        /// <summary>
        /// 添加学生
        /// </summary>
        /// <param name="student"></param>
        public Student AddStudent(Student student)
        {
            return this.Add(student);
        }
    }
}
