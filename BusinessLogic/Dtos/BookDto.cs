using System;
using BusinessLogic.Dtos.Base;
using Domain.Entities;
using FluentValidation;

namespace BusinessLogic.Dtos
{
    public class BookDto : Dto<BookDto, int>
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public override void ValidateInsert(Validator<BookDto> validator)
        {
            validator.RuleFor(e => e.Author).NotNull()
                .WithMessage($"{nameof(Author)} Not valid");
            validator.Validate();
        }
    }

    
}
