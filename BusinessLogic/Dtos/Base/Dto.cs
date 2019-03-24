using BusinessLogic.Dtos.Interfaces;
using Domain.Entities;

namespace BusinessLogic.Dtos.Base
{
    public abstract class Dto<TDto, TKey> : IDtoEvent<TDto>, IDtoValidator<TDto> where TDto : class where TKey : struct
    {
        public TKey Id { get; set; }
        public bool Active { get; set; }

        public virtual void BeforeGet(TDto dto) { }
        public virtual void BeforeSave(TDto dto) { }
        public virtual void AfterSave(TDto dto) { }
        public virtual void BeforeUpdate(TDto dto) { }
        public virtual void AfterUpdate(TDto dto) { }
        public virtual void BeforeInsert(TDto dto) { }
        public virtual void AfterInsert(TDto dto) { }

        public virtual void ValidateInsert(Validator<TDto> validator) { }
        public virtual void ValidateUpdate(Validator<TDto> validator) { }
        public virtual void ValidateSave(Validator<TDto> validator) { }
    }
}
