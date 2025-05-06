using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Application.DTOs.BookDtos;

namespace BookStore.Application.Interfaces.IManagers.Books;
public interface IAuthorManager
{
    
    Task<bool> CreateAsync(CreateAuthorDto dto);
    Task<bool> UpdateAsync(UpdateAuthorDto dto);

    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(int authorId);
}

