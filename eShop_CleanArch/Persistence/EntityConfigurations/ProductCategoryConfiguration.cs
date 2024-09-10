using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories").HasKey(pc => pc.Id);
        builder.Property(pc => pc.Id).IsRequired();
        builder.Property(pc => pc.CategoryId).HasColumnName("CategoryId");
        builder.Property(pc => pc.ProductId).HasColumnName("ProductId");
        builder.Property(pc => pc.CreateAt).HasColumnName("Create Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        builder.Property(pc => pc.UpdateAt).HasColumnName("Update Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();
        
        builder.HasQueryFilter(cb => !cb.DeletedAt.HasValue);
        
        //composite key, çoka çok ilişki
        builder.HasKey(pc => new { pc.ProductId, pc.CategoryId });


    }
}