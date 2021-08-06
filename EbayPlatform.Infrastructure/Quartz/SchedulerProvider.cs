using EbayPlatform.Domain.Models;
using Quartz;
using System.Reflection;
using System;

namespace EbayPlatform.Infrastructure.Quartz
{
    /// <summary>
    /// 任务提供器
    /// </summary>
    public class SchedulerProvider
    {
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="syncTaskJobConfig"></param>
        public async static void StartJob(IScheduler scheduler, SyncTaskJobConfig syncTaskJobConfig)
        {
            Assembly assembly = Assembly.Load(syncTaskJobConfig.JobClassFullName);
            if (assembly == null)
            {
                throw new ArgumentNullException($"未能加载类型[{syncTaskJobConfig.JobClassFullName}]");
            }

            var job = JobBuilder.Create(assembly.GetType())
                .WithIdentity(syncTaskJobConfig.JobName)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{syncTaskJobConfig.JobName}.trigger")
                .StartNow()
                .WithCronSchedule(syncTaskJobConfig.Cron)
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
