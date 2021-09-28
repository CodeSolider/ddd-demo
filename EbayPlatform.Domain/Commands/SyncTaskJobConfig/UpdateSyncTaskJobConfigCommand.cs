using MediatR;
using System.Collections.Generic;

namespace EbayPlatform.Domain.Commands.SyncTaskJobConfig
{
    /// <summary>
    /// 更新同步任务信息
    /// </summary>
    public class UpdateSyncTaskJobConfigCommand : IRequest<bool>
    {
        /// <summary>
        /// 同步任务配置信息
        /// </summary>
        public List<Models.SyncTaskJobConfig> SyncTaskJobConfigs { get; }


        public UpdateSyncTaskJobConfigCommand(List<Models.SyncTaskJobConfig> syncTaskJobConfigs)
        {
            this.SyncTaskJobConfigs = syncTaskJobConfigs;
        }
    }
}
