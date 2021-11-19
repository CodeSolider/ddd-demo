using EbayPlatform.Domain.Commands.StystemLog;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.StystemLog
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class SystemLogCommonHandler : IRequestHandler<DeleteSystemLogCommand, bool>,
        IRequestHandler<CreateSystemLogCommand, int>
    {
        private readonly ISystemLogRepository _systemLogRepository;
        private readonly ILogger<SystemLogCommonHandler> _logger;
        public SystemLogCommonHandler(ISystemLogRepository systemLogRepository,
            ILogger<SystemLogCommonHandler> logger)
        {
            _systemLogRepository = systemLogRepository;
            _logger = logger;
        }

        /// <summary>
        /// 根据创建删除过期的日志
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteSystemLogCommand request, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("DeleteSystemLogCommand", $"{request.CreateDate}"))
            {
                try
                {
                    var systemLogPagedList = await _systemLogRepository
                                                    .GetExpireSystemLogListAsync(request.CreateDate, cancellationToken)
                                                    .ConfigureAwait(false);

                    if (!systemLogPagedList.Items.Any())
                    {
                        _logger.LogWarning($"暂无日志记录信息");
                        return false;
                    }
                    _systemLogRepository.RemoveRange(systemLogPagedList.Items);
                    _logger.LogDebug($"{JsonConvert.SerializeObject(systemLogPagedList.Items)}日志记录已被删除");
                    return await Task.FromResult(true).ConfigureAwait(false);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("删除过期日志发生异常");
                    _logger.LogError($"异常信息:{ex.Message}");
                    return false;
                }
            } 
        }


        /// <summary>
        /// 添加系统日志信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(CreateSystemLogCommand request, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("CreateSystemLogCommand", $"{JsonConvert.SerializeObject(request)}"))
            {
                var systemLog = await _systemLogRepository
                                  .AddAsync(new AggregateModel.SystemLog(request.ObjectId, request.LogType, request.Content), cancellationToken)
                                  .ConfigureAwait(false);

                await _systemLogRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return systemLog.Id;
            }
        }
    }
}
