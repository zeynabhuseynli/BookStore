using BookStore.Domain.Entities.Categories;

namespace BookStore.Application.Interfaces.IManagers;
public interface ICategoryManager : IBaseManager<Category>
{
    Task<bool> IsCategoryNameUniqueAsync(string name);
}

