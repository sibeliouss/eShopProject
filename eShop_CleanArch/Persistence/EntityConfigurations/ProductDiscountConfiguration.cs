using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ProductDiscountConfiguration : IEntityTypeConfiguration<ProductDiscount>
{
    public void Configure(EntityTypeBuilder<ProductDiscount> builder)
    {
        builder.ToTable("ProductDiscounts").HasKey(pd => pd.Id);
        builder.Property(pd => pd.DiscountPercentage).IsRequired().HasDefaultValue(0);
        builder.Property(pd => pd.StartDate).IsRequired();
        builder.Property(pd => pd.EndDate).IsRequired();
        builder.Property(pd => pd.DiscountedPrice).IsRequired().HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);
        builder.HasIndex(pd => new { pd.ProductId, pd.StartDate, pd.EndDate })
            .IsUnique(); // Aynı ürün için aynı dönemde birden fazla indirim olmasın
        builder.Property(pd => pd.CreateAt).HasColumnName("CreateDate").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        builder.Property(pd => pd.UpdateAt).HasColumnName("UpdateDate").HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();

    }
}