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

        builder.Property(e => e.Username).HasMaxLength(256);
        builder.Property(e => e.NormalizedUsername).HasMaxLength(256);
        builder.Property(e => e.Salt).HasMaxLength(1024);
        builder.Property(e => e.HashedPassword).HasMaxLength(1024);
        builder.Property(e => e.FirstName).HasMaxLength(256);
        builder.Property(e => e.MiddleName).HasMaxLength(256);
        builder.Property(e => e.LastName).HasMaxLength(256);
        builder.Property(e => e.FullName).HasMaxLength(1024);
    }
}