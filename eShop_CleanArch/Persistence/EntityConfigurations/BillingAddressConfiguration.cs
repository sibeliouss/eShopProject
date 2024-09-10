using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class BillingAddressConfiguration : IEntityTypeConfiguration<BillingAddress>
{
    public void Configure(EntityTypeBuilder<BillingAddress> builder)
    {
        builder.ToTable("BillingAddresses").HasKey(a => a.Id);
        builder.Property(ba => ba.Id).IsRequired();
        builder.Property(ba => ba.CreateAt).HasColumnName("Create Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        builder.Property(ba => ba.UpdateAt).HasColumnName("Update Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();

    }
}