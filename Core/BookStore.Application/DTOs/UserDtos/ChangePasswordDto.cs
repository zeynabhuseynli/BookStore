namespace BookStore.Application.DTOs.UserDtos;
public class ChangePasswordDto
{
    public int UserId { get; set; }
    public string NewPassword { get; set; }
}

