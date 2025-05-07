using BookStore.Application.DTOs.Categories;
using BookStore.Application.Interfaces.IManagers.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.AdminPanel.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles ="Admin, SuperAdmin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;

        public CategoriesController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            await _categoryManager.CreateAsync(dto);
            return Ok("Category created successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryDto dto)
        {
            await _categoryManager.UpdateAsync(dto);
            return Ok("Category updated successfully.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryManager.DeleteCategoryWithDependenciesAsync(id);
            return Ok("Category and its dependencies deleted successfully.");
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedCategories()
        {
            var deletedCategories = await _categoryManager.GetDeletedCategoriesAsync();
            return Ok(deletedCategories);
        }
    }
}

