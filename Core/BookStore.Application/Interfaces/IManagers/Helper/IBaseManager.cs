using System.Linq.Expressions;
using BookStore.Domain.Common;

namespace BookStore.Application.Interfaces.IManagers;
public interface IBaseManager<T> where T : BaseEntity
{
    Task Commit();
    Task AddAsync(T entity, int? currentUserId = null);
    void Update<TEntity>(TEntity entity, int? currentUserId = null) where TEntity : BaseEntity;
    void HardDelete<TEntity>(TEntity entity) where TEntity : BaseEntity;
    bool HardRemoveRange(IEnumerable<T> entities);
    void SoftDelete<TEntity>(TEntity entity, int? currentUserId = null) where TEntity : BaseEntity;
    bool SoftRemoveRange<TEntity>(IEnumerable<TEntity> entities, int currentUserId) where TEntity : BaseEntity;
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
    Task ValidateAsync<TEntity>(TEntity dto) where TEntity : class;
    Task<bool> IsPropertyUniqueAsync<TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value, int id = 0);
    Task SyncManyToMany<TJoinEntity>(IEnumerable<TJoinEntity> existingEntities, IEnumerable<TJoinEntity> newEntities) where TJoinEntity : class;
}

