using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Infrastructure.BaseMessages;
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
        return author == null ? NotFound(UIMessage.GetNotFoundMessage("Author id")) : Ok(author);
    }
}

