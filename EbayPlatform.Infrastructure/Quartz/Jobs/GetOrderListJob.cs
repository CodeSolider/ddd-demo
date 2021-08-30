using Quartz;
using System;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Quartz.Jobs
{
    /// <summary>
    /// 获取所有订单数据
    /// </summary>
    [DisallowConcurrentExecution]
    public class GetOrderListJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
