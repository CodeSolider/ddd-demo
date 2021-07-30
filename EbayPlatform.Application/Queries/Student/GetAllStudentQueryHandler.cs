using EbayPlatform.Application.Dto;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mapster;

namespace EbayPlatform.Application.Queries.Student
{
    public class GetAllStudentQueryHandler : IRequestHandler<GetAllStudentQuery, List<StudentDto>>
    {
        private readonly IStudentRepository _studentRepository;
        public GetAllStudentQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<StudentDto>> Handle(GetAllStudentQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_studentRepository.GetStudentList().Adapt<List<StudentDto>>());
        }
    }
}
