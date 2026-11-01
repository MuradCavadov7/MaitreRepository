using Maitre.Core.Entities;
using Maitre.Core.Enums;

namespace Maitre.Core.Repostories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order?> GetByIdWithItemsAsync(int restaurantId, int orderId, CancellationToken ct = default);
    Task<(List<Order> Items, int TotalCount)> GetPagedByRestaurantAsync(
        int restaurantId, OrderStatus? status, int page, int pageSize, CancellationToken ct = default);
}
