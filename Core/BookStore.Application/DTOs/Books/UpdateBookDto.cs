using BookStore.Domain.Entities.Enums;

namespace BookStore.Application.DTOs.Books;
public class UpdateBookDto
{
    public int Id;
    public string Title;
    public string Description;
    public string Path;
    public string CoverPath;
    public int PageCount;
    public Language Language;
    public Publisher Publisher;
}

