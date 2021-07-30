using EbayPlatform.Application.Dto;
using MediatR;
using System.Collections.Generic;

namespace EbayPlatform.Application.Queries.Student
{
    /// <summary>
    /// 获取所有学生
    /// </summary>
    public class GetAllStudentQuery : IRequest<List<StudentDto>>
    {
    }
}
