using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Infrastructure.Data.Configurations;

public class RolePermissionConfiguration : BaseEntityConfiguration<RolePermission>
{
    protected override void EntityConfiguration(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable($"Trx{nameof(RolePermission)}");
        builder.HasKey(e => e.RolePermissionId);
        builder.Property(e => e.RolePermissionId).ValueGeneratedNever();
    }
}