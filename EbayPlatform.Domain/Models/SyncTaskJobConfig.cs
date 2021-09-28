﻿using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Events.SyncTaskJobConfig;
using EbayPlatform.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Job程序集
        /// </summary>
        public string JobAssemblyName { get; private set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; private set; }

        /// <summary>
        /// CronDesc注解
        /// </summary>
        public string CronDesc { get; private set; }

        /// <summary>
        /// 作业状态
        /// </summary>
        public JobStatusType JobStatus { get; private set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime CreateDate { get; private set; } = DateTime.Now;

        /// <summary>
        /// 同步修改时间
        /// </summary>
        public DateTime? ModifyDate { get; private set; } = DateTime.Now;

        /// <summary>
        /// 店铺任务
        /// </summary>
        public virtual ICollection<ShopTask> ShopTasks { get; private set; }


        protected SyncTaskJobConfig()
        {
            this.ShopTasks = new List<ShopTask>();
        }

        public SyncTaskJobConfig(string jobName, string jobDesc,
            string jobAssemblyName, string cron, string cronDesc) : this()
        {
            this.JobName = jobName;
            this.JobDesc = jobDesc;
            this.JobAssemblyName = jobAssemblyName;
            this.Cron = cron;
            this.CronDesc = cronDesc;
            this.JobStatus = JobStatusType.UnExecute;
            ////添加事件
            //this.AddDomainEvent(new CreateSyncTaskJobConfigDomainEvent(this));
        }

        /// <summary>
        /// 更新作业状态
        /// </summary>
        /// <param name="JobStatus"></param>
        public void ChangeSyncTaskJobConfigJobStatus(JobStatusType jobStatus)
        {
            this.JobStatus = jobStatus;
        }

        /// <summary>
        /// 添加店铺任务
        /// </summary>
        public void AddShopTask(string shopName, string paramValue)
        {
            var existsShopTask = this.ShopTasks.FirstOrDefault(o => o.ShopName.Equals(shopName));
            if (existsShopTask != null)
            {
                existsShopTask.ChangeShopTaskParamValue(paramValue);
            }
            else
            {
                existsShopTask = new ShopTask(shopName, paramValue);
                this.ShopTasks.Add(existsShopTask);
            }
        }

    }
}
