using System.Linq.Expressions;
using BookStore.Domain.Common;

namespace BookStore.Application.Interfaces.IManagers;
public interface IBaseManager<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task SoftDeleteAsync(int id);

    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
}

