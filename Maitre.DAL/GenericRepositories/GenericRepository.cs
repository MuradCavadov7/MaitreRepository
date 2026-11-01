using Maitre.Core.Entities.Common;
using Maitre.Core.Repostories;
using Maitre.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Maitre.DAL.GenericRepositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
{
    protected readonly MaitreDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(MaitreDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public IQueryable<T> Query(bool asNoTracking = true)
        => asNoTracking ? _dbSet.AsNoTracking() : _dbSet;

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _dbSet.FindAsync(new object[] { id }, ct);

    public async Task<T?> GetAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = false,
        CancellationToken ct = default)
    {
        var query = Query(asNoTracking);

        if (include is not null)
            query = include(query);

        return await query.FirstOrDefaultAsync(predicate, ct);
    }

    public async Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        bool asNoTracking = true,
        CancellationToken ct = default)
    {
        var query = Query(asNoTracking);

        if (include is not null)
            query = include(query);

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync(ct);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.AnyAsync(predicate, ct);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        => predicate is null
            ? await _dbSet.CountAsync(ct)
            : await _dbSet.CountAsync(predicate, ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _dbSet.AddAsync(entity, ct);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        => await _dbSet.AddRangeAsync(entities, ct);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Remove(T entity)
        => _dbSet.Remove(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
