using BookStore.Application.DTOs.BookDtos;
using BookStore.Application.Interfaces.IManagers.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookManager _bookManager;

    public BooksController(IBookManager bookManager)
    {
        _bookManager = bookManager;
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAllAsync()
    {
        var books = await _bookManager.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("get-book/{id}")]
    public async Task<ActionResult<BookDto>> GetByIdAsync(int id)
    {
        var book = await _bookManager.GetByIdAsync(id);
        return book == null ? NotFound() : Ok(book);
    }

    [HttpGet("{id:int}/authors")]
    public async Task<IActionResult> GetAuthorsByBookId(int id)
    {
        var authors = await _bookManager.GetAuthorIdsByBookIdAsync(id);
        return Ok(authors);
    }

    [HttpGet("{id:int}/categories")]
    public async Task<IActionResult> GetCategoriesByBookId(int id)
    {
        var categories = await _bookManager.GetCategoryIdsByBookIdAsync(id);
        return Ok(categories);
    }

    [HttpPost("{id:int}/send-pdf")]
    [Authorize] // login olmadan bu endpointə giriş yoxdur
    public async Task<IActionResult> SendPdfToEmail(int id)
    {
        await _bookManager.SendBookPdfToEmailAsync(id);
        return Ok("PDF kitab emailinizə göndərildi.");
    }
}

