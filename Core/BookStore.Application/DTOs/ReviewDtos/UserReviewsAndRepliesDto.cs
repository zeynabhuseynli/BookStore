using BookStore.Domain.Entities.Reviews;

namespace BookStore.Application.DTOs.ReviewDtos;
public class UserReviewsAndRepliesDto
{
    public int UserId { get; set; }
    public List<Review> Reviews { get; set; }
    public List<Review> Replies { get; set; }
}


