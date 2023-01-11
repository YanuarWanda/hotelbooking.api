using hotelbooking.api.Core.Entities;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Events;

public class BookingFailedEvent : BaseDomainEvent
{
	public Booking Booking { get; }

	public BookingFailedEvent(Booking booking)
	{
		Booking = booking;
	}
}