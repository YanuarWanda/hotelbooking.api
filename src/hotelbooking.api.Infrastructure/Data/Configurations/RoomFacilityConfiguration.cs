using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Infrastructure.Data.Configurations;

public class RoomFacilityConfiguration : BaseEntityConfiguration<RoomFacility>
{
    protected override void EntityConfiguration(EntityTypeBuilder<RoomFacility> builder)
    {
        builder.ToTable($"Trx{nameof(RoomFacility)}");
        builder.HasKey(e => e.RoomFacilityId);
        builder.Property(e => e.RoomFacilityId).ValueGeneratedNever();
    }
}