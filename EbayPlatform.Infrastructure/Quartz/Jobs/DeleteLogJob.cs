using EbayPlatform.Domain.Commands.StystemLog;
using MediatR;
using Quartz;
using System;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Quartz.Jobs
{
    /// <summary>
    /// 删除过期日志
    /// </summary>
    [DisallowConcurrentExecution]
    public class DeleteLogJob : IJob
    {
        private readonly IMediator _mediator;
        public DeleteLogJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return _mediator.Send(new DeleteSystemLogCommand(createDate: DateTime.Now));
        }
    }
}
