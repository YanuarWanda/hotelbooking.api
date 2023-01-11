using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Infrastructure.Data.Configurations;

public class RoomConfiguration : BaseEntityConfiguration<Room>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable($"Mst{nameof(Room)}");
        builder.HasKey(e => e.RoomId);
        builder.Property(e => e.RoomId).ValueGeneratedNever();

        builder.Property(e => e.Name).HasMaxLength(256);
        builder.Property(e => e.Description).HasMaxLength(1024);
        builder.Property(e => e.Pax).HasColumnType("int").HasMaxLength(2);
        builder.Property(e => e.PricePerNight).HasColumnType("int").HasMaxLength(8);
    }
}