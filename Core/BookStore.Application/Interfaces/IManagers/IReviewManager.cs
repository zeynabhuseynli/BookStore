using BookStore.Application.DTOs.ReviewDtos;

namespace BookStore.Application.Interfaces.IManagers;
public interface IReviewManager
{
    Task<bool> CreateAsync(CreateReviewDto dto);
    Task<bool> UpdateAsync(UpdateReviewDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ReviewDto>> GetReviewsForBookAsync(int bookId);
    Task<ReviewDto?> GetByIdAsync(int id);
}

