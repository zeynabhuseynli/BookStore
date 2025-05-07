using AutoMapper;
using BookStore.Application.DTOs.Categories;
using BookStore.Domain.Entities.Categories;
using BookStore.Infrastructure.Utils;

namespace BookStore.Application.Mappings;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CreateCategoryDto, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Capitalize()));
        CreateMap<UpdateCategoryDto, Category>();
    }
}

