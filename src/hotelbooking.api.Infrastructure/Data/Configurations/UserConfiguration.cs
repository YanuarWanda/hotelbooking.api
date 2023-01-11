using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Infrastructure.Data.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    protected override void EntityConfiguration(EntityTypeBuilder<User> builder)
    {
        builder.ToTable($"Mst{nameof(User)}");
        builder.HasKey(e => e.UserId);
        builder.Property(e => e.UserId).ValueGeneratedNever();

        builder.Property(e => e.Email).HasMaxLength(256);
        builder.Property(e => e.Name).HasMaxLength(1024);
        builder.Property(e => e.Salt).HasMaxLength(1024);
        builder.Property(e => e.HashedPassword).HasMaxLength(1024);
    }
}