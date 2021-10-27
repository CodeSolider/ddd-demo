using EbayPlatform.Application.Dtos;
using EbayPlatform.Domain.Commands.StystemLog;
using EbayPlatform.Domain.Core.Abstractions;
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
        public Task DeleteSystemLogByDateAsync(DateTime createDate, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new DeleteSystemLogCommand(createDate), cancellationToken);
        }

        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <returns></returns>
        public Task<int> AddSystemLogAsync(SystemLogDto systemLogDto, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new CreateSystemLogCommand(systemLogDto.ObjectId, systemLogDto.LogType, systemLogDto.Content), cancellationToken);
        }


#pragma warning disable CA1816 // Dispose 方法应调用 SuppressFinalize
        public void Dispose() => GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose 方法应调用 SuppressFinalize
    }
}
