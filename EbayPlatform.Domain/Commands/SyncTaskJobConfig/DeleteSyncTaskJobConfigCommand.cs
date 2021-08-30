using MediatR;

namespace EbayPlatform.Domain.Commands.SyncTaskJobConfig
{
    /// <summary>
    /// 删除同步配置任务
    /// </summary>
    public class DeleteSyncTaskJobConfigCommand : IRequest<bool>
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; private set; }

        public DeleteSyncTaskJobConfigCommand() { }
        public DeleteSyncTaskJobConfigCommand(string jobName)
        {
            this.JobName = jobName;
        }
    }
}
