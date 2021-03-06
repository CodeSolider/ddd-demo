using DotNetCore.CAP;
using EbayPlatform.Infrastructure.Core.Behaviors;
using Microsoft.Extensions.Logging;

namespace EbayPlatform.Infrastructure.Context
{
    /// <summary>
    /// EbayPlatformDbContext 事务
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class EbayPlatformContextTransactionBehavior<TRequest, TResponse> :
        TransactionBehavior<EbayPlatformDbContext, TRequest, TResponse>
    {
        public EbayPlatformContextTransactionBehavior(EbayPlatformDbContext dbContext,
            ICapPublisher capPublisher,
            ILogger<EbayPlatformContextTransactionBehavior<TRequest, TResponse>> logger)
            : base(dbContext, capPublisher, logger)
        {
        }
    }
}
