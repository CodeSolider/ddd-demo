using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz.Spi;
using System.Collections.Specialized;

namespace EbayPlatform.Infrastructure.Quartz
{
    /// <summary>
    /// Quartz扩展类
    /// </summary>
    public static class AppQuartzExtensioin
    {
        public static void UseQuartz(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, JobFactory>();

            services.AddSingleton(provider =>
            {
                var props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };

                var schedulerFactory = new StdSchedulerFactory(props);
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();

                return scheduler;
            });
        }
    }
}
