namespace BookStore.Application.DTOs.Categories;
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ParentCategoryId { get; set; } = 0;

    public DateTime CreatedAt { get; set; }
    public int? CreatedById { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }

    public DateTime? DeletedAt { get; set; }
    public int? DeletedById { get; set; }

    public bool IsDeleted { get; set; }
}

