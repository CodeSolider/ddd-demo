using EbayPlatform.Domain.Commands.SyncTaskJobConfig;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.SyncTaskJobConfig
{
    /// <summary>
    /// 创建新的同步任务配置信息
    /// </summary>
    public class CreateSyncTaskJobConfigCommandHandler : IRequestHandler<CreateSyncTaskJobConfigCommand, int>
    {
        private readonly ISyncTaskJobConfigRepository _syncTaskJobConfigRepository;
        public CreateSyncTaskJobConfigCommandHandler(ISyncTaskJobConfigRepository syncTaskJobConfigRepository)
        {
            _syncTaskJobConfigRepository = syncTaskJobConfigRepository;
        }

        public async Task<int> Handle(CreateSyncTaskJobConfigCommand request, CancellationToken cancellationToken)
        {
            bool exists = _syncTaskJobConfigRepository.CheckJobName(request.JobName);
            if (exists)
            {
                throw new Exception("任务名称重复");
            }

            var syncTaskJobConfig = await _syncTaskJobConfigRepository
                                         .AddAsync(new Models.SyncTaskJobConfig(request.JobName, request.JobDesc, 
                                          request.JobClassFullName,request.Cron, request.CronDesc,
                                          request.JobStatus), cancellationToken);
            await _syncTaskJobConfigRepository.UnitOfWork.CommitAsync(cancellationToken);
            return syncTaskJobConfig.Id;
        }
    }
}
