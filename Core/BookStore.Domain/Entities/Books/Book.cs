using BookStore.Domain.Common;
using BookStore.Domain.Entities.Authors;
using BookStore.Domain.Entities.Categories;
using BookStore.Domain.Entities.Enums;
using BookStore.Domain.Entities.Reviews;

namespace BookStore.Domain.Entities.Books;
public class Book: BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Path { get; set; }
    public string CoverPath { get; set; }
    public int ViewCount { get; set; } = 0;
    public Rating Rating { get; set; }
    public int AverageRating { get; set; } = 0;
    public int PageCount { get; set; }
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    public Language Language { get; set; }
    public Publisher Publisher { get; set; }
    public ICollection<BookAuthor> Authors { get; set; } = new List<BookAuthor>();

    public List<Review> Reviews { get; set; } = new List<Review>();
}


