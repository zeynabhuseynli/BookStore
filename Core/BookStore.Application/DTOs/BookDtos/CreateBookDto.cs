using BookStore.Domain.Entities.Enums;
using Microsoft.AspNetCore.Http;

namespace BookStore.Application.DTOs.BookDtos;
public class CreateBookDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int PageCount { get; set; }
    public Language Language { get; set; }
    public Publisher Publisher { get; set; }
    public List<int> CategoryIds { get; set; }
    public List<int> AuthorIds { get; set; }

    public IFormFile PdfFile { get; set; }
    public IFormFile CoverImage { get; set; }
}

