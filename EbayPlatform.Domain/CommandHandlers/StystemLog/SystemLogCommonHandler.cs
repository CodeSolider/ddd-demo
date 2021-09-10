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

        /// <summary>
        /// 根据创建删除过期的日志
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteSystemLogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var systemLogPagedList = await _systemLogRepository
                                                .GetExpireSystemLogListAsync(request.CreateDate, cancellationToken)
                                                .ConfigureAwait(false);
                _systemLogRepository.RemoveRange(systemLogPagedList.Items);
                _logger.LogWarning($"日志:{JsonConvert.SerializeObject(systemLogPagedList.Items)}已被删除");
                return await _systemLogRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning("删除过期日志发生异常");
                _logger.LogError($"异常信息:{ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _systemLogRepository.Dispose();
        }
    }
}
