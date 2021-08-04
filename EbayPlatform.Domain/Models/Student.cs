using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Events.Student;

namespace EbayPlatform.Domain.Models
{
    /// <summary>
    /// 学生测试类
    /// </summary>
    public class Student : Entity<long>, IAggregateRoot
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 性别
        /// </summary>
        public short Gender { get; private set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; private set; }

        /// <summary>
        /// 身高
        /// </summary>
        public decimal Height { get; private set; }

        protected Student() { }

        public Student(string name, short gender, int age, decimal height)
        {
            this.Name = name;
            this.Age = age;
            this.Gender = gender;
            this.Height = height;

            //添加事件
            this.AddDomainEvent(new CreateStudentDomainEvent(this));
        }



    }
}
