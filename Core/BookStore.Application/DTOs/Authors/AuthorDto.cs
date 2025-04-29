namespace BookStore.Application.DTOs.Authors;
public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public DateTime BirthDay { get; set; }
    public DateTime? DeathTime { get; set; }
}

