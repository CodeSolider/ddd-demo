using MediatR;
using System;
using static EbayPlatform.Domain.Models.Enums.SyncTask;

namespace EbayPlatform.Domain.Commands.SyncTaskJobConfig
{
    public class CreateSyncTaskJobConfigCommand : IRequest<int>
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; private set; }

        /// <summary>
        /// 作业注解
        /// </summary>
        public string JobDesc { get; private set; }

        /// <summary>
        /// 任务类全名
        /// </summary>
        public string JobClassFullName { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; private set; }

        /// <summary>
        /// CronDesc注解
        /// </summary>
        public string CronDesc { get; private set; }

        /// <summary>
        /// 是否运行中
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 作业状态
        /// </summary>
        public JobStatus JobStatus { get; private set; }

        public CreateSyncTaskJobConfigCommand() { }

        public CreateSyncTaskJobConfigCommand(string jobName, string jobDesc, string jobClassFullName,
            string cron, string cronDesc)
        {
            this.JobName = jobName;
            this.JobDesc = jobDesc;
            this.JobClassFullName = jobClassFullName;
            this.Cron = cron;
            this.CronDesc = CronDesc;
            this.IsRunning = false;
            this.JobStatus = JobStatus.UnExecute; 
        }
    }
}
