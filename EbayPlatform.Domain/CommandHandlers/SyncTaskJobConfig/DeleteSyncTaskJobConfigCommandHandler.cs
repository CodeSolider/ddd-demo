using EbayPlatform.Domain.Commands.SyncTaskJobConfig;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace EbayPlatform.Domain.CommandHandlers.SyncTaskJobConfig
{
    /// <summary>
    /// 删除同步任务
    /// </summary>
    public class DeleteSyncTaskJobConfigCommandHandler : IRequestHandler<DeleteSyncTaskJobConfigCommand, bool>
    {
        private readonly ISyncTaskJobConfigRepository _syncTaskJobConfigRepository;
        public DeleteSyncTaskJobConfigCommandHandler(ISyncTaskJobConfigRepository syncTaskJobConfigRepository)
        {
            _syncTaskJobConfigRepository = syncTaskJobConfigRepository;
        }

        public async Task<bool> Handle(DeleteSyncTaskJobConfigCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.JobName))
            {
                throw new NullReferenceException($"参数不能为空{nameof(request.JobName)}");
            }

            var syncTaskJobConfigItem = _syncTaskJobConfigRepository.GetSyncTaskJobConfigByJobName(request.JobName);
            if (syncTaskJobConfigItem == null)
            {
                throw new Exception("该项不存在或已经被删除");
            }

            return await _syncTaskJobConfigRepository.DeleteAsync(syncTaskJobConfigItem.Id).ConfigureAwait(false);
        }
    }
}
