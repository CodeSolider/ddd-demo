using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetCore.CAP;

namespace EbayPlatform.Infrastructure.Context
{
    /// <summary>
    /// 事务
    /// </summary>
    public class Transaction<TDbContext> : ITransaction where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        /// <summary>
        /// 分布式事物
        /// </summary>
        private readonly ICapPublisher _capPublisher;
        public Transaction(TDbContext dbContext,
            ICapPublisher capPublisher)
        {
            _dbContext = dbContext;
            _capPublisher = capPublisher;
        }


        #region ITransaction

        /// <summary>
        /// 当前事物
        /// </summary>
        private IDbContextTransaction _currentTransaction;

        /// <summary>
        /// 获取当前事物
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        /// <summary>
        /// 是否有事物
        /// </summary>
        public bool HasActiveTransaction => _currentTransaction != null;

        /// <summary>
        /// 开始事物
        /// </summary>
        /// <returns></returns>
        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;
            _currentTransaction = _dbContext.Database.BeginTransaction(_capPublisher, autoCommit: false);
            return Task.FromResult(_currentTransaction);
        }

        /// <summary>
        /// 事物提交
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        /// <summary>
        /// 事物回滚
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        #endregion
    }
}
