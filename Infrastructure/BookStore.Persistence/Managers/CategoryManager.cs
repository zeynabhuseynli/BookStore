using BookStore.Application.Interfaces.IManagers;
using BookStore.Domain.Entities.Categories;
using BookStore.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence.Managers;
public class CategoryManager : BaseManager<Category>, ICategoryManager
{
    public CategoryManager(AppDbContext context) : base(context) { }

    public async Task<bool> IsCategoryNameUniqueAsync(string name)
    {
        return !await _dbSet.AnyAsync(c => c.Name.ToLower() == name.ToLower() && !c.IsDeleted);
    }
}

