using Maitre.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maitre.DAL.Contexts;

public class MaitreDbContext : DbContext
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public MaitreDbContext(DbContextOptions<MaitreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MaitreDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
