using EbayPlatform.Application.Dto;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Infrastructure.Core.Dependency;
using System.Collections.Generic;
using Mapster;
using MediatR;
using EbayPlatform.Domain.Commands.Student;

namespace EbayPlatform.Application.Services
{
    /// <summary>
    /// 学生接口服务实现
    /// </summary>
    public class StudentService : IStudentService, IDependency
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMediator _mediator;
        public StudentService(IStudentRepository studentRepository,
            IMediator mediator)
        {
            this._studentRepository = studentRepository;
            this._mediator = mediator;
        }

        /// <summary>
        /// 获取所有学生信息
        /// </summary>
        /// <returns></returns>
        public List<StudentDto> GetStudentList()
        {
            return _studentRepository.GetStudentList().Adapt<List<StudentDto>>();
        }
    }
}
