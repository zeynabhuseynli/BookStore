using System.Linq.Expressions;
using BookStore.Application.Interfaces.IManagers;
using BookStore.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using BookStore.Infrastructure.BaseMessages;
using BookStore.Domain.Common;

namespace BookStore.Persistence.Managers;
public class BaseManager<T> : IBaseManager<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public BaseManager(AppDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async Task AddAsync(T entity, int? currentUserId)
    {
        entity.CreatedAt = DateTime.UtcNow;
        if (currentUserId != null)
        {
            entity.CreatedById = currentUserId;
        }
        await _context.Set<T>().AddAsync(entity);
    }

    public void HardDelete<TEntity>(TEntity entity) where TEntity : class
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public bool HardRemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
        return true;
    }

    public void SoftDelete<TEntity>(TEntity entity, int? currentUserId = null) where TEntity : BaseEntity
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedById = currentUserId;
    }

    public bool SoftRemoveRange<TEntity>(IEnumerable<TEntity> entities, int currentUserId) where TEntity : BaseEntity
    {
        foreach (var entity in entities)
        {
            SoftDelete(entity, currentUserId);
            Update(entity);
        }
        return true;
    }

    public void Recover<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedById = null;
    }

    public bool RecoveryRange<TEntity>(IEnumerable<TEntity> entities, int currentUserId) where TEntity : BaseEntity
    {
        foreach (var entity in entities)
        {
            Recover(entity);
            Update(entity, currentUserId);
        }
        return true;
    }

    public void Update<TEntity>(TEntity entity, int? currentUserId = null) where TEntity : BaseEntity
    {
        if (currentUserId != null)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedById = currentUserId;
        }
        _context.Set<TEntity>().Update(entity);
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

    public async Task ValidateAsync<TEntity>(TEntity dto) where TEntity : class
    {
        var validator = _serviceProvider.GetService<IValidator<TEntity>>();
        if (validator is null)
            throw new InvalidOperationException(UIMessage.GetNotFoundMessage($"Validator for {typeof(TEntity).Name}"));

        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }

    public async Task<bool> IsPropertyUniqueAsync<TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value, int id = 0)
    {
        var dbSet = _context.Set<T>();

        var predicate = PredicateBuilder.New<T>(true);

        predicate = predicate.And(Expression.Lambda<Func<T, bool>>(
            Expression.Equal(propertySelector.Body, Expression.Constant(value)),
            propertySelector.Parameters));

        if (id != 0)
        {
            predicate = predicate.And(e => EF.Property<int>(e, "Id") != id);
        }

        return !await dbSet.Where(predicate).AnyAsync();
    }

    public async Task SyncManyToMany<TJoinEntity>(
    IEnumerable<TJoinEntity> existingEntities,
    IEnumerable<TJoinEntity> newEntities)
    where TJoinEntity : class
    {
        var toRemove = existingEntities
            .Except(newEntities)
            .ToList();

        var toAdd = newEntities
            .Except(existingEntities)
            .ToList();

        if (toRemove.Any())
            _context.Set<TJoinEntity>().RemoveRange(toRemove);

        if (toAdd.Any())
            await _context.Set<TJoinEntity>().AddRangeAsync(toAdd);

        await _context.SaveChangesAsync();
    }
}

