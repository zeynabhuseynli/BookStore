using BookStore.Domain.Common;

namespace BookStore.Domain.Entities.Authors;
public class Author: BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public DateTime? DeathTime { get; set; }
    public DateTime BirthDay { get; set; }
    public ICollection<BookAuthor> Books { get; set; } = new List<BookAuthor>();
}

