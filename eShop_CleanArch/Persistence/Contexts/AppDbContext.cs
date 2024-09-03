using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<BillingAddress> BillingAddresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory>? ProductCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    { 
        builder.Ignore<IdentityUserRole<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>(); 
        
        builder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });
        builder.Entity<BillingAddress>()
            .HasOne(b => b.Customer)
            .WithOne() // BillingAddress, Customer'ın tek bir adresine sahip olabilir
            .HasForeignKey<BillingAddress>(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict); // İlişkili Customer silindiğinde BillingAddress silinmez

        // Address ile Customer arasındaki ilişki
        builder.Entity<Address>()
            .HasOne(a => a.Customer)
            .WithMany() // Bir Customer birden fazla Address'e sahip olabilir
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict); // İlişkili Customer silindiğinde Address silinmez
        builder.Entity<ProductCategory>()
            .HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // veya .OnDelete(DeleteBehavior.NoAction)
        
        builder.Entity<ProductCategory>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // veya .OnDelete(DeleteBehavior.NoAction)

    }
}