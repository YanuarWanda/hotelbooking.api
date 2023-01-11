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

	public async Task<bool> IsRoomBooked(Guid roomId, CancellationToken cancellationToken)
	{
		var booking = await GetBaseBookingQueryable()
			.Where(b => b.RoomId.Equals(roomId))
			.FirstOrDefaultAsync(cancellationToken);

		return booking != null;
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