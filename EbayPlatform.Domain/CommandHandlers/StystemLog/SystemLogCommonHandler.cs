using EbayPlatform.Domain.Commands.StystemLog;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.StystemLog
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class SystemLogCommonHandler : IRequestHandler<DeleteSystemLogCommand, bool>
    {
        private readonly ISystemLogRepository _systemLogRepository;
        private readonly ILogger<SystemLogCommonHandler> _logger;
        public SystemLogCommonHandler(ISystemLogRepository systemLogRepository,
            ILogger<SystemLogCommonHandler> logger)
        {
            _systemLogRepository = systemLogRepository;
            _logger = logger;
        }


        public async Task<bool> Handle(DeleteSystemLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var systemLogPagedList = await _systemLogRepository
                                                .GetExpireSystemLogListAsync(request.CreateDate, request.LogType, cancellationToken)
                                                .ConfigureAwait(false);
                _systemLogRepository.RemoveRange(systemLogPagedList.Items);
                _logger.LogWarning($"日志:{JsonConvert.SerializeObject(systemLogPagedList.Items)}已被删除");
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning("删除过期日志发生异常");
                _logger.LogError($"异常信息:{ex.Message}");
                return false;
            }
        }


    }
}
