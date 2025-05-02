using AutoMapper;
using BookStore.Application.DTOs.Categories;
using BookStore.Domain.Entities.Categories;

namespace BookStore.Application.Mappings;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
    }
}

