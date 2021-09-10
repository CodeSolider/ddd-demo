using EbayPlatform.Domain.Commands.SyncTaskJobConfig;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.SyncTaskJobConfig
{
    public class SyncTaskJobConfigCommandHandler : IRequestHandler<CreateSyncTaskJobConfigCommand, Models.SyncTaskJobConfig>
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
        public async Task<Models.SyncTaskJobConfig> Handle(CreateSyncTaskJobConfigCommand request, CancellationToken cancellationToken)
        {
            bool exists = _syncTaskJobConfigRepository.CheckJobName(request.JobName);
            if (exists)
            {
                throw new Exception("任务名称重复");
            }

            var syncTaskJobConfig = new Models.SyncTaskJobConfig(request.JobName, request.JobDesc, request.JobAssemblyName,
                                                                 request.Cron, request.CronDesc);

            request.SyncTaskJobParams.ForEach(SyncTaskJobParam =>
            {
                syncTaskJobConfig.ChangeSyncTaskJobParamValue(SyncTaskJobParam.ShopName, SyncTaskJobParam.ParamValue);
            });

            return await _syncTaskJobConfigRepository
                         .AddAsync(syncTaskJobConfig, cancellationToken)
                         .ConfigureAwait(false);
        }


        public void Dispose()
        {
            _syncTaskJobConfigRepository.Dispose();
        }
    }
}
