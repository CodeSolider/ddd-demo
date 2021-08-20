using EbayPlatform.Domain.Core.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Domain.Interfaces
{
    /// <summary>
    /// 定义通用存储库的接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        TEntity Add(TEntity entity);
        ValueTask<TEntity> AddAsync(TEntity entity);
        bool AddRange(List<TEntity> entities);
        Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);


        bool Delete(TKey id);
        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        TEntity Get(TKey id);
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
    }
}
