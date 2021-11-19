using DotNetCore.CAP.Filter;
using EbayPlatform.Domain.Commands.StystemLog;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EbayPlatform.Infrastructure.Filters
{
    public class CapSubscribeFilter : SubscribeFilter
    {
        private readonly ILogger<CapSubscribeFilter> _logger;
        private readonly IMediator _mediator;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public CapSubscribeFilter(ILogger<CapSubscribeFilter> logger, IMediator mediator)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            _logger = logger;  
            _mediator = mediator;
        }

        /// <summary>
        /// 开始订阅
        /// </summary>
        /// <param name="context"></param>
        public override void OnSubscribeExecuting(ExecutingContext context)
        {
            _logger.LogDebug($"订阅的名称为:【{context.ConsumerDescriptor.TopicName}】参数为:【{JsonConvert.SerializeObject(context.Arguments)}】");
        }

        /// <summary>
        /// 执行完成
        /// </summary>
        /// <param name="context"></param>
        public override void OnSubscribeExecuted(ExecutedContext context)
        {
            _logger.LogDebug($"订阅的名称为:【{context.ConsumerDescriptor.TopicName}】执行成功");
        }

        /// <summary>
        /// 订阅发生异常时
        /// </summary>
        /// <param name="context"></param>
        public override void OnSubscribeException(ExceptionContext context)
        {
            _mediator.Send(new CreateSystemLogCommand(context.ConsumerDescriptor.TopicName, Domain.AggregateModel.Enums.LogType.EbayPlatform,
                            $"错误原因为：【{context.Exception.Message}】</br> 参数为【{JsonConvert.SerializeObject(context.DeliverMessage.Headers)}】"))
                     .Wait();
        }
    }
}
