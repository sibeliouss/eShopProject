using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Rating).IsRequired();
        builder.Property(r => r.Title).HasMaxLength(60);
        builder.Property(r => r.Comment).HasMaxLength(1000);
        /*builder.HasOne(r => r.Product).WithMany(p => p.Reviews).HasForeignKey(r => r.ProductId);*/
        
        builder.Property(r => r.CreateAt).HasDefaultValueSql("GETDATE()");

        builder.ToTable("Reviews");
    }
}