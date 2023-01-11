using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Entities;

public class User : BaseEntity
{
	public User()
	{
		UserId = Guid.NewGuid();

        Bookings = new HashSet<Booking>();
	}

	public Guid UserId { get; set; }
	public string Email { get; set; } = String.Empty;
	public string Name { get; set; } = String.Empty;
	public string Salt { get; set; } = String.Empty;
	public string HashedPassword { get; set; } = String.Empty;

    public ICollection<Booking> Bookings { get; set; }
}