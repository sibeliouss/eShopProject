using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses").HasKey(a => a.Id);
        builder.Property(a => a.Id).IsRequired();
        builder.Property(a => a.CreateAt).HasColumnName("CreateShoppingBasket Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        builder.Property(a => a.UpdateAt).HasColumnName("Update Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();

    }
}