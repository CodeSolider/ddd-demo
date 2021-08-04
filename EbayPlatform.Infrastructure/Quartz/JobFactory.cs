using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using Quartz.Util;
using System;

namespace EbayPlatform.Infrastructure.Quartz
{
    /// <summary>
    /// 任务工厂->用来管理任务生命周期
    /// </summary>
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public JobFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                if (_serviceProvider.GetRequiredService(bundle.JobDetail.JobType) is IJob job) return job;
                return ObjectUtils.InstantiateType<IJob>(bundle.JobDetail.JobType);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException($"从容器IServiceProvider中构建Job:{bundle.JobDetail.JobType.FullName}失败", ex.Message);
            }
        }

        /// <summary>
        /// 释放Job
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
