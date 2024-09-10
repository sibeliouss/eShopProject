using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class UserConfiguration :IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users").HasKey(u => u.Id);
        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.FirstName).HasColumnName("FirstName").HasMaxLength(20).IsRequired();
        builder.Property(u => u.LastName).HasColumnName("LastName").HasMaxLength(20).IsRequired();
        builder.Property(u => u.UserName).HasColumnName("UserName").HasMaxLength(20).IsRequired();
        builder.Property(u => u.Email).HasColumnName("Email").HasMaxLength(50).IsRequired();
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

    }
}