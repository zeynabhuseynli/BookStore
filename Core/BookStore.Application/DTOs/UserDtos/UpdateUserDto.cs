using BookStore.Domain.Entities.Enums;

namespace BookStore.Application.DTOs.UserDtos;
public class UpdateUserDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
}

