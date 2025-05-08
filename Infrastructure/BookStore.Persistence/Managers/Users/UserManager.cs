using System.Security.Authentication;
using AutoMapper;
using BookStore.Application.DTOs.UserDtos;
using BookStore.Application.Exceptions;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Application.Interfaces.IManagers.Helper;
using BookStore.Domain.Entities.Enums;
using BookStore.Domain.Entities.Users;
using BookStore.Infrastructure.BaseMessages;
using BookStore.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;

namespace BookStore.Persistence.Managers;
public class UserManager : IUserManager
{
    private readonly IBaseManager<User> _baseManager;
    private readonly IClaimManager _claimManager;
    private readonly IEmailManager _emailManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UserManager(IEmailManager emailManager, IConfiguration configuration, IBaseManager<User> baseManager, IMapper mapper, IClaimManager claimManager)
    {
        _emailManager = emailManager;
        _configuration = configuration;
        _baseManager = baseManager;
        _mapper = mapper;
        _claimManager = claimManager;
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        await _baseManager.ValidateAsync(dto);

        if (!dto.Email.IsEmail())
            throw new ArgumentException(UIMessage.GetFormatMessage("Email"));

        bool isEmailUnique = await _baseManager.IsPropertyUniqueAsync(u => u.Email, dto.Email);
        if (!isEmailUnique)
            throw new BadRequestException(UIMessage.GetUniqueNamedMessage("Email"));

        PasswordHasher.EnsureSecurePasswordOrThrow(dto.Password);

        var user = new User();
        user.SetDetailsForRegister(dto.FirstName, dto.LastName, dto.Email, dto.BirthDay, dto.Password);
        await _baseManager.AddAsync(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<TokenResponseDto?> LoginAsync(LoginDto dto)
    {
        await _baseManager.ValidateAsync(dto);
        var user = await _baseManager.GetAsync(x => x.Email == dto.Email && x.IsActivated);
        if (user == null) return null;

        if (!PasswordHasher.VerifyPassword(user.PasswordHash, dto.Password))
        {
            user.LoginCount += 1;
            if (user.LoginCount == 5)
            {
                await SetUserActivationStatusAsync(user.Id, false);
                throw new AuthenticationException(UIMessage.ACCOUNT_LOCKED_AFTER_FAILED_ATTEMPTS);
            }
            return null;
        }

        user.SetForLogin();
        var jwtSettings = _configuration.GetSection("JWTSettings");
        var secret = jwtSettings["Secret"];
        var expireAt = int.Parse(jwtSettings["ExpireAt"]);

        var tokens = Generator.GenerateTokens(user.Id, user.Email, user.Role.ToString(), secret, expireAt);
        if (tokens == null)
            throw new AuthenticationException(UIMessage.INVALID_MESSAGE);
        user.UpdateRefreshToken(tokens.RefreshToken);
        _baseManager.Update(user);
        await _baseManager.Commit();
        return tokens;
    }

    public async Task SendForgotPasswordOtpAsync(string email)
    {
        var user = await _baseManager.GetAsync(x => x.Email == email && x.IsActivated);
        if (user == null)
            throw new KeyNotFoundException(UIMessage.GetNotFoundMessage($"User for {email}"));

        var otpCode = Generator.GenerateOtpCode();
        user.UpdateOtp(otpCode);
        _baseManager.Update(user);
        await _baseManager.Commit();

        await _emailManager.SendOtpAsync(user.Email, otpCode.ToString());
    }

    public async Task<bool> ResetPasswordWithOtpAsync(ResetPasswordDto dto)
    {
        PasswordHasher.EnsureSecurePasswordOrThrow(dto.NewPassword);

        await _baseManager.ValidateAsync(dto);
        var user = await _baseManager.GetAsync(x => x.Email == dto.Email && x.IsActivated);
       
        if (user == null || user.PasswordResetOtp != dto.OtpCode )
            return false;

        if (DateTime.UtcNow.AddMinutes(-20) > user.PasswordResetOtpDate || user.PasswordResetOtpDate > DateTime.UtcNow)
            throw new InvalidDataException(UIMessage.OTP_INVALID_OR_EXPIRED);

        if (user.PasswordHash == PasswordHasher.HashPassword(dto.NewPassword))
            throw new ArgumentException(UIMessage.PASSWORD_SAME_AS_OLD);

        user.ResetPassword(PasswordHasher.HashPassword(dto.NewPassword));
        _baseManager.Update(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserDto dto)
    {
        await _baseManager.ValidateAsync(dto);
        var user = await _baseManager.GetAsync(x => x.Id == dto.UserId && x.IsActivated);
        if (user == null) return false;
        if (!await _baseManager.IsPropertyUniqueAsync(u => u.Email, dto.Email, dto.UserId))
            throw new BadRequestException(UIMessage.GetUniqueNamedMessage(dto.Email));

        user.SetDetailsForUpdate(dto.FirstName, dto.LastName, dto.Email, dto.Gender, dto.BirthDate);
        user.UpdateRefreshToken(user.RefreshToken);
        _baseManager.Update(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
    {
        PasswordHasher.EnsureSecurePasswordOrThrow(dto.NewPassword);

        await _baseManager.ValidateAsync(dto);
        var user = await _baseManager.GetAsync(x => x.Id == dto.UserId && x.IsActivated);
        if (user == null) return false;

        if (user.PasswordHash == PasswordHasher.HashPassword(dto.NewPassword))
            throw new ArgumentException(UIMessage.PASSWORD_SAME_AS_OLD);

        user.SetPasswordHash(PasswordHasher.HashPassword(dto.NewPassword));
        user.UpdateRefreshToken(user.RefreshToken);
        _baseManager.Update(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> SoftDeleteAsync(int userId)
    {
        var user = await _baseManager.GetAsync(x => x.Id == userId && x.IsActivated);
        if (user == null) return false;

        user.UpdateRefreshToken(null);
        _baseManager.SoftDelete(user);
        _baseManager.Update(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateRoleAsync(UpdateRoleDto dto)
    {
        await _baseManager.ValidateAsync(dto);
        if (dto.UserId == _claimManager.GetCurrentUserId())
            throw new AuthenticationException();
        var user = await _baseManager.GetAsync(x => x.Id == dto.UserId && x.IsActivated);
        if (user == null) return false;
        if (user.Role == Role.SuperAdmin)
            throw new AuthenticationException();

        user.UpdateRole(dto.Role);
        _baseManager.Update(user, _claimManager.GetCurrentUserId());
        await _baseManager.Commit();
        return true;
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _baseManager.GetAsync(x => x.Email == email && x.IsActivated);
        if (user != null)
            return _mapper.Map<UserDto>(user);
        return null;
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var userId = _claimManager.GetCurrentUserId();
        if (userId <= 0)
            return null;

        var user = await _baseManager.GetAsync(x => x.Id == userId);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _baseManager.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<List<UserDto>> GetDeactivatedUsersAsync()
    {
        var users = await _baseManager.GetAllAsync();
        var deactivatedUsers = users.Where(u => !u.IsActivated).ToList();
        return _mapper.Map<List<UserDto>>(deactivatedUsers);
    }

    public async Task<List<UserDto>> GetSoftDeletedUsersAsync()
    {
        var users = await _baseManager.GetAllAsync();
        var softDeletedUsers = users.Where(u => u.IsDeleted).ToList();
        return _mapper.Map<List<UserDto>>(softDeletedUsers);
    }

    public async Task<bool> SetUserActivationStatusAsync(int userId, bool activate)
    {
        var user = await _baseManager.GetAsync(x => x.Id == userId);
        if (user == null)
            return false;
        user.GetActiveOrDeActive(activate);
        _baseManager.Update(user, _claimManager.GetCurrentUserId());
        await _baseManager.Commit();
        return true;
    }

    public async Task CheckPermissionOrThrowAsync(int? ownerUserId)
    {
        var currentUserId = _claimManager.GetCurrentUserId();
        if (currentUserId <= 0)
            throw new AuthenticationException();

        await CheckPermissionAsync(currentUserId, ownerUserId);
    }

    private async Task CheckPermissionAsync(int currentUserId, int? userId)
    {
        var currentUser = await _baseManager.GetAsync(x => x.Id == currentUserId && x.IsActivated);
        if (currentUserId != userId &&
            currentUser.Role != Role.Admin &&
            currentUser.Role != Role.SuperAdmin)
        {
            throw new AuthenticationException(UIMessage.NO_PERMISSION);
        }
    }

    public async Task<IEnumerable<User>> GetAllSubscribers()
    {
        var users = await _baseManager.GetAllAsync(x => x.IsActivated && x.Role != Role.Admin && x.Role != Role.SuperAdmin);
        return users;
    }
}

