
using Maitre.Core.Enums;

namespace Maitre.Core.Entities;

public class Order : BaseEntity
{
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
