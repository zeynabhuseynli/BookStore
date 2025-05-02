using BookStore.Application.DTOs.Categories;
using BookStore.Domain.Entities.Categories;

namespace BookStore.Application.Interfaces.IManagers.Books;
public interface ICategoryManager
{
    Task<bool> CreateAsync(CreateCategoryDto dto);
    Task<bool> UpdateAsync(UpdateCategoryDto dto);
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
}

