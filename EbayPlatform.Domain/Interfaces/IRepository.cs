using EbayPlatform.Domain.Abstractions;
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
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        bool Remove(Entity entity);
        Task<bool> RemoveAsync(Entity entity);

        bool Delete(TKey id);
        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        TEntity Get(TKey id);
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
    }
}
