using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Infrastructure.BaseMessages;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorManager _authorManager;

    public AuthorsController(IAuthorManager authorManager)
    {
        _authorManager = authorManager;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var author = await _authorManager.GetByIdAsync(id);
        return (author == null) ? NotFound(UIMessage.GetNotFoundMessage("author for id")) : Ok(author);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var authors = await _authorManager.GetAllAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{id}/books")]
    public async Task<IActionResult> GetBooksByAuthorId(int id)
    {
        var books = await _authorManager.GetBooksByAuthorIdAsync(id);
        return Ok(books);
    }
}

