using MediatR;

namespace EbayPlatform.Domain.Commands.Student
{
    /// <summary>
    /// 创建学生
    /// </summary>
    public class CreateStudentCommand : IRequest<long>
    {
        public string Name { get; private set; }

        /// <summary>
        /// 性别
        /// </summary>
        public short Gender { get; private set; }

        public int Age { get; private set; }

        public decimal Height { get; private set; }


        public CreateStudentCommand() { }

        public CreateStudentCommand(string name, short gender, int age, decimal height)
        {
            this.Name = name;
            this.Age = age;
            this.Gender = gender;
            this.Height = height;
        }
    }
}
