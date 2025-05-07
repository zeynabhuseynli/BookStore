using AutoMapper;
using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Domain.Entities.Authors;
using BookStore.Infrastructure.Utils;

namespace BookStore.Application.Mappings;
public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorDto>()
            .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.Books.Count));

        CreateMap<CreateAuthorDto, Author>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Capitalize()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Capitalize()));

        CreateMap<UpdateAuthorDto, Author>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Capitalize()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Capitalize()));

    }
}