using DotNetCore.CAP.Filter;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace EbayPlatform.Infrastructure.Core.Filters
{
    /// <summary>
    /// 订阅者过滤器
    /// </summary>
    public class CapSubscribeFilter : SubscribeFilter
    {
        private readonly ILogger _logger;
        public CapSubscribeFilter([NotNull] ILogger<SubscribeFilter> logger)
        {
            _logger = logger;
        }

        public override void OnSubscribeExecuting(ExecutingContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSubscribeExecuted(ExecutedContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSubscribeException(ExceptionContext context)
        {
            throw new System.NotImplementedException();
        }

    }
}
