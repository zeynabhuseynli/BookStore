namespace BookStore.Application.DTOs.UserDtos;
public class ResetPasswordDto
{
    public string Email { get; set; } 
    public int OtpCode { get; set; }
    public string NewPassword { get; set; }
}

