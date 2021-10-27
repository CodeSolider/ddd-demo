using EbayPlatform.Application.Quartz;
using EbayPlatform.Application.Services;
using EbayPlatform.Infrastructure.Core.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using EbayPlatform.Application.Dtos;

namespace EbayPlatform.WebApi.HostedService
{
    /// <summary>
    /// Quazrt 托管服务
    /// </summary>
    public class QuartzHostedService : BackgroundService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ISyncTaskJobAppService _syncTaskJobAppService;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public QuartzHostedService(ISchedulerFactory schedulerFactory,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
            IServiceScopeFactory serviceScopeFactory)
        {
            _schedulerFactory = schedulerFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _syncTaskJobAppService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISyncTaskJobAppService>();
        }

        /// <summary>
        /// 任务计划
        /// </summary>
        private IScheduler Scheduler { get; set; }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(stoppingToken).ConfigureAwait(false);
            while (!stoppingToken.IsCancellationRequested)
            {
                var syncTaskJobConfigList = await _syncTaskJobAppService
                                                   .GetSyncTaskJobConfigListAsync(cancellationToken: stoppingToken)
                                                   .ConfigureAwait(false);

                syncTaskJobConfigList.ForEach(async syncTaskJobConfigItem =>
                {
                    await QuartzProvider.StartJobAsync(Scheduler, syncTaskJobConfigItem.Adapt<SyncTaskJobConfigDto>()).ConfigureAwait(false);
                });
                Scheduler.JobFactory = new JobFactory(_serviceScopeFactory);
                await Scheduler.Start(stoppingToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Scheduler?.Shutdown(cancellationToken);
        }
    }
}
