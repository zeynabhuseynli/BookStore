using BookStore.Domain.Entities.Enums;

namespace BookStore.Application.DTOs.ReviewDtos;
public class ReviewDto
{
    public int Id { get; set; }
    public string FromUser { get; set; }
    public string? Message { get; set; }
    public Rating? Rating { get; set; }
    public int? ParentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ReviewDto>? Replies { get; set; }
}

