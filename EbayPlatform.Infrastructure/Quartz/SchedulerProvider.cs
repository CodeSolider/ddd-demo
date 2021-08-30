using EbayPlatform.Domain.Models;
using Quartz;
using System.Reflection;
using System;
using System.Threading.Tasks;
using System.Threading;

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
        public static async Task StartJob(IScheduler scheduler,
            SyncTaskJobConfig syncTaskJobConfig,
            CancellationToken cancellationToken = default)
        {
            Assembly assembly = Assembly.Load(syncTaskJobConfig.JobClassFullName);
            if (assembly == null)
            {
                throw new ArgumentNullException($"未能加载类型[{syncTaskJobConfig.JobClassFullName}],任务名称[{syncTaskJobConfig.JobName}]");
            }

            IJobDetail job = JobBuilder.Create(assembly.GetType())
                                       .WithIdentity(syncTaskJobConfig.JobName)
                                       .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                             .WithIdentity($"{syncTaskJobConfig.JobName}.trigger")
                                             .StartNow()
                                             .WithCronSchedule(syncTaskJobConfig.Cron)
                                             .Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken)
                           .ConfigureAwait(false);
        }
    }
}
