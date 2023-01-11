using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Infrastructure.Services;

public class BookingService : IBookingService
{
	private readonly IApplicationDbContext _dbContext;

	public BookingService(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	private IQueryable<Booking> GetBaseBookingQueryable()
	{
		var query = _dbContext.Bookings.AsQueryable();

		return query;
	}

	public bool IsRoomBooked(Guid roomId, DateTime? checkInDate, DateTime? checkOutDate)
	{
		var existingBookings = GetBaseBookingQueryable()
			.Where(b => b.RoomId.Equals(roomId))
			.ToList();

		if (existingBookings != null && existingBookings.Count > 0)
		{
			foreach(var existingBooking in existingBookings)
			{
				if
				(
					(checkInDate >= existingBooking.CheckInDate && checkInDate <= existingBooking.CheckOutDate) ||
					(checkOutDate >= existingBooking.CheckInDate && checkInDate <= existingBooking.CheckOutDate) ||
					(checkInDate <= existingBooking.CheckInDate && checkOutDate >= existingBooking.CheckOutDate)
				)
				{
					return true;
				}
			}
		}

		return false;
	}

	public Task<IReadOnlyList<User>> GetAllAsync(int page, int size, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public Task<bool> IsExistsAsync(string id, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}