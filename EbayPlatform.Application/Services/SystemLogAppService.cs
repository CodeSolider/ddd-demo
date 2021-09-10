using EbayPlatform.Domain.Commands.StystemLog;
using EbayPlatform.Infrastructure.Core;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Services
{
    public class SystemLogAppService : ISystemLogAppService, IDependency
    {
        private readonly IMediator _mediator;
        public SystemLogAppService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 根据创建删除过期的日志
        /// </summary>
        /// <param name="createDate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DeleteSystemLogByDate(DateTime createDate, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new DeleteSystemLogCommand(createDate), cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
