using BookStore.Domain.Entities.Enums;

namespace BookStore.Application.DTOs.BookDtos;
public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int PageCount { get; set; }
    public string Path { get; set; }
    public int ViewCount { get; set; } = 0;//neçə nəfərin baxdığını göstərir
    public string CoverPath { get; set; }
    public Language Language { get; set; }
    public Publisher Publisher { get; set; }
    public List<int> CategoryIds { get; set; }
    public List<int> AuthorIds { get; set; }

    public DateTime CreatedAt { get; set; }
    public int? CreatedById { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }

    public DateTime? DeletedAt { get; set; }
    public int? DeletedById { get; set; }

    public bool IsDeleted { get; set; } 
}

