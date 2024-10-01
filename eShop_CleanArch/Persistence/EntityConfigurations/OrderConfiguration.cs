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
        builder.Property(o => o.PaymentDate).HasDefaultValueSql("GETDATE()");

        builder.HasOne(o => o.Customer).WithMany(c => c.Orders).HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade); // Müşteri silinirse ilgili siparişler de silinir
        
        builder.Property(o => o.Status).IsRequired();
        builder.Property(o => o.PaymentCurrency).IsRequired();
    }
    
}