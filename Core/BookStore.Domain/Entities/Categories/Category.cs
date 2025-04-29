using BookStore.Domain.Common;

namespace BookStore.Domain.Entities.Categories;
public class Category:BaseEntity
{
    public string Name { get; set; }
    public int? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }
    public List<Category> SubCategories { get; private set; }
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}

