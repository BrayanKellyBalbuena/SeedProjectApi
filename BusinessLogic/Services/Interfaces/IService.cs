using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;

namespace BusinessLogic.Services.Interfaces
{
    public interface IService<TDto, TKey> : IDisposable
    {
        Task<TDto> Add(TDto dto);
        Task AddRange(IEnumerable<TDto> dtos);
        Task<TDto> Update(TKey key, TDto entity);
        Task UpdateRange(IEnumerable<TDto> dtos);
        Task<TDto> Delete(TKey id);
        Task DeleteRange(IEnumerable<TDto> dtos);
        Task<TDto> GetById(TKey id);
        Task<TDto> GetByIdAsync(TKey id);
        IQueryable<TDto> GetAll(Expression<Func<TDto, bool>> filter = null);

        void ValidateDto(Validator<TDto> validator);
        void ValidateAdd(Validator<TDto> validator);
        void ValidateUpdate(Validator<TDto> validator);
        void ValidateDelete(TKey id, Validator validator);
    }
}
