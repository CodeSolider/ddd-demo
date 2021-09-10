using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;

namespace EbayPlatform.Domain.Core.Abstractions
{
    /// <summary>
    /// 定义通用存储库的接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity, TKey> : IDisposable where TEntity : Entity<TKey>, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        TEntity Add(TEntity entity);
        ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        bool AddRange(List<TEntity> entities);
        Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);


        bool Delete(TKey id);
        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        TEntity Get(TKey id);
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            CancellationToken cancellationToken = default);
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Get List NoTracking
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TEntity> GetNoTrackingList(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Get List Async NoTracking 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetNoTrackingListAsync(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Get List NoTracking
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Get List Async NoTracking 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null);
    }
}
