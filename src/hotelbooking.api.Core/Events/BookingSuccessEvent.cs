using hotelbooking.api.Core.Entities;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Events;

public class BookingSuccessEvent : BaseDomainEvent
{
	public Booking Booking { get; }

	public BookingSuccessEvent(Booking booking)
	{
		Booking = booking;
	}
}