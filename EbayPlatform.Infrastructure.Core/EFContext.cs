using DotNetCore.CAP;
using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Infrastructure.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Core
{
    /// <summary>
    /// https://github.com/dotnetcore/CAP/issues/539
    /// </summary>
    public class EFContext : DbContext, IUnitOfWork
    {
        protected readonly IMediator _mediator;
        public EFContext(DbContextOptions options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        #region IUnitOfWork
        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            _ = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await _mediator.DispatchDomainEventsAsync(this).ConfigureAwait(false);
            return true;
        }
        #endregion

        #region Transaction

        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;
            return Database.BeginTransactionAsync();
        }

        /// <summary>
        /// 分布式事务开启
        /// </summary>
        /// <param name="capPublisher"></param>
        /// <returns></returns>
        public Task<IDbContextTransaction> BeginTransactionAsync(ICapPublisher capPublisher)
        {
            if (_currentTransaction != null) return null;
            _currentTransaction = Database.BeginTransaction(capPublisher, autoCommit: false);
            return Task.FromResult(_currentTransaction);
        }


        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await base.SaveChangesAsync().ConfigureAwait(false);
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
