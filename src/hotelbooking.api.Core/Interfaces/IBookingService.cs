using hotelbooking.api.Core.Entities;
using hotelbooking.api.SharedKernel.Interfaces;

namespace hotelbooking.api.Core.Interfaces;

public interface IBookingService : IBaseReadRepository<User>
{
	Task<bool> IsRoomBooked(Guid roomId, CancellationToken cancellationToken);
}