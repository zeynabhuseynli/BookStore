using BookStore.Application.DTOs.BookDtos;
using BookStore.Application.Interfaces.IManagers.Books;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookManager _bookManager;

    public BookController(IBookManager bookManager)
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

    [HttpPost("create")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAsync([FromForm] CreateBookDto dto)
    {
        await _bookManager.CreateAsync(dto);
        return Ok();
    }

    [HttpPut("update")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateAsync([FromForm] UpdateBookDto dto)
    {
        await _bookManager.UpdateAsync(dto);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _bookManager.DeleteAsync(id);
        return Ok();
    }
}

