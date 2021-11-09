using EbayPlatform.Application.Services;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Infrastructure.Core.Engines;
using Quartz;
using System;
using System.Threading.Tasks;

namespace EbayPlatform.Application.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class DeleteLogJob : IJob, IDependency
    {
        private readonly ISystemLogAppService _systemLogAppService;
        public DeleteLogJob()
        {
            _systemLogAppService = EngineContext.Current.Resolve<ISystemLogAppService>();
        }

        /// <summary>
        /// 根据创建删除过期的日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            return _systemLogAppService.DeleteSystemLogByDateAsync(DateTime.Now.AddDays(-3));
        }
    }
}
