using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;
using hotelbooking.api.WebApi.Services;
using hotelbooking.api.WebApi.EndPoints.Rooms;

namespace hotelbooking.api.WebApi.EndPoints.Bookings;

public class CreateBookingResponse
{
	public Guid RoomId { get; init; }
	public DateTime CheckInDate { get; init; }
	public DateTime CheckOutDate { get; init; }
	public int TotalPrice { get; init; } = 0;
}

public static class CreateBookingResponseExtension
{
	public static CreateBookingResponse Build(Booking booking)
	{
		return new CreateBookingResponse
		{
			RoomId = booking.RoomId,
			CheckInDate = booking.CheckInDate,
			CheckOutDate = booking.CheckOutDate,
			TotalPrice = DaysBetween(booking.CheckInDate, booking.CheckOutDate) * booking.Room!.PricePerNight
		};
	}

	private static int DaysBetween (DateTime d1, DateTime d2)
	{
		TimeSpan span = d2.Subtract(d1);
		return (int) span.TotalDays;
	}
}