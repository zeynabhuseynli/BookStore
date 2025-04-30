using BookStore.Application.Interfaces.IManagers;
using BookStore.Domain.Entities.Enums;
using BookStore.Domain.Entities.Users;
using BookStore.Infrastructure.Utils;
using BookStore.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence.Managers;

public class UserManager : BaseManager<User>, IUserManager
{
    private readonly AppDbContext _context;
    private readonly IEmailManager _emailManager;

    public UserManager(AppDbContext context, IEmailManager emailManager) : base(context)
    {
        _context = context;
        _emailManager = emailManager;
    }

    public bool IsValidEmail(string email)
    {
        return _context.Users.All(u => u.Email != email);
    }

    public async Task RegisterAsync(User user, string password)
    {
        user.SetDetailsForRegister(user.FirstName, user.LastName, user.Email, user.BirthDay, password);
        await AddAsync(user);
        await Commit();

        await _emailManager.SendEmailAsync(user.Email, "Welcome to BookStore!", $"Hi {user.FirstName}, your account has been created.");
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

        if (user is null || user.PasswordHash != PasswordHasher.HashPassword(password))
            return null;

        user.SetForLogin(true);
        await Commit();
        return user;
    }

    public async Task UpdateUserAsync(User user, string firstName, string lastName, string email, Gender gender, DateTime birthDate)
    {
        user.SetDetailsForUpdate(firstName, lastName, email, gender, birthDate);
        await Update(user);
        await Commit();

        await _emailManager.SendEmailAsync(user.Email, "Account Updated", $"Hi {firstName}, your account details have been updated.");
    }

    public async Task SoftDeleteUserAsync(User user)
    {
        user.SetForSoftDelete();
        await Update(user);
        await Commit();

        await _emailManager.SendEmailAsync(user.Email, "Account Deleted", "Your account has been soft deleted.");
    }

    public async Task SendResetPasswordOtpAsync(User user)
    {
        var otp = new Random().Next(100000, 999999);
        user.UpdateOtp(otp);
        await Update(user);
        await Commit();

        await _emailManager.SendOtpAsync(user.Email, otp.ToString());
    }

    public async Task<bool> ResetPasswordAsync(User user, int otpCode, string newPassword)
    {
        if (user.PasswordResetOtp == otpCode && user.PasswordResetOtpDate > DateTime.UtcNow)
        {
            var hashedPassword = PasswordHasher.HashPassword(newPassword);
            user.ResetPassword(hashedPassword);
            await Update(user);
            await Commit();

            await _emailManager.SendEmailAsync(user.Email, "Password Reset", "Your password has been reset successfully.");
            return true;
        }
        return false;
    }
}
