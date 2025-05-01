using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Application.Interfaces.IManagers.Books;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorManager _authorManager;

    public AuthorController(IAuthorManager authorManager)
    {
        _authorManager = authorManager;
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAllAsync()
    {
        var authors = await _authorManager.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet("get-author/{id}")]
    public async Task<ActionResult<AuthorDto>> GetByIdAsync(int id)
    {
        var author = await _authorManager.GetByIdAsync(id);
        return author == null ? NotFound($"Author with ID {id} not found.") : Ok(author);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAuthorDto dto)
    {
        await _authorManager.CreateAsync(dto);
        return Ok(); 
    }

    // PUT: api/Author/5
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateAuthorDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Author ID mismatch.");

        await _authorManager.UpdateAsync(dto);
        return Ok(); 
    }
}

