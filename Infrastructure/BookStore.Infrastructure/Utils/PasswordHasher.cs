using System.Security.Cryptography;
using System.Text;

namespace BookStore.Infrastructure.Utils;
public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    public static bool VerifyPassword(string userPasswordHash, string password)
    {
        var hashedPassword = HashPassword(password);
        if (userPasswordHash == hashedPassword)
            return true;
        return false;
    }

    private static readonly string[] CommonWeakPasswords = new[]
    {
        "password", "123456", "123456789", "qwerty", "abc123",
        "111111", "123123", "admin", "letmein", "welcome"
    };

    public static void EnsureSecurePasswordOrThrow(string password)
    {
        var errors = GetPasswordProblems(password);
        if (errors.Any())
        {
            var message = string.Join("; ", errors);
            throw new ArgumentException($"Password is insecure: {message}");
        }
    }

    public static List<string> GetPasswordProblems(string password)
    {
        List<string> errors = new();

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("Password cannot be empty.");
            return errors;
        }

        if (password.Length < 8)
            errors.Add("Minimum length is 8 characters.");

        if (!password.Any(char.IsUpper))
            errors.Add("Must contain at least one uppercase letter.");

        if (!password.Any(char.IsLower))
            errors.Add("Must contain at least one lowercase letter.");

        if (!password.Any(char.IsDigit))
            errors.Add("Must contain at least one digit.");

        if (!password.Any(ch => "!@#$%^&*()_+-=[]{}|;:',.<>?/`~".Contains(ch)))
            errors.Add("Must contain at least one special character.");

        if (CommonWeakPasswords.Contains(password.ToLower()))
            errors.Add("Password is too common.");

        return errors;
    }

}

