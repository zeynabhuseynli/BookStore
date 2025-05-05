using BookStore.Application.DTOs.ReviewDtos;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Infrastructure.BaseMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewManager _reviewManager;

    public ReviewController(IReviewManager reviewManager)
    {
        _reviewManager = reviewManager;
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateReviewDto dto)
    {
        var result = await _reviewManager.CreateAsync(dto);
        return result ? Ok(new { message = "Review added successfully" }) : BadRequest();
    }
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateReviewDto dto)
    {
        var result = await _reviewManager.UpdateAsync(dto);
        return result ? Ok(new { message = "Review updated successfully" }) : NotFound();
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _reviewManager.DeleteAsync(id);
        return result ? Ok(new { message = UIMessage.DELETED_MESSAGE }) : NotFound();
    }

    [HttpGet("book/{bookId}")]
    public async Task<IActionResult> GetByBook(int bookId)
    {
        var reviews = await _reviewManager.GetReviewsForBookAsync(bookId);
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var review = await _reviewManager.GetByIdAsync(id);
        return review is null ? NotFound() : Ok(review);
    }
}

