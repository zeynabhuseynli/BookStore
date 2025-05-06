using System.Security.Authentication;
using AutoMapper;
using BookStore.Application.DTOs.ReviewDtos;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Helper;
using BookStore.Domain.Entities.Enums;
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
    private readonly IUserManager _userManager;

    public ReviewManager(IBaseManager<Review> baseManager, IMapper mapper, AppDbContext context, IClaimManager claimManager, IUserManager userManager)
    {
        _baseManager = baseManager;
        _mapper = mapper;
        _context = context;
        _claimManager = claimManager;
        _userManager = userManager;
    }

    public async Task<bool> CreateAsync(CreateReviewDto dto)
    {
        var review = _mapper.Map<Review>(dto);
        review.CreatedAt = DateTime.UtcNow;
        review.FromUserId = _claimManager.GetCurrentUserId();

        await _baseManager.AddAsync(review);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateReviewDto dto)
    {
        var review = await _baseManager.GetAsync(x => x.Id == dto.Id && !x.IsDeleted);
        if (review == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Review"));
        if (review.FromUserId != _claimManager.GetCurrentUserId())
            throw new AuthenticationException("This is not your message.");
        review.Message = dto.Message;
        review.Rating = dto.Rating;
        review.UpdatedAt = DateTime.UtcNow;

        await _baseManager.Update(review);
        await _baseManager.Commit();
        return true;
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

    public async Task<List<ReviewDto>> GetRepliesByReviewIdAsync(int reviewId)
    {
        var replies = await _baseManager.GetAllAsync(r => r.ParentRewievId == reviewId && !r.IsDeleted, nameof(Review.User));
        return _mapper.Map<List<ReviewDto>>(replies.OrderByDescending(r => r.CreatedAt).ToList());
    }

    public async Task<List<ReviewDto>> GetAllReviewsWithRepliesForBookAsync(int bookId)
    {
        var reviews = await _baseManager.GetAllAsync(
            r => r.BookId == bookId && r.ParentRewievId == null && !r.IsDeleted,
            nameof(Review.User),
            nameof(Review.Replies),
            "Replies.User"
        );
        return _mapper.Map<List<ReviewDto>>(reviews.OrderByDescending(r => r.CreatedAt).ToList());
    }

    public async Task<List<ReviewDto>> GetUserReviewsAndRepliesAsync(int? userId)
    {
        if (userId != null)
            await _userManager.CheckPermissionOrThrowAsync(userId);

        var currentUserId = _claimManager.GetCurrentUserId();
        if (currentUserId <= 0)
            throw new AuthenticationException("User ID təyin edilməyib və istifadəçi autentifikasiya olunmayıb.");
        userId = currentUserId;

        var reviews = await _baseManager.GetAllAsync(
            r => r.FromUserId == userId && !r.IsDeleted,
            nameof(Review.User),
            nameof(Review.Replies),
            "Replies.User"
        );

        return _mapper.Map<List<ReviewDto>>(reviews.OrderByDescending(r => r.CreatedAt).ToList());
    }

    public async Task<List<ReviewDto>> GetRepliesToUserAsync(int? userId)
    {
        if (userId != null)
            await _userManager.CheckPermissionOrThrowAsync(userId);

        var currentUserId = _claimManager.GetCurrentUserId();
        if (currentUserId <= 0)
            throw new AuthenticationException("User ID təyin edilməyib və istifadəçi autentifikasiya olunmayıb.");
        userId = currentUserId;

        var replies = await _baseManager.GetAllAsync(
            r => r.ToUserId == userId && !r.IsDeleted,
            nameof(Review.User)
        );

        return _mapper.Map<List<ReviewDto>>(replies.OrderByDescending(r => r.CreatedAt).ToList());
    }

    public async Task<bool> DeleteReviewWithRepliesAsync(int reviewId, bool isdeleted=true)
    {
        var review = await _baseManager.GetAsync(
            r => r.Id == reviewId,
            nameof(Review.Replies)
        );

        if (review == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage("Review"));


        await _userManager.CheckPermissionOrThrowAsync(review.FromUserId);
        review.IsDeleted = isdeleted;
        review.DeletedAt = DateTime.UtcNow;

        foreach (var reply in review.Replies.Where(r => !r.IsDeleted))
        {
            reply.IsDeleted = isdeleted;
            reply.DeletedAt = DateTime.UtcNow;
        }

        await _baseManager.Update(review);
        await _baseManager.Commit();
        return true;
    }

    public async Task<List<ReviewDto>> GetDeletedReviewsWithRepliesAsync()
    {
        var currentUser = await _userManager.GetCurrentUserAsync();

        if (currentUser == null ||
            (currentUser.Role != Role.Admin && currentUser.Role != Role.SuperAdmin))
        {
            throw new AuthenticationException("Yalnız admin silinmiş review-lara baxa bilər.");
        }

        var deletedReviews = await _baseManager.GetAllAsync(
            r => r.IsDeleted && r.ParentRewievId == null,
            nameof(Review.User),
            nameof(Review.Replies),
            "Replies.User"
        );

        var filtered = deletedReviews.Select(r =>
        {
            r.Replies = r.Replies.Where(reply => reply.IsDeleted).ToList();
            return r;
        }).OrderByDescending(r => r.DeletedAt).ToList();

        return _mapper.Map<List<ReviewDto>>(filtered);
    }


}

