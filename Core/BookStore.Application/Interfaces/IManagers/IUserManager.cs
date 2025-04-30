using BookStore.Domain.Entities.Enums;
using BookStore.Domain.Entities.Users;

namespace BookStore.Application.Interfaces.IManagers;
public interface IUserManager:IBaseManager<User>
{
    Task RegisterAsync(User user, string password);
    Task<User?> LoginAsync(string email, string password);
    Task UpdateUserAsync(User user, string firstName, string lastName, string email, Gender gender, DateTime birthDate);
    Task SoftDeleteUserAsync(User user);
    Task SendResetPasswordOtpAsync(User user);
    Task<bool> ResetPasswordAsync(User user, int otpCode, string newPassword);
    bool IsValidEmail(string email);
}

