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
        builder.Property(p => p.Name).HasColumnName("ProductName").HasMaxLength(30).IsRequired();
        builder.Property(p => p.Brand).HasColumnName("ProductBrand").HasMaxLength(30).IsRequired();
        builder.Property(p => p.CreateAt).HasColumnName("CreateDate").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        builder.Property(p => p.UpdateAt).HasColumnName("UpdateDate").HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();
        
        builder.HasOne(p => p.ProductDetail)
            .WithOne(pd => pd.Product)
            .HasForeignKey<ProductDetail>(pd => pd.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // Ürün silinince detayları da silinsin
        
        //Value Object
        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value).HasColumnType("money");
            price.Property(p => p.Currency).HasMaxLength(5);
        });

    }
}