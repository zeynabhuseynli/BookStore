using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Application.Interfaces.IManagers.Books;
using BookStore.Infrastructure.BaseMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.AdminPanel.Controllers
{
    [Route("api/adminPanel/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, SuperAdmin")]
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAuthorDto dto)
        {
            await _authorManager.CreateAsync(dto);
            return Ok();
        }

        // PUT: api/Author/5
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateAuthorDto dto)
        {
            await _authorManager.UpdateAsync(dto);
            return Ok();
        }
    }

}

