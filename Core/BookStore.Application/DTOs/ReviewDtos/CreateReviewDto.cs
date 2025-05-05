using BookStore.Domain.Entities.Enums;

namespace BookStore.Application.DTOs.ReviewDtos;
public class CreateReviewDto
{
    public int BookId { get; set; }
    public int? ToUserId { get; set; }
    public string Message { get; set; }
    public Rating? Rating { get; set; }
    public int? ParentReviewId { get; set; }
}

