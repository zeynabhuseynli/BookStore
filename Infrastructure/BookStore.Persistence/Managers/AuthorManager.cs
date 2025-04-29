using BookStore.Application.Interfaces.IManagers;
using BookStore.Domain.Entities.Authors;
using BookStore.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence.Managers;
public class AuthorManager : BaseManager<Author>, IAuthorManager
{
    public AuthorManager(AppDbContext context) : base(context) { }

    public async Task<List<Author>> GetByNameAsync(string name)
    {
        return await _dbSet
            .Where(a => a.FirstName.ToLower().Contains(name.ToLower()) && !a.IsDeleted)
            .ToListAsync();
    }
}

