using Maitre.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maitre.DAL.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);

        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new Restaurant { Id = 1, Name = "KFC", CreatedTime = seedDate },
            new Restaurant { Id = 2, Name = "MC", CreatedTime = seedDate },
            new Restaurant { Id = 3, Name = "PopEyes", CreatedTime = seedDate }
        );

        builder.HasMany(r => r.Products)
               .WithOne(p => p.Restaurant)
               .HasForeignKey(p => p.RestaurantId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Orders)
               .WithOne(o => o.Restaurant)
               .HasForeignKey(o => o.RestaurantId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
