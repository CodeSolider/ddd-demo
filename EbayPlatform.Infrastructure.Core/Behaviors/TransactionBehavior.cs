using DotNetCore.CAP;
using EbayPlatform.Infrastructure.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Core.Behaviors
{
    /// <summary>
    /// 事物行为
    /// </summary>
    public class TransactionBehavior<TDbContext, TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TDbContext : EFContext
    {
        private readonly ILogger _logger;
        private readonly TDbContext _dbContext;

        /// <summary>
        /// 分布式事务
        /// </summary>
        private readonly ICapPublisher _capPublisher;
        public TransactionBehavior(TDbContext dbContext,
            ICapPublisher capPublisher,
            ILogger logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _capPublisher = capPublisher;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();
            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;
                    using (var transaction = await _dbContext.BeginTransactionAsync(_capPublisher).ConfigureAwait(false))
                    using (_logger.BeginScope("TransactionContext:{TransactionId}", transaction.TransactionId))
                    {
                        _logger.LogInformation("----- 开始事务 {TransactionId} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation("----- 提交事务 {TransactionId} {CommandName}", transaction.TransactionId, typeName);

                        await _dbContext.CommitTransactionAsync(transaction).ConfigureAwait(false);
                        transactionId = transaction.TransactionId;
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理事务出错 {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}
