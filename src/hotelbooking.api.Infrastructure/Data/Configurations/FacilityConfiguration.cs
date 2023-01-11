using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Infrastructure.Data.Configurations;

public class FacilityConfiguration : BaseEntityConfiguration<Facility>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Facility> builder)
    {
        builder.ToTable($"Mst{nameof(Facility)}");
        builder.HasKey(e => e.FacilityId);
        builder.Property(e => e.FacilityId).ValueGeneratedNever();

        builder.Property(e => e.Name).HasMaxLength(256);
    }
}