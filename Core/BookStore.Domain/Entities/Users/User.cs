using BookStore.Domain.Common;
using BookStore.Domain.Entities.Enums;
using BookStore.Domain.Entities.Reviews;
using BookStore.Infrastructure.Utils;

namespace BookStore.Domain.Entities.Users;
public class User : BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public Gender Gender { get; private set; }
    public Role Role { get; private set; }
    public DateTime BirthDay { get; private set; }
    public string PasswordHash { get; private set; }
    public int LoginCount { get; set; }
    public bool IsActivated { get; private set; }
    public DateTime? ResetPasswordDate { get; private set; }
    public string? RefreshToken { get; private set; }

    public int? PasswordResetOtp { get; private set; }
    public DateTime? PasswordResetOtpDate { get; private set; }

    public List<Review> Reviews { get; private set; } = new List<Review>();

    public void SetDetailsForRegister(string firstName, string lastName, string email,DateTime birthDay, string password, Gender gender= Gender.Other)
    {
        FirstName = firstName.Capitalize();
        LastName = lastName.Capitalize();
        Email = email;
        IsActivated = true;
        IsDeleted = false;
        PasswordHash = PasswordHasher.HashPassword(password);
        BirthDay = birthDay;
        Role = Role.SuperAdmin;
        Gender = gender;
    }
    public void SetForLogin()
    {
        LoginCount = 0;
        IsDeleted = false;
        DeletedAt = null;
    }

    public void SetForSoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        UpdateRefreshToken(null);
    }
    public void SetDetailsForUpdate(string firstName, string lastName, string email, Gender gender, DateTime dateTime)
    {
        FirstName = firstName.Capitalize();
        LastName = lastName.Capitalize();
        Gender = gender;
        BirthDay = dateTime;
        Email = email;
        IsDeleted = false;
    }
    public void SetPasswordHash(string newPasswordHash)
    {
        if (PasswordHash != newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            ResetPasswordDate = DateTime.UtcNow.AddHours(4);
        }
    }

    //ForgotPassword üçün:
    public void UpdateOtp(int? otpCode)
    {
        PasswordResetOtp = otpCode;
        PasswordResetOtpDate = DateTime.UtcNow.AddHours(4);
    }
    public void ResetPassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        ResetPasswordDate = DateTime.UtcNow.AddHours(4);
        PasswordResetOtpDate = null;
        PasswordResetOtp = null;
    }

    public void GetActiveOrDeActive(bool isActiveOrDeActived)
    {
        IsActivated = isActiveOrDeActived;
    }

    public void UpdateRole(Role role)
    {
        Role = role;
    }

    public void UpdateRefreshToken(string? refreshToken)
    {
        RefreshToken = refreshToken;
    }

}



