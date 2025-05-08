namespace BookStore.Application.DTOs.AuthorDtos;
public class AuthorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public DateTime BirthDay { get; set; }
    public DateTime? DeathTime { get; set; }
    public int? BookCount { get; set; }

    public DateTime CreatedAt { get; set; }
    public int? CreatedById { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }

    public DateTime? DeletedAt { get; set; }
    public int? DeletedById { get; set; }

    public bool IsDeleted { get; set; } 
}

