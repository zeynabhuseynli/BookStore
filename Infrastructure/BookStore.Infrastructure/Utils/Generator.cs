using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookStore.Infrastructure.Utils;
public static class Generator
{
    public static int GenerateOtpCode()
    {
        var bytes = new byte[4];
        RandomNumberGenerator.Fill(bytes);
        int generated = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF;
        return (generated % 900000) + 100000;
    }

    public static TokenResponseDto GenerateTokens(int userId,string userEmail,string userRole, string secret, int expireMinutes)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, userEmail),
            new Claim(ClaimTypes.Role, userRole)
        };

        var accessToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds,
            claims: claims
        );

        return new TokenResponseDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
        };
    }
}
public class TokenResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}

