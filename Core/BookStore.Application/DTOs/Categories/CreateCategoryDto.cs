namespace BookStore.Application.DTOs.Categories;

public class CreateCategoryDto
{
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; } = null;
}

