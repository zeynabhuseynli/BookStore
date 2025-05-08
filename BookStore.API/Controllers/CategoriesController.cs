using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Infrastructure.BaseMessages;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryManager _categoryManager;

    public CategoriesController(ICategoryManager categoryManager)
    {
        _categoryManager = categoryManager;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryManager.GetByIdAsync(id);
        return category == null ? NotFound(UIMessage.GetNotFoundMessage("Category")) : Ok(category);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryManager.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{categoryId}/subcategories")]
    public async Task<IActionResult> GetSubCategories(int categoryId)
    {
        var subCategories = await _categoryManager.GetSubCategoriesByCategoryIdAsync(categoryId);
        return Ok(subCategories);
    }

    [HttpGet("subcategory/{subCategoryId}/books")]
    public async Task<IActionResult> GetBooksBySubCategory(int subCategoryId)
    {
        var books = await _categoryManager.GetBooksBySubCategoryIdAsync(subCategoryId);
        return Ok(books);
    }
}
