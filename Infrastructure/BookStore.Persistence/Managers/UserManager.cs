using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.Application.DTOs.UserDtos;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Domain.Entities.Users;
using BookStore.Infrastructure.Utils;
using BookStore.Persistence.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Persistence.Managers;

public class UserManager :BaseManager<User>,IUserManager
{
    private readonly IBaseManager<User> _baseManager;
    private readonly IEmailManager _emailManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;


    public UserManager(AppDbContext context,IEmailManager emailManager, IConfiguration configuration, IBaseManager<User> baseManager):base(context)
    {
        _emailManager = emailManager;
        _configuration = configuration;
        _baseManager = baseManager;
        _dbContext = context;
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        if (!dto.Email.IsEmail())
            throw new ArgumentException("Email formatı düzgün deyil.");

        bool isEmailUnique = await _baseManager.IsPropertyUniqueAsync(u => u.Email, dto.Email);
        if (!isEmailUnique)
            throw new InvalidOperationException("Bu email artıq istifadə olunub.");



        var user = new User();
        user.SetDetailsForRegister(dto.FirstName, dto.LastName, dto.Email, dto.BirthDay, dto.Password);
        await _baseManager.AddAsync(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<TokenResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _baseManager.GetAsync(x => x.Email == dto.Email && x.IsActivated);
        if (user == null) return null;

        var hashedPassword = PasswordHasher.HashPassword(dto.Password);
        if (user.PasswordHash != hashedPassword)
        {
            user.LoginCount += 1;
            return null;
        }

        user.SetForLogin();
        var tokens = GenerateTokens(user);

        user.UpdateRefreshToken(tokens.RefreshToken);
        await _baseManager.Update(user);
        await _baseManager.Commit();
        return tokens;
    }

    public async Task SendForgotPasswordOtpAsync(string email)
    {
        var user = await _baseManager.GetAsync(x => x.Email == email && x.IsActivated);
        if (user == null) return;

        var otpCode = new Random().Next(100000, 999999);
        user.UpdateOtp(otpCode);
        await _baseManager.Update(user);
        await _baseManager.Commit();

        await _emailManager.SendOtpAsync(user.Email, otpCode.ToString());
    }

    public async Task<bool> ResetPasswordWithOtpAsync(ResetPasswordDto dto)
    {
        var user = await _baseManager.GetAsync(x => x.Email == dto.Email && x.IsActivated);
        if (user == null || user.PasswordResetOtp != dto.OtpCode || user.PasswordResetOtpDate < DateTime.UtcNow)
            return false;

        user.ResetPassword(PasswordHasher.HashPassword(dto.NewPassword));
        await _baseManager.Update(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserDto dto)
    {
        var user = await _baseManager.GetAsync(x => x.Id == dto.UserId && x.IsActivated);
        if (user == null) return false;

        user.SetDetailsForUpdate(dto.FirstName, dto.LastName, dto.Email, dto.Gender, dto.BirthDate);
        user.UpdateRefreshToken(user.RefreshToken);
        await _baseManager.Update(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await _baseManager.GetAsync(x => x.Id == dto.UserId && x.IsActivated);
        if (user == null) return false;

        user.SetPasswordHash(PasswordHasher.HashPassword(dto.NewPassword));
        user.UpdateRefreshToken(user.RefreshToken);
        await _baseManager.Update(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> SoftDeleteAsync(int userId)
    {
        var user = await _baseManager.GetAsync(x => x.Id == userId && x.IsActivated);
        if (user == null) return false;

        user.SetForSoftDelete();
        await _baseManager.Update(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<bool> UpdateRoleAsync(UpdateRoleDto dto)
    {
        var user = await _baseManager.GetAsync(x => x.Id == dto.UserId && x.IsActivated);
        if (user == null) return false;

        user.UpdateRole(dto.Role);
        await _baseManager.Update(user);
        await _baseManager.Commit();
        return true;
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _baseManager.GetAsync(x => x.Email == email && x.IsActivated);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender,
            BirthDate = user.BirthDay,
            Role = user.Role
        };
    }

    private TokenResponseDto GenerateTokens(User user)
    {
        var jwtSettings = _configuration.GetSection("JWTSettings");
        var secret = jwtSettings["Secret"];
        var expireAt = int.Parse(jwtSettings["ExpireAt"]); // minute type

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

        var accessToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(expireAt),
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
