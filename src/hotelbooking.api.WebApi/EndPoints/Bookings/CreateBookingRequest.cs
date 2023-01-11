namespace hotelbooking.api.WebApi.EndPoints.Bookings;

public class CreateBookingRequest
{
    public const string Route = "api/bookings";

	public Guid RoomId { get; set; }
	public DateTime CheckInDate { get; set; }
	public DateTime CheckOutDate { get; set; }
}