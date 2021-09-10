using EbayPlatform.Infrastructure.Core.ThirdComponents;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace EbayPlatform.Infrastructure.Core.Quartz
{
    /// <summary>
    /// Quartz扩展类
    /// </summary>
    public static class AppQuartzExtension
    {
        public static void UseQuartz(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        }
    }
}
