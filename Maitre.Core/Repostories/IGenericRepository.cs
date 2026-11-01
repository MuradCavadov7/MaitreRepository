using System.Linq.Expressions;

namespace Maitre.Core.Repostories;

public interface IGenericRepository<T> where T : BaseEntity, new()
{
    IQueryable<T> Query(bool asNoTracking = true);

    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<T?> GetAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = false,
        CancellationToken ct = default);

    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true,
        CancellationToken ct = default);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);

    Task AddAsync(T entity, CancellationToken ct = default);

    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);

    void Update(T entity);

    void Remove(T entity);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
