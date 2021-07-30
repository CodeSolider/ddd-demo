using EbayPlatform.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Commands.Student
{
    /// <summary>
    /// 创建学生处理程序
    /// </summary>
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, long>
    {
        private readonly IStudentRepository _studentRepository;
        public CreateStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<long> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = new Domain.Models.Student(request.Name, request.Gender, request.Age, request.Height);
            _studentRepository.Add(student);
            await _studentRepository.UnitOfWork.CommitAsync(cancellationToken);
            return student.Id;
        }
    }
}
