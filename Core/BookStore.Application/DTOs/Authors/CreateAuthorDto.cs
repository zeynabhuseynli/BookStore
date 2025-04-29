namespace BookStore.Application.DTOs.Authors;
public class CreateAuthorDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public DateTime BirthDay { get; set; }
    public DateTime? DeathTime { get; set; }
}
