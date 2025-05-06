namespace BookStore.Application.DTOs.ReviewDtos;
public class IncomingReplyDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int FromUserId { get; set; }
    public int? ToUserId { get; set; }
    public int ParentReviewId { get; set; }
    public string ParentContent { get; set; }
}
