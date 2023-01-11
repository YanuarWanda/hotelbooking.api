using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.Core.Interfaces;

public interface IUserEntity
{
	DbSet<User> Users { get; }
	DbSet<Booking> Bookings { get; }
}