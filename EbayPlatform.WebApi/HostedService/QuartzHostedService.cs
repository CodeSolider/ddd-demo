using EbayPlatform.Application.Services;
using EbayPlatform.Infrastructure.Core.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.WebApi.HostedService
{
    /// <summary>
    /// Quazrt 托管服务
    /// </summary>
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ISyncTaskJobAppService _syncTaskJobAppService;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public QuartzHostedService(ISchedulerFactory schedulerFactory,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
            IServiceScopeFactory serviceScopeFactory,
            ISyncTaskJobAppService syncTaskJobAppService)
        {
            _schedulerFactory = schedulerFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _syncTaskJobAppService = syncTaskJobAppService;
        }

        /// <summary>
        /// 任务计划
        /// </summary>
        public IScheduler Scheduler { get; set; }

        /// <summary>
        /// 开始托管
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);
            var syncTaskJobConfigList = await _syncTaskJobAppService.GetSyncTaskJobConfigListByAsync(cancellationToken).ConfigureAwait(false);

            syncTaskJobConfigList.ForEach(async syncTaskJobConfigItem =>
            {
                await QuartzProvider.StartJobAsync(Scheduler, syncTaskJobConfigItem).ConfigureAwait(false);
            });
            Scheduler.JobFactory = new JobFactory(_serviceScopeFactory);
            await Scheduler.Start(cancellationToken).ConfigureAwait(false);

        }

        /// <summary>
        /// 结束托管
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Scheduler?.Shutdown(cancellationToken);
        }
    }
}
