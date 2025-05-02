using BookStore.Application.DTOs.BookDtos;

namespace BookStore.Application.Interfaces.IManagers.Books;
public interface IBookManager
{
    Task<bool> CreateAsync(CreateBookDto dto);
    Task<bool> UpdateAsync(UpdateBookDto dto);
    Task<BookDto?> GetByIdAsync(int id);
    Task<IEnumerable<BookDto>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
}

