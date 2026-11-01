using Maitre.Core.Entities;
using Maitre.Core.Enums;
using Maitre.Core.Repostories;
using Maitre.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.DAL.GenericRepositories;

public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(MaitreDbContext context) : base(context)
    {
    }
    public async Task<List<(int ProductId, string ProductName, int TotalQuantity)>> GetTopProductsAsync(
        int restaurantId, int take, CancellationToken ct = default)
    {
        var rows = await _dbSet
            .AsNoTracking()
            .Where(i => i.Order.RestaurantId == restaurantId
                     && i.Order.Status != OrderStatus.Cancelled)
            .GroupBy(i => new { i.ProductId, i.Product.Name })
            .Select(g => new
            {
                g.Key.ProductId,
                ProductName = g.Key.Name,
                TotalQuantity = g.Sum(i => i.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .ThenBy(x => x.ProductName)
            .Take(take)
            .ToListAsync(ct);

        return rows.Select(r => (r.ProductId, r.ProductName, r.TotalQuantity)).ToList();
    }
}
