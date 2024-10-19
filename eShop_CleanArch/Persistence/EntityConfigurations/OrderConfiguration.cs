using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders").HasKey(o => o.Id);
        builder.Property(o => o.OrderNumber).IsRequired();
        builder.Property(o => o.PaymentMethod).IsRequired();
        builder.Property(o => o.PaymentNumber).IsRequired();
        builder.Property(o => o.PaymentDate).HasDefaultValueSql("GETDATE()");
        builder.Property(o => o.Status).IsRequired();
        builder.Property(o => o.PaymentCurrency).IsRequired();
    }
    
}