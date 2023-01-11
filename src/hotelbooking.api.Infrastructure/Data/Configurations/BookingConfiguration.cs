using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Infrastructure.Data.Configurations;

public class BookingConfiguration : BaseEntityConfiguration<Booking>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable($"Trx{nameof(Booking)}");
        builder.HasKey(e => e.BookingId);
        builder.Property(e => e.BookingId).ValueGeneratedNever();

        builder.Property(e => e.CheckInDate).HasColumnType("date");
        builder.Property(e => e.CheckOutDate).HasColumnType("date");
    }
}