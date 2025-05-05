using BookStore.Domain.Entities.Enums;

namespace BookStore.Application.DTOs.ReviewDtos;
public class UpdateReviewDto
{
    public int Id { get; set; }
    public string Message { get; set; }
    public Rating? Rating { get; set; }
}

