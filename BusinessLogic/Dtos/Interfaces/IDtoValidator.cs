using Domain.Entities;

namespace BusinessLogic.Dtos.Interfaces
{
    public interface IDtoValidator<TDto>
    {
        void ValidateInsert(Validator<TDto> validator);
        void ValidateUpdate(Validator<TDto> validator);
        void ValidateSave(Validator<TDto> validator);
    }
}
