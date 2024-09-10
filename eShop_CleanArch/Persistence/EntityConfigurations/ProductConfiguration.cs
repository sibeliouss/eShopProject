using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ProductConfiguration :IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products").HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.Name).HasColumnName("Product Name").HasMaxLength(30).IsRequired();
        builder.Property(p => p.Brand).HasColumnName("Product Brand").HasMaxLength(30).IsRequired();
        builder.Property(p => p.CreateAt).HasColumnName("Create Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        builder.Property(p => p.UpdateAt).HasColumnName("Update Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();

    }
}