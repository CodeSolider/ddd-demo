using EbayPlatform.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;
using static EbayPlatform.Domain.Models.Enums.SyncTask;

namespace EbayPlatform.Domain.Models
{
    /// <summary>
    /// 同步任务作业配置
    /// </summary>
    public class SyncTaskJobConfig : Entity<int>, IAggregateRoot
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

        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTimeOffset CreateDate { get; private set; } = DateTimeOffset.Now;

        /// <summary>
        /// 同步修改时间
        /// </summary>
        public DateTimeOffset? ModifyDate { get; private set; } = DateTimeOffset.Now;

        /// <summary>
        /// 任务参数
        /// </summary>
        public ICollection<SyncTaskJobParam> SyncTaskJobParams { get; private set; }


        protected SyncTaskJobConfig() { }

        public SyncTaskJobConfig(string jobName, string jobDesc,
            string cron, string cronDesc,
            List<SyncTaskJobParam> syncTaskJobParams)
        {
            this.JobName = jobName;
            this.JobDesc = jobDesc;
            this.Cron = cron;
            this.CronDesc = cronDesc;
            this.SyncTaskJobParams = syncTaskJobParams;
        }
    }
}
