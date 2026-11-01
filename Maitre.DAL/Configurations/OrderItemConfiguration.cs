
using Maitre.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maitre.DAL.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(x => x.ProductName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.UnitPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();

        //Hesablama property
        builder.Ignore(x => x.LineTotal);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        //Top 5 
        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.OrderId);
    }

}

