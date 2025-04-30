using System.Linq.Expressions;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using LinqKit;

namespace BookStore.Persistence.Managers;
public class BaseManager<T> : IBaseManager<T> where T : class
{
    private readonly AppDbContext _context;

    public BaseManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await (filter == null ? query.ToListAsync() : query.Where(filter).ToListAsync());
    }
    public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await (filter == null ? query.FirstOrDefaultAsync() : query.FirstOrDefaultAsync(filter));
    }

    public async Task HardDelete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public async Task<bool> IsPropertyUniqueAsync<TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value, int id = 0)
    {
        var dbSet = _context.Set<T>();

        // Dinamik predicate yaradılır
        var predicate = PredicateBuilder.New<T>();

        // Propertinin dəyəri ilə müqayisə edilərək predicate yaradılır
        predicate = predicate.And(Expression.Lambda<Func<T, bool>>(
            Expression.Equal(propertySelector.Body, Expression.Constant(value)),
            propertySelector.Parameters));

        // ID varsa onu istisna edirik
        if (id != 0)
        {
            predicate = predicate.And(e => EF.Property<int>(e, "Id") != id);
        }

        // Predicate tətbiq edilərək yoxlanılır
        return !await dbSet.AsExpandable().AnyAsync(predicate);
    }

}

