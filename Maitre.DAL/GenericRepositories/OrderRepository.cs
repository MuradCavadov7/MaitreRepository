using Maitre.Core.Entities;
using Maitre.Core.Enums;
using Maitre.Core.Repostories;
using Maitre.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.DAL.GenericRepositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(MaitreDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetByIdWithItemsAsync(int restaurantId, int orderId, CancellationToken ct = default)
            => await _dbSet
                .Include(o => o.OrderItems) 
                .FirstOrDefaultAsync(o => o.Id == orderId && o.RestaurantId == restaurantId, ct);

    public async Task<(List<Order> Items, int TotalCount)> GetPagedByRestaurantAsync(int restaurantId, OrderStatus? status, int page, int pageSize, CancellationToken ct = default)
    {

        var query = _dbSet.AsNoTracking().Where(o => o.RestaurantId == restaurantId);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.CreatedTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }
}
