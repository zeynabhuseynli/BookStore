using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Infrastructure.BaseMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.AdminPanel.Controllers;
[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, SuperAdmin")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorManager _authorManager;

    public AuthorsController(IAuthorManager authorManager)
    {
        _authorManager = authorManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAuthorDto dto)
    {
        await _authorManager.CreateAsync(dto);
        return Ok(UIMessage.ADD_MESSAGE);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAuthorDto dto)
    {
        await _authorManager.UpdateAsync(dto);
        return Ok(UIMessage.UPDATE_MESSAGE);
    }
}

