using EbayPlatform.Domain.Commands.SyncTaskJobConfig;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.SyncTaskJobConfig
{
    public class SyncTaskJobConfigCommandHandler : IRequestHandler<CreateSyncTaskJobConfigCommand, int>,
        IRequestHandler<DeleteSyncTaskJobConfigCommand, bool>
    {
        private readonly ISyncTaskJobConfigRepository _syncTaskJobConfigRepository;
        public SyncTaskJobConfigCommandHandler(ISyncTaskJobConfigRepository syncTaskJobConfigRepository)
        {
            _syncTaskJobConfigRepository = syncTaskJobConfigRepository;
        }


        /// <summary>
        /// 创建新的同步任务配置信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(CreateSyncTaskJobConfigCommand request, CancellationToken cancellationToken)
        {
            bool exists = _syncTaskJobConfigRepository.CheckJobName(request.JobName);
            if (exists)
            {
                throw new Exception("任务名称重复");
            }

            var syncTaskJobConfig = await _syncTaskJobConfigRepository
                                         .AddAsync(new Models.SyncTaskJobConfig(request.JobName, request.JobDesc,
                                          request.JobClassFullName, request.Cron, request.CronDesc,
                                          request.JobStatus), cancellationToken);
            await _syncTaskJobConfigRepository.UnitOfWork.CommitAsync(cancellationToken);
            return syncTaskJobConfig.Id;
        }

        /// <summary>
        /// 删除同步任务
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteSyncTaskJobConfigCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.JobName))
            {
                throw new NullReferenceException($"参数不能为空{nameof(request.JobName)}");
            }

            var syncTaskJobConfigItem = await _syncTaskJobConfigRepository
                                              .GetSyncTaskJobConfigByJobNameAsync(request.JobName)
                                              .ConfigureAwait(false);
            if (syncTaskJobConfigItem == null)
            {
                throw new Exception("该项不存在或已经被删除");
            }

            return await _syncTaskJobConfigRepository.DeleteAsync(syncTaskJobConfigItem.Id).ConfigureAwait(false);
        }

    }
}
