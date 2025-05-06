using System.Linq.Expressions;
using FluentValidation;

namespace BookStore.Application.Interfaces.IManagers;
public interface IBaseManager<T> where T : class
{
    Task Commit();
    Task AddAsync(T entity);
    Task HardDelete(T entity);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
    Task RemoveRange(IEnumerable<T> entities);
    Task Update(T entity);
    Task ValidateAsync<TEntity>(TEntity dto) where TEntity : class;
    Task<bool> IsPropertyUniqueAsync<TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value, int id = 0);
    Task SyncManyToMany<TJoinEntity>(IEnumerable<TJoinEntity> existingEntities,IEnumerable<TJoinEntity> newEntities) where TJoinEntity : class;
}

