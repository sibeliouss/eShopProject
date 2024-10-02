using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories").HasKey(c => c.Id);
        builder.Property(c => c.Id).IsRequired();
        builder.Property(c => c.Name).HasColumnName("CategoryName").HasMaxLength(50);
        builder.Property(c => c.CreateAt).HasColumnName("CreateShoppingBasket Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        builder.Property(c => c.UpdateAt).HasColumnName("Update Date").HasDefaultValueSql("GETDATE()").ValueGeneratedOnUpdate();
    }
}