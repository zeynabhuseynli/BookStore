using BookStore.Domain.Entities.Authors;

namespace BookStore.Application.Interfaces.IManagers;
public interface IAuthorManager : IBaseManager<Author>
{
    Task<List<Author>> GetByNameAsync(string name);
}

