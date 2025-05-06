using BookStore.Application.DTOs.ReviewDtos;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Infrastructure.BaseMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly IReviewManager _reviewManager;

    public ReviewsController(IReviewManager reviewManager)
    {
        _reviewManager = reviewManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {
        await _reviewManager.CreateAsync(dto);
        return Ok(UIMessage.ADD_MESSAGE);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateReviewDto dto)
    {
        await _reviewManager.UpdateAsync(dto);
        return Ok(UIMessage.UPDATE_MESSAGE);
    }

    [HttpDelete("with-replies/{id}")]
    public async Task<IActionResult> SoftDeleteWithReplies(int id)
    {
        await _reviewManager.DeleteReviewWithRepliesAsync(id);
        return Ok(UIMessage.DELETED_MESSAGE);
    }

    [HttpGet("book/all/{bookId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllReviewsWithRepliesForBook(int bookId)
    {
        var result = await _reviewManager.GetAllReviewsWithRepliesForBookAsync(bookId);
        return Ok(result);
    }

    [HttpGet("replies/{reviewId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRepliesByReviewId(int reviewId)
    {
        var result = await _reviewManager.GetRepliesByReviewIdAsync(reviewId);
        return Ok(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserReviewsAndReplies([FromQuery] int? userId)
    {
        var result = await _reviewManager.GetUserReviewsAndRepliesAsync(userId);
        return Ok(result);
    }

    [HttpGet("replies-to")]
    public async Task<IActionResult> GetRepliesToUser([FromQuery] int? userId)
    {
        var result = await _reviewManager.GetRepliesToUserAsync(userId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var review = await _reviewManager.GetByIdAsync(id);
        if (review == null)
            return NotFound(UIMessage.GetNotFoundMessage("Review for id"));
        return Ok(review);
    }
}

