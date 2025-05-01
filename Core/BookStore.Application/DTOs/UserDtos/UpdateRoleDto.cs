using BookStore.Domain.Entities.Enums;

namespace BookStore.Application.DTOs.UserDtos;
public class UpdateRoleDto
{
    public int UserId { get; set; }
    public Role Role { get; set; }
}

