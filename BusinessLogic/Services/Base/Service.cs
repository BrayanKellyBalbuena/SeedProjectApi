using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLogic.Dtos.Interfaces;
using BusinessLogic.Services.Interfaces;
using DataAccess.Repositories.Interfaces;
using Domain.Base;
using Domain.Entities;
using ValidateException = BusinessLogic.Exception.ValidateException;

namespace BusinessLogic.Services.Base
{
    public  class Service<TDto, TEntity, TKey> : IService<TDto, TKey> where TEntity : Entity<TKey> where TDto : class
        where TKey: struct
    {
        private IMapper _mapper;

        public Service(IRepository<TEntity, TKey> repository, IMapper mapper)
        {
            this.Repository = repository;
            _mapper = mapper;
        }
        public IRepository<TEntity, TKey> Repository { get; protected set; }

        public virtual IQueryable<TDto> GetDto()
        {
            return this.Repository.GetAll().ProjectTo<TDto>();
        }

        public virtual async Task<TDto> Add(TDto dto)
        {
            if (dto is null) throw new ValidateException("Resource not found");
            var dtoEventHandler = dto as IDtoEvent<TDto>;

            if (dtoEventHandler != null)
            {
                dtoEventHandler.BeforeSave(dto);
                dtoEventHandler.BeforeInsert(dto);

            }
            var validator = new Validator<TDto>(dto);
            var dtoValidator = dto as IDtoValidator<TDto>;

            if (dtoValidator != null)
            {
                dtoValidator.ValidateSave(validator);
                dtoValidator.ValidateInsert(validator);
            }
            ValidateDto(validator);
            ValidateAdd(validator);
            validator.Validate();

            var entity = _mapper.Map<TEntity>(dto);
            entity.Active = true;
           
            await Repository.AddAsync(entity);
            await Repository.SaveChangesAsync();

            dto = _mapper.Map<TDto>(entity);
            if (dtoEventHandler != null)
            {
                dtoEventHandler.AfterSave(dto);
                dtoEventHandler.AfterInsert(dto);
            }
            return dto;
        }

        public virtual async Task AddRange(IEnumerable<TDto> dtos)
        {
            if (dtos is null) throw new ValidateException($"Resource not found {nameof(TDto)}");
            Repository.RestrictSave();
            foreach (var dto in dtos) await Add(dto);
            Repository.EnableSave();
            await Repository.SaveChangesAsync();
        }

        public async Task <TDto> Delete(TKey id)
        {
            var validator = new Validator();
            ValidateDelete(id, validator);
            validator.Validate();

            var entity = await Repository.DeleteAsync(id);
            if (entity == null) throw new ArgumentNullException("Resource not found");
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task DeleteRange(IEnumerable<TDto> dtos)
        {
            if (dtos is null) throw new ValidateException($"Resource not found {nameof(TDto)}");
            Repository.RestrictSave();

            var keys = _mapper.Map<IEnumerable<TEntity>>(dtos)?.Select(d => d.Id);

            if (keys != null) foreach (var key in keys) await Delete(key);

            Repository.EnableSave();
            await Repository.SaveChangesAsync();
        }

        public virtual async Task<TDto> Update(TKey key, TDto dto)
        {
            if ( dto is null) throw new ArgumentNullException("Resource not found");
            var dtoEventHandler = dto as IDtoEvent<TDto>;
            if (dtoEventHandler != null)
            {
                dtoEventHandler.BeforeSave(dto);
                dtoEventHandler.BeforeUpdate(dto);
            }
            var entity = await Repository.GetByIdAsync(key);
            _mapper.Map(dto, entity);
            dto = _mapper.Map<TDto>(entity);

            var validator = new Validator<TDto>(dto);
            var dtoValidator = dto as IDtoValidator<TDto>;
            if (dtoValidator != null)
            {
                dtoValidator.ValidateSave(validator);
                dtoValidator.ValidateUpdate(validator);
            }

            ValidateDto(validator);
            ValidateUpdate(validator);
            validator.Validate();

            await Repository.UpdateAsync(entity);
            dto = _mapper.Map<TDto>(entity);
            if (dtoEventHandler != null)
            {
                dtoEventHandler.AfterSave(dto);
                dtoEventHandler.AfterUpdate(dto);
            }
            return dto;
        }

        public virtual async Task UpdateRange(IEnumerable<TDto> dtos)
        {
            if (dtos is null) throw new ArgumentNullException($" {nameof(TDto)}");
            var keyDtos = _mapper.Map<IEnumerable<TEntity>>(dtos)?.Select(d => new { Id = d.Id, dto = _mapper.Map<TDto>(d) });
            Repository.RestrictSave();
            if (keyDtos != null) foreach (var keyDto in keyDtos) await Update(keyDto.Id, keyDto.dto);
            Repository.EnableSave();
            await Repository.SaveChangesAsync();
        }

        public async Task <TDto> GetById(TKey id) {

            var entity = await this.Repository.GetByIdAsync(id);
            var dto = _mapper.Map<TDto>(entity);

            (dto as IDtoEvent<TDto>)?.BeforeGet(dto);

            return dto;
        }

        public async Task<TDto> GetByIdAsync(TKey id)
        {
            var entity = await this.Repository.GetByIdAsync(id);
            var dto = _mapper.Map<TDto>(entity);

            (dto as IDtoEvent<TDto>)?.BeforeGet(dto);

            return dto;
        }

        public virtual IQueryable<TDto> GetAll(Expression<Func<TDto, bool>> filter = null)
        {
            if(filter != null) return _mapper.ProjectTo<TDto>(Repository.GetAll(x => x.Active))
                .Where(filter);

            return _mapper.ProjectTo<TDto>(Repository.GetAll(x => x.Active));
        }

        public void Dispose()
        {
            this.Repository?.Dispose();
            this.Repository = null;
        }

        public virtual void ValidateDto(Validator<TDto> validator) { }
        public virtual void ValidateAdd(Validator<TDto> validator) { }
        public virtual void ValidateUpdate(Validator<TDto> validator) { }
        public virtual void ValidateDelete(TKey id, Validator validator) { }
    }
}
