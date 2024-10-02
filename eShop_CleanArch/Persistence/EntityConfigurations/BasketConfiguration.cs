using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.ToTable("Baskets").HasKey(b => b.Id);
        builder.Property(b => b.CustomerId).IsRequired();
        builder.Property(b => b.ProductId).IsRequired();
        builder.Property(b => b.Quantity).IsRequired();
        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5);
        });
    }
}
    
