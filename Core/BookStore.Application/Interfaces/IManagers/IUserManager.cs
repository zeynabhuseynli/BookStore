using BookStore.Application.DTOs.UserDtos;

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
}

