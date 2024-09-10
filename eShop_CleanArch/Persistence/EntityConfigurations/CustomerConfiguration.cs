using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers").HasKey(c => c.Id);
        builder.Property(c => c.Id).IsRequired();
        builder.Property(c => c.FirstName).HasColumnName("FirstName").HasMaxLength(20).IsRequired();
        builder.Property(c => c.LastName).HasColumnName("LastName").HasMaxLength(20).IsRequired();
        builder.Property(c => c.UserName).HasColumnName("UserName").HasMaxLength(20).IsRequired();
        builder.Property(c => c.Email).HasColumnName("Email").HasMaxLength(50).IsRequired();
        builder.HasIndex(c => c.UserName).IsUnique();
        builder.HasIndex(c => c.Email).IsUnique();

    }
}