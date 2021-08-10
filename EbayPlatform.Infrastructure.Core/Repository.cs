using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Domain.Interfaces;
using EbayPlatform.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Repository
{
    /// <summary>
    /// 定义通用存储库的实现
    /// </summary>
    public abstract class Repository<TEntity, TKey, TDbContext> :
        IRepository<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot where TDbContext : EFContext
    {
        protected virtual TDbContext DbContext { get; }

        public Repository(TDbContext context)
        {
            this.DbContext = context;
        }

        public virtual IUnitOfWork UnitOfWork => DbContext;
        public IQueryable<TEntity> NoTrackingQueryable => DbContext.Set<TEntity>().AsNoTracking();

        public virtual TEntity Add(TEntity entity)
        {
            return DbContext.Add(entity).Entity;
        }

        public virtual async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return (await DbContext.AddAsync(entity, cancellationToken).ConfigureAwait(false)).Entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            return DbContext.Update(entity).Entity;
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Update(entity));
        }

        public virtual bool Remove(Entity entity)
        {
            DbContext.Remove(entity);
            return true;
        }

        public virtual Task<bool> RemoveAsync(Entity entity)
        {
            return Task.FromResult(Remove(entity));
        }

        public virtual bool Delete(TKey id)
        {
            var entity = DbContext.Find<TEntity>(id);
            if (entity == null)
            {
                return false;
            }
            DbContext.Remove(entity);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await DbContext.FindAsync<TEntity>(id, cancellationToken);
            if (entity == null)
            {
                return false;
            }
            DbContext.Remove(entity);
            return true;
        }

        public virtual TEntity Get(TKey id)
        {
            return DbContext.Find<TEntity>(id);
        }

        public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await DbContext.FindAsync<TEntity>(id, cancellationToken);
        }

    }
}
