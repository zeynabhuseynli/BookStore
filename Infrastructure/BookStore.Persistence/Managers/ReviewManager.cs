using BookStore.Application.Interfaces.IManagers;
using BookStore.Domain.Entities.Reviews;
using BookStore.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence.Managers;
public class ReviewManager : BaseManager<Review>, IReviewManager
{
    public ReviewManager(AppDbContext context) : base(context) { }

    public async Task<List<Review>> GetReviewsByBookIdAsync(int bookId)
    {
        return await _dbSet
            .Include(r => r.User)
            .Include(r => r.Replies)
            .Where(r => r.BookId == bookId && r.ParentRewievId == null && !r.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<Review>> GetRepliesByReviewIdAsync(int reviewId)
    {
        return await _dbSet
            .Where(r => r.ParentRewievId == reviewId && !r.IsDeleted)
            .ToListAsync();
    }
}

