using BookStore.Application.DTOs.UserDtos;
using BookStore.Infrastructure.Utils;

namespace BookStore.Application.Interfaces.IManagers;
public interface IUserManager
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<TokenResponseDto?> LoginAsync(LoginDto dto);
    Task SendForgotPasswordOtpAsync(string email);
    Task<bool> ResetPasswordWithOtpAsync(ResetPasswordDto dto);
    Task<bool> UpdateUserInfoAsync(UpdateUserDto dto);
    Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
    Task<bool> SoftDeleteAsync(int userId);
    Task<bool> UpdateRoleAsync(UpdateRoleDto dto);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<List<UserDto>> GetSoftDeletedUsersAsync();
    Task<UserDto?> GetCurrentUserAsync();
    Task<List<UserDto>> GetAllUsersAsync();
    Task<List<UserDto>> GetDeactivatedUsersAsync();
    Task<bool> SetUserActivationStatusAsync(int userId, bool activate);

    Task CheckPermissionOrThrowAsync(int? ownerUserId);

}

