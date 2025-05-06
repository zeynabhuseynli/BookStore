using System.Security.Authentication;
using AutoMapper;
using BookStore.Application.DTOs.ReviewDtos;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Helper;
using BookStore.Domain.Entities.Reviews;
using BookStore.Infrastructure.BaseMessages;
using BookStore.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence.Managers;
public class ReviewManager : IReviewManager
{
    private readonly IBaseManager<Review> _baseManager;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly IClaimManager _claimManager;

    public ReviewManager(IBaseManager<Review> baseManager, IMapper mapper, AppDbContext context, IClaimManager claimManager)
    {
        _baseManager = baseManager;
        _mapper = mapper;
        _context = context;
        _claimManager = claimManager;
    }

    public async Task<bool> CreateAsync(CreateReviewDto dto)
    {
        var review = _mapper.Map<Review>(dto);
        review.CreatedAt = DateTime.UtcNow;
        review.FromUserId =_claimManager.GetCurrentUserId();

        await _baseManager.AddAsync(review);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateReviewDto dto)
    {
        var review = await _baseManager.GetAsync(x => x.Id == dto.Id && !x.IsDeleted);
        if (review == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Review"));
        if (review.FromUserId!=_claimManager.GetCurrentUserId())
            throw new AuthenticationException("This is not your message.");
        review.Message = dto.Message;
        review.Rating = dto.Rating;
        review.UpdatedAt = DateTime.UtcNow;

        await _baseManager.Update(review);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var review = await _baseManager.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (review == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Review"));

        review.IsDeleted = true;
        review.DeletedAt = DateTime.UtcNow;

        await _baseManager.Update(review);
        await _baseManager.Commit();
        return true;
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsForBookAsync(int bookId)
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Replies.Where(r => !r.IsDeleted))
                .ThenInclude(reply => reply.User)
            .Where(r => r.BookId == bookId && r.ParentRewievId == null && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<ReviewDto>>(reviews);
    }

    public async Task<ReviewDto?> GetByIdAsync(int id)
    {
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Replies.Where(r => !r.IsDeleted))
                .ThenInclude(reply => reply.User)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        return review == null ? null : _mapper.Map<ReviewDto>(review);
    }
}

