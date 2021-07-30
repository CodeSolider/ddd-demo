using EbayPlatform.Application.Commands.Student;
using EbayPlatform.Application.Dto;
using EbayPlatform.Application.Queries.Student;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbayPlatform.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<StudentDto>> GetStudetsList()
        {
            return await _mediator.Send(new GetAllStudentQuery(), HttpContext.RequestAborted);
        }

        [HttpGet]
        public async Task<object> DeleteStudent(int id)
        {
            // await _mediator.Send(new RemoveStudentCommand(id));
            return null;
        }

        /// <summary>
        /// 创建学生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddStudent([FromBody] CreateStudentCommand request)
        {
            return await _mediator.Send(request, HttpContext.RequestAborted);
        }
    }
}
