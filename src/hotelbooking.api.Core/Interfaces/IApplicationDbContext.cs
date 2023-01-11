using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Core.Interfaces;

public interface IApplicationDbContext
{
	void DetachAllEntities();
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);

	DbSet<User> Users { get; }
	DbSet<Booking> Bookings { get; }
	DbSet<Room> Rooms { get; }
	DbSet<Facility> Facilities { get; }
	DbSet<RoomFacility> RoomFacilities { get; }
}