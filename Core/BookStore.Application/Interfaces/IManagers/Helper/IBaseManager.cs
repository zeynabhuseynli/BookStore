using System.Linq.Expressions;
using System.Threading.Tasks;

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
    Task<bool> IsPropertyUniqueAsync<TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value, int id = 0);
    Task SyncManyToMany<TJoinEntity>(IEnumerable<TJoinEntity> existingEntities,IEnumerable<TJoinEntity> newEntities) where TJoinEntity : class;
}

