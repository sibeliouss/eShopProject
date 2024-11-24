using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class WishListConfiguration : IEntityTypeConfiguration<WishList>
{
    public void Configure(EntityTypeBuilder<WishList> builder)
    {
        builder.HasKey(w => w.Id);
        
        builder.ToTable("WishLists");
        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5);
        });
        builder.Property(p => p.CreateAt).HasDefaultValueSql("GETDATE()");
    }
}