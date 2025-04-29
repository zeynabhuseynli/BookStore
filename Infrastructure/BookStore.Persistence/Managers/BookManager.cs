using System;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Domain.Entities.Books;
using BookStore.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence.Managers;
public class BookManager : BaseManager<Book>, IBookManager
{
    public BookManager(AppDbContext context) : base(context) { }

    public async Task<List<Book>> GetBooksByAuthorIdAsync(int authorId)
    {
        return await _dbSet
            .Include(b => b.Authors)
                .ThenInclude(ba => ba.Author)
            .Where(b => b.Authors.Any(ba => ba.AuthorId == authorId) && !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<Book>> GetBooksByCategoryIdAsync(int categoryId)
    {
        return await _dbSet
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .Where(b => b.BookCategories.Any(bc => bc.CategoryId == categoryId) && !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<Book?> GetBookWithAuthorsAndCategoriesAsync(int bookId)
    {
        return await _dbSet
            .Include(b => b.Authors)
                .ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.Id == bookId && !b.IsDeleted);
    }
}


