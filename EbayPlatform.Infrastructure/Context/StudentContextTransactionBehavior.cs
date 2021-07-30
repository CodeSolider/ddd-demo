using EbayPlatform.Infrastructure.Behaviors;
using Microsoft.Extensions.Logging;

namespace EbayPlatform.Infrastructure.Context
{
    /// <summary>
    /// Student DbContext 事物
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class StudentContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<StudentDbContext, TRequest, TResponse>
    {
        public StudentContextTransactionBehavior(StudentDbContext dbContext,
            ITransaction transaction,
            ILogger<StudentContextTransactionBehavior<TRequest, TResponse>> logger)
            : base(dbContext, transaction, logger)
        {
        }
    }
}
