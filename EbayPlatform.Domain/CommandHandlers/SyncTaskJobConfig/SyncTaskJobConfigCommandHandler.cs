using EbayPlatform.Domain.Commands.SyncTaskJobConfig;
using EbayPlatform.Domain.Interfaces;
using MediatR;
using Newtonsoft.Json;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.CommandHandlers.SyncTaskJobConfig
{
    public class SyncTaskJobConfigCommandHandler : IRequestHandler<CreateSyncTaskJobConfigCommand, int>,
        IRequestHandler<UpdateSyncTaskJobConfigCommand, bool>
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
            using (LogContext.PushProperty("CreateSyncTaskJobConfigCommand", $"{JsonConvert.SerializeObject(request)}"))
            {
                bool exists = _syncTaskJobConfigRepository.CheckJobName(request.JobName);
                if (exists)
                {
                    throw new Exception("任务名称重复");
                }

                var syncTaskJobConfig = await _syncTaskJobConfigRepository
                                              .AddAsync(new AggregateModel.SyncTaskJobConfig(request.JobName, request.JobDesc,
                                                        request.JobAssemblyName, request.Cron, request.CronDesc), cancellationToken)
                                              .ConfigureAwait(false);


                request.ShopTasks.ForEach(SyncTaskJobParam =>
                {
                    syncTaskJobConfig.AddShopTask(SyncTaskJobParam.ShopName, SyncTaskJobParam.ParamValue);
                });

                await _syncTaskJobConfigRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
                return syncTaskJobConfig.Id;
            }

        }


        public async Task<bool> Handle(UpdateSyncTaskJobConfigCommand request, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("UpdateSyncTaskJobConfigCommand", $"{JsonConvert.SerializeObject(request)}"))
            {
                _syncTaskJobConfigRepository.UpdateRange(request.SyncTaskJobConfigs);
                return await _syncTaskJobConfigRepository.UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            }

        }
    }
}
