using BookStore.Application.DTOs.BookDtos;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Infrastructure.BaseMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.AdminPanel.Controllers;
[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles ="SuperAdmin, Admin")]
public class BooksController : ControllerBase
{
    private readonly IBookManager _bookManager;

    public BooksController(IBookManager bookManager)
    {
        _bookManager = bookManager;
    }

    [HttpPost("create")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAsync([FromForm] CreateBookDto dto)
    {
        await _bookManager.CreateAsync(dto);
        return Ok(UIMessage.ADD_MESSAGE);
    }

    [HttpPut("update")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateAsync([FromForm] UpdateBookDto dto)
    {
        await _bookManager.UpdateAsync(dto);
        return Ok(UIMessage.UPDATE_MESSAGE);
    }

    [HttpPut("recover/{id}")]
    public async Task<IActionResult> RecoverAsync(int id)
    {
        await _bookManager.RecoverBookAsync(id);
        return Ok(UIMessage.UPDATE_MESSAGE);
    }

    [HttpDelete("soft_delete/{id}")]
    public async Task<IActionResult> SoftDeleteAsync(int id)
    {
        await _bookManager.SoftDeleteAsync(id);
        return Ok(UIMessage.DELETED_MESSAGE);
    }

    [HttpDelete("hard_delete/{id}")]
    public async Task<IActionResult> HardDeleteAsync(int id)
    {
        await _bookManager.HardDeleteAsync(id);
        return Ok(UIMessage.HARD_DELETED_MESSAGE);
    }
}