using MediatR;

namespace EbayPlatform.Domain.Commands.SyncTaskJobConfig
{
    public class GetSyncTaskJobConfigCommand : IRequest<Models.SyncTaskJobConfig>
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; private set; }

        public GetSyncTaskJobConfigCommand() { }
        public GetSyncTaskJobConfigCommand(string jobName)
        {
            this.JobName = jobName;
        }
    }
}
