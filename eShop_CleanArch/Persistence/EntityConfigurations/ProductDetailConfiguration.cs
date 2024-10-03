using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ProductDetailConfiguration :IEntityTypeConfiguration<ProductDetail>
{
    public void Configure(EntityTypeBuilder<ProductDetail> builder)
    {
        builder.ToTable("ProductDetails").HasKey(pd => pd.Id);
        builder.Property(pd => pd.Id).IsRequired();
        builder.Property(pd => pd.ProductId).HasColumnName("ProductId");
        builder.Property(pd => pd.Description).HasMaxLength(500).IsUnicode(true);
        builder.Property(pd => pd.Barcode).HasMaxLength(50);
        builder.Property(pd => pd.Material).HasMaxLength(100);
        builder.Property(pd => pd.Size).IsRequired().HasMaxLength(50);
        builder.Property(pd => pd.Color).HasMaxLength(50);
        builder.Property(pd => pd.Fit).HasMaxLength(10);
        builder.Property(pd => pd.CreateAt).HasDefaultValueSql("GETDATE()");
        builder.Property(pd => pd.UpdateAt).HasDefaultValueSql("GETDATE()");


   
        



    }
}