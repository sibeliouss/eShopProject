using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ProductDetailConfiguration :IEntityTypeConfiguration<ProductDetail>
{
    public void Configure(EntityTypeBuilder<ProductDetail> builder)
    {
        builder.ToTable("ProductDetails").HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.ProductId).HasColumnName("ProductId");
        builder.Property(p => p.Description).HasMaxLength(500).IsUnicode(true);
        builder.Property(p => p.Stock).IsRequired().HasDefaultValue(0);
        builder.Property(p => p.Barcode).HasMaxLength(50);
        builder.Property(p => p.Material).HasMaxLength(100);
        builder.Property(p => p.Size).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Color).HasMaxLength(50);
        builder.Property(p => p.Fit).HasMaxLength(10);
        builder.Property(pc => pc.CreateAt).HasColumnName("Create Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        builder.Property(pc => pc.UpdateAt).HasColumnName("Update Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();

        
    }
}