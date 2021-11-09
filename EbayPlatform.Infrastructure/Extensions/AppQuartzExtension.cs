using EbayPlatform.Infrastructure.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi; 

namespace EbayPlatform.Infrastructure.Extensions
{
    /// <summary>
    /// Quartz扩展类
    /// </summary>
    public static class AppQuartzExtension
    {
        /// <summary>
        /// 注入Quartz
        /// </summary>
        /// <param name="services"></param> 
        public static void UseQuartz(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        }
    }
}
