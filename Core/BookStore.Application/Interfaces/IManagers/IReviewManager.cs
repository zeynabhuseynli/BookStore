using BookStore.Application.DTOs.ReviewDtos;

namespace BookStore.Application.Interfaces.IManagers;
public interface IReviewManager
{
    Task<bool> CreateAsync(CreateReviewDto dto);
    Task<bool> UpdateAsync(UpdateReviewDto dto);
    Task<ReviewDto?> GetByIdAsync(int id);
    Task<List<ReviewDto>> GetRepliesByReviewIdAsync(int reviewId);
    Task<List<ReviewDto>> GetAllReviewsWithRepliesForBookAsync(int bookId);
    Task<List<ReviewDto>> GetUserReviewsAndRepliesAsync(int? userId);
    Task<List<ReviewDto>> GetRepliesToUserAsync(int? userId);
    Task<bool> DeleteReviewWithRepliesAsync(int reviewId, bool isDeleted=true);
    Task<List<ReviewDto>> GetDeletedReviewsWithRepliesAsync();
}

