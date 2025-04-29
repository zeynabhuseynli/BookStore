using BookStore.Domain.Entities.Books;

namespace BookStore.Application.Interfaces.IManagers;
public interface IBookManager : IBaseManager<Book>
{
    Task<List<Book>> GetBooksByAuthorIdAsync(int authorId);
    Task<List<Book>> GetBooksByCategoryIdAsync(int categoryId);
    Task<Book?> GetBookWithAuthorsAndCategoriesAsync(int bookId);
}

