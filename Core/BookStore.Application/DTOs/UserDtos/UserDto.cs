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
    public bool IsActivated { get; set; }

    public int LoginCount { get; set; }

    public DateTime CreatedAt { get; set; }
    public int? CreatedById { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }

    public DateTime? DeletedAt { get; set; }
    public int? DeletedById { get; set; }

    public bool IsDeleted { get; set; }
}

