using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Infrastructure.Data.Configurations;

public class PermissionConfiguration : BaseEntityConfiguration<Permission>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable($"Mst{nameof(Permission)}");
        builder.HasKey(e => e.PermissionId);
        builder.Property(e => e.PermissionId).ValueGeneratedNever();

        builder.Property(e => e.Name).HasMaxLength(256);
        builder.HasIndex(e => e.Name).IsUnique();
    }
}