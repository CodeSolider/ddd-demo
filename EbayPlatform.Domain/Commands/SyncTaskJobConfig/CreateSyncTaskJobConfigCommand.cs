using EbayPlatform.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.Commands.SyncTaskJobConfig
{
    public class CreateSyncTaskJobConfigCommand : IRequest<Models.SyncTaskJobConfig>
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; }

        /// <summary>
        /// 作业注解
        /// </summary>
        public string JobDesc { get; }

        /// <summary>
        /// 任务程序集名称
        /// </summary>
        public string JobAssemblyName { get; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; }

        /// <summary>
        /// CronDesc注解
        /// </summary>
        public string CronDesc { get; }

        /// <summary>
        /// 同步任务参数
        /// </summary>
        public List<SyncTaskJobParam> SyncTaskJobParams { get; }


        public CreateSyncTaskJobConfigCommand(string jobName, string jobDesc,
            string jobAssemblyName, string cron, string cronDesc,
            List<SyncTaskJobParam> syncTaskJobParams)
        {
            this.JobName = jobName;
            this.JobDesc = jobDesc;
            this.JobAssemblyName = jobAssemblyName;
            this.Cron = cron;
            this.CronDesc = cronDesc;
            this.SyncTaskJobParams = syncTaskJobParams;
        }
    }
}
