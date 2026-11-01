using Maitre.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maitre.DAL.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

        builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");

        builder.Property(p => p.Category).IsRequired().HasMaxLength(50);

        builder.HasMany(p => p.OrderItems)
               .WithOne(oi => oi.Product)
               .HasForeignKey(oi => oi.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
