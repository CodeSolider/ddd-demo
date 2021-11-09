using EbayPlatform.Domain.Core.Abstractions;
using EbayPlatform.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EbayPlatform.Infrastructure.Repository
{
    /// <summary>
    /// 定义通用存储库的实现
    /// </summary>
    public abstract class Repository<TEntity, TKey, TDbContext> : IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>, IAggregateRoot where TDbContext : EFContext
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
            return DbContext.Set<TEntity>().Add(entity).Entity;
        }

        public virtual async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return (await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false)).Entity;
        }

        public virtual bool AddRange(List<TEntity> entities)
        {
            DbContext.Set<TEntity>().AddRange(entities);
            return true;
        }

        public virtual Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return DbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public virtual TEntity Update(TEntity entity)
        {
            return DbContext.Set<TEntity>().Update(entity).Entity;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().UpdateRange(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
        }

        public virtual bool Delete(TKey id)
        {
            var entity = Get(id);
            if (entity == null)
            {
                return false;
            }
            Remove(entity);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(TKey id)
        {
            var entity = await GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                return false;
            }
            Remove(entity);
            return true;
        }

        public virtual TEntity Get(TKey id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public virtual async Task<TEntity> GetAsync(TKey id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public void Dispose()
        {
            DbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}