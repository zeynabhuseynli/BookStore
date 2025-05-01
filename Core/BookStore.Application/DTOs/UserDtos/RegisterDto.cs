using BookStore.Domain.Entities.Enums;

namespace BookStore.Application.DTOs.UserDtos;
public class RegisterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime BirthDay { get; set; }
    public string Password { get; set; }
    public Gender? gender { get; set; } = Gender.Other;
}

