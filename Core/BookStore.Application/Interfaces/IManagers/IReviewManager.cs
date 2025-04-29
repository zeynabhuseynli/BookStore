using BookStore.Domain.Entities.Reviews;

namespace BookStore.Application.Interfaces.IManagers;
public interface IReviewManager : IBaseManager<Review>
{
    Task<List<Review>> GetReviewsByBookIdAsync(int bookId);
    Task<List<Review>> GetRepliesByReviewIdAsync(int reviewId);
}

