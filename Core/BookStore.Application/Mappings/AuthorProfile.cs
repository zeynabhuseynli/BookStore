using AutoMapper;
using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Domain.Entities.Authors;

namespace BookStore.Application.Mappings;
public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorDto>()
            .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.Books.Count));

        CreateMap<CreateAuthorDto, Author>();
        CreateMap<UpdateAuthorDto, Author>();

    }
}