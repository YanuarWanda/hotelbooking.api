using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Infrastructure.Data.Configurations;

public class RoleConfiguration : BaseEntityConfiguration<Role>
{
	protected override void EntityConfiguration(EntityTypeBuilder<Role> builder)
	{
		builder.ToTable($"Mst{nameof(Role)}");
		builder.HasKey(e => e.RoleId);
		builder.Property(e => e.RoleId).ValueGeneratedNever();

		builder.Property(e => e.Name).HasMaxLength(maxLength: 256);
		builder.HasIndex(e => e.Name).IsUnique();

		builder.Property(e => e.NormalizedName).HasMaxLength(maxLength: 256);
		builder.HasIndex(e => e.NormalizedName).IsUnique();
	}
}