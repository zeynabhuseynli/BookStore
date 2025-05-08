using BookStore.Application.DTOs.BookDtos;

namespace BookStore.Application.Interfaces.IManagers.Books;
public interface IBookManager
{
    Task<bool> CreateAsync(CreateBookDto dto);
    Task<bool> UpdateAsync(UpdateBookDto dto);
    Task<bool> RecoverBookAsync(int id);
    Task<bool> SoftDeleteAsync(int id);
    Task<bool> HardDeleteAsync(int bookId);
    Task<BookDto?> GetByIdAsync(int id);
    Task<IEnumerable<BookDto>> GetAllAsync();
    Task<IEnumerable<int>> GetAuthorIdsByBookIdAsync(int bookId);
    Task<IEnumerable<int>> GetCategoryIdsByBookIdAsync(int bookId);
    Task<bool> SendBookPdfToEmailAsync(int bookId);
}

