using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts").HasKey(b => b.Id);
        builder.Property(b => b.UserId).IsRequired();
        //builder.Property(b => b.ProductId).IsRequired();
        builder.Property(b => b.Quantity).IsRequired();
        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5);
        });
        builder.Property(p => p.CreateAt).HasColumnName("CreateAt").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
    }
}
    
