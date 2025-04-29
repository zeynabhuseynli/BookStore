using BookStore.Domain.Common;
using BookStore.Domain.Entities.Books;
using BookStore.Domain.Entities.Enums;
using BookStore.Domain.Entities.Users;

namespace BookStore.Domain.Entities.Reviews;
public class Review : BaseEntity
{
    public int FromUserId { get; set; }
    public int? ToUserId { get; set; }
    public User User { get; set; }
    public string? Message { get; set; }
    public int? ParentRewievId { get; set; }
    public Rating? Rating { get; set; }
    public Review? ParentRewiev { get; set; }
    public List<Review> Replies { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public bool IsDeleted { get; set; } = false;
}

