using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Entities;

public class Booking : BaseEntity
{
    public Booking()
    {
        BookingId = Guid.NewGuid();
    }

    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid RoomId { get; set; }
    public Room? Room { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}