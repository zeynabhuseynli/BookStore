using AutoMapper;
using BookStore.Application.DTOs.BookDtos;
using BookStore.Domain.Entities.Books;

namespace BookStore.Application.Mappings;
public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>()
           .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.BookCategories.Select(bc => bc.CategoryId)))
           .ForMember(dest => dest.AuthorIds, opt => opt.MapFrom(src => src.Authors.Select(ba => ba.AuthorId)))
           .ReverseMap();

        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();
    }
}

