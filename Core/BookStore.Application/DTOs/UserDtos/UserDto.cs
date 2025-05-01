using BookStore.Domain.Entities.Enums;

namespace BookStore.Application.DTOs.UserDtos;
public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public Role Role { get; set; }
}

