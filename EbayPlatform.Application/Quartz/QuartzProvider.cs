using EbayPlatform.Application.Dtos; 
using Quartz;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz
{
    public class QuartzProvider
    {
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="scheduler">调度器</param>
        /// <param name="syncTaskJobConfig">任务配置信息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public static async Task StartJobAsync(IScheduler scheduler,
            SyncTaskJobConfigDto syncTaskJobConfig,
           CancellationToken cancellationToken = default)
        {
            Assembly assembly = Assembly.Load(syncTaskJobConfig.JobAssemblyName);
            if (assembly == null)
            {
                throw new ArgumentNullException($"未能加载程序集[{syncTaskJobConfig.JobAssemblyName}]");
            }

            TypeInfo typeInfo = assembly.DefinedTypes.FirstOrDefault(o => o.Name == syncTaskJobConfig.JobName);
            if (typeInfo == null)
            {
                return;
            }

            bool isExists = await scheduler.CheckExists(new JobKey(syncTaskJobConfig.JobName), cancellationToken).ConfigureAwait(false);
            if (isExists)
            {
                return;
            }

            IJobDetail job = JobBuilder.Create(typeInfo)
                                       .WithIdentity(syncTaskJobConfig.JobName)
                                       .WithDescription(syncTaskJobConfig.JobDesc)
                                       .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                             .WithIdentity($"{syncTaskJobConfig.JobName}.trigger")
                                             .WithCronSchedule(syncTaskJobConfig.Cron)
                                             .WithDescription(syncTaskJobConfig.CronDesc)
                                             .StartNow()
                                             .Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken).ConfigureAwait(false);
        }
    }
}
