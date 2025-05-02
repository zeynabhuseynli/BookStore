using BookStore.Application.DTOs.Categories;
using BookStore.Application.Interfaces.IManagers.Books;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryManager _categoryManager;

    public CategoryController(ICategoryManager categoryManager)
    {
        _categoryManager = categoryManager;
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync()
    {
        var categories = await _categoryManager.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("get-category/{id}")]
    public async Task<ActionResult<CategoryDto>> GetByIdAsync(int id)
    {
        var category = await _categoryManager.GetByIdAsync(id);
        return category == null ? NotFound($"Category with ID {id} not found.") : Ok(category);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryDto dto)
    {
        await _categoryManager.CreateAsync(dto);
        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateCategoryDto dto)
    {
        await _categoryManager.UpdateAsync(dto);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _categoryManager.DeleteAsync(id);
        return result ? Ok($"Category with ID {id} deleted.") : NotFound();
    }
}


