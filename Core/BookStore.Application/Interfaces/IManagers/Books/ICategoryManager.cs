using BookStore.Application.DTOs.BookDtos;
using BookStore.Application.DTOs.Categories;

namespace BookStore.Application.Interfaces.IManagers.Books;
public interface ICategoryManager
{
    Task<bool> CreateAsync(CreateCategoryDto dto);
    Task<bool> UpdateAsync(UpdateCategoryDto dto);
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<IEnumerable<CategoryDto>> GetAllAsync();

    Task<IEnumerable<CategoryDto>> GetSubCategoriesByCategoryIdAsync(int categoryId);
    Task<IEnumerable<BookDto>> GetBooksBySubCategoryIdAsync(int subCategoryId);
    Task<bool> DeleteCategoryWithDependenciesAsync(int categoryId);
    Task<IEnumerable<CategoryDto>> GetDeletedCategoriesAsync();


}

