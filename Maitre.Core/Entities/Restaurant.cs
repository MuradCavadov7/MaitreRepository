namespace Maitre.Core.Entities
{
    public class Restaurant : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
