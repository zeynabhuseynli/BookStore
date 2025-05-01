namespace BookStore.Application.DTOs.AuthorDtos;
public class CreateAuthorDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public DateTime BirthDay { get; set; }
    public DateTime? DeathTime { get; set; }
}

