using AutoMapper;
using BusinessLogic.Dtos;
using Domain.Entities;

namespace BusinessLogic.MappingProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookDto, Book>().ReverseMap();
        }
    }
}
