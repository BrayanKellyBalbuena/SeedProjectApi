using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiCore.DataAccess.Exceptions;
using DataAccess.Repositories.Interfaces;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Base
{
    public class Repository<TContext, TEntity, TKey> : IDisposable, IRepository<TEntity,
        TKey> where TEntity : Entity<TKey> where TKey : struct
        where TContext : DbContext
    {
        private bool _save = true;
        private readonly TContext _context;
        private readonly DbSet<TEntity> _set;

        public Repository(TContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            entity.Active = true;
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            RestrictSave();
            foreach (var entity in entities)  await AddAsync(entity);
            EnableSave();
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            RestrictSave();
            foreach (var entity in entities) await UpdateAsync(entity);
            EnableSave();
            await SaveChangesAsync();
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity entity)
        {
            entity.Active = false;

            await SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) throw new RepositoryException($"id: {id} not found to delete");
            entity = await DeleteAsync(entity);
            return entity;
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            var dto = await _set.FirstOrDefaultAsync(e => e.Id.Equals(id) & e.Active);

            return dto;
        }

        public virtual Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if(filter == null) return _set.FirstOrDefaultAsync();
            return _set.FirstOrDefaultAsync(filter);
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _set;
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);

            return query;
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            if (_save) return await _context.SaveChangesAsync();
            return 0;
        }

        public virtual void RestrictSave() { _save = false; }
        public virtual void EnableSave() { _save = true; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
