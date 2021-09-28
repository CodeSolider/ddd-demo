using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System; 

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
        void UpdateRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);


        bool Delete(TKey id);
        Task<bool> DeleteAsync(TKey id);
        TEntity Get(TKey id);
        ValueTask<TEntity> GetAsync(TKey id);       
    }
}
