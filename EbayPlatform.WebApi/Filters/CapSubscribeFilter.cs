using DotNetCore.CAP.Filter;
using EbayPlatform.Application.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace EbayPlatform.WebApi.Filters
{
    /// <summary>
    /// 订阅者过滤器
    /// </summary>
    public class CapSubscribeFilter : SubscribeFilter
    {
        private readonly ILogger<CapSubscribeFilter> _logger;
        private readonly ISystemLogAppService _systemLogAppService;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public CapSubscribeFilter([NotNull] ILogger<CapSubscribeFilter> logger,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
            ISystemLogAppService systemLogAppService)
        {
            _logger = logger;
            _systemLogAppService = systemLogAppService;
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
            _systemLogAppService.AddSystemLogAsync(new Application.Dtos.SystemLogDto
            {
                ObjectId = context.ConsumerDescriptor.TopicName,
                LogType = Domain.Models.Enums.LogType.EbayPlatform,
                Content = $"错误原因为：【{context.ConsumerDescriptor}】</br> 参数为【{JsonConvert.SerializeObject(context.DeliverMessage.Headers)}】"
            });
        }
    }
}
