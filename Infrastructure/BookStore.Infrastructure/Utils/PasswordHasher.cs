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
}

