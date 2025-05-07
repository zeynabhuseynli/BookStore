using BookStore.Application.Interfaces.IManagers;
using BookStore.Infrastructure.BaseMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.AdminPanel.Controllers;
[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles ="SuperAdmin, Admin")]
public class ReviewsController : Controller
{
    private readonly IReviewManager _reviewManager;

    public ReviewsController(IReviewManager reviewManager)
    {
        _reviewManager = reviewManager;
    }

    [HttpGet("deleted")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetDeletedReviewsWithReplies()
    {
        var result = await _reviewManager.GetDeletedReviewsWithRepliesAsync();
        return Ok(result);
    }

    [HttpPut("restore-with-replies/{id}")]
    public async Task<IActionResult> RestoreDeletedWithReplies(int id)
    {
        await _reviewManager.DeleteReviewWithRepliesAsync(id,false);
        return Ok(UIMessage.UPDATE_MESSAGE);
    }
}

