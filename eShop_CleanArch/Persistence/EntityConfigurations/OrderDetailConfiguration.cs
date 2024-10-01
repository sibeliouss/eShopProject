using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{

    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("OrderDetails").HasKey(od => od.Id);
        builder.HasOne(od => od.Order).WithMany(o => o.OrderDetails).HasForeignKey(od => od.OrderId).OnDelete(DeleteBehavior.Cascade);  // Sipariş silindiğinde ilgili detaylar da silinir
        //composite key
        builder.HasKey(od => new { od.ProductId, od.OrderId });
        //bunu kontrol et
        builder.Property(od => od.Quantity).IsRequired().HasDefaultValue(1).HasColumnType("int");
        
        builder.OwnsOne(od => od.Price, price =>
        {
            price.Property(od => od.Value).HasColumnType("money");
            price.Property(od => od.Currency).HasMaxLength(5);
        });
       

    }
}