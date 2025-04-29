using System.Linq.Expressions;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence.Managers;
public class BaseManager<T> : IBaseManager<T> where T : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseManager(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !(EF.Property<bool>(x, "IsDeleted")));
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.Where(x => !(EF.Property<bool>(x, "IsDeleted"))).ToListAsync();
    }

    public virtual async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task SoftDeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;

        var prop = typeof(T).GetProperty("IsDeleted");
        var deletedTimeProp = typeof(T).GetProperty("DeletedTime");

        if (prop != null) prop.SetValue(entity, true);
        if (deletedTimeProp != null) deletedTimeProp.SetValue(entity, DateTime.UtcNow);

        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
       => await _dbSet.Where(predicate).Where(e => !e.IsDeleted).ToListAsync();

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.CountAsync(predicate);
}

