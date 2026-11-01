using Maitre.Core.Entities;

namespace Maitre.Core.Repostories;

public interface IOrderItemRepository : IGenericRepository<OrderItem>
{
    Task<List<(int ProductId, string ProductName, int TotalQuantity)>> GetTopProductsAsync(
        int restaurantId, int take, CancellationToken ct = default);
}
