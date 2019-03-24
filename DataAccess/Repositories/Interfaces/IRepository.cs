using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Base;

namespace DataAccess.Repositories.Interfaces
{
    public interface IRepository<TEntity, in TKey> : IDisposable where TEntity : Entity<TKey> where TKey : struct
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> DeleteAsync(TKey id);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(TKey id);
        Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<int> SaveChangesAsync();
        void RestrictSave();
        void EnableSave();
    }
}
