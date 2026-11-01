
namespace Maitre.Core.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public bool IsAvailable { get; set; } = true;
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

}
