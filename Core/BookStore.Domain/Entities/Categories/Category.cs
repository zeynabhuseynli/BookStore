using BookStore.Domain.Common;

namespace BookStore.Domain.Entities.Categories;
public class Category:BaseEntity
{
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; } = 0;
    public Category? ParentCategory { get; set; }
    public List<Category>? SubCategories { get; set; } = new();
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}

