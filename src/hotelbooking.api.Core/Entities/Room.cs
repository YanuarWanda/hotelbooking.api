using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Entities;

public class Room : BaseEntity
{
	public Room()
	{
		RoomId = Guid.NewGuid();

		RoomFacilities = new HashSet<RoomFacility>();
	}

	public Guid RoomId { get; set; }
	public string Name { get; set; } = String.Empty;
	public string Description { get; set; } = String.Empty;
	public int Pax { get; set; } = 0;
	public int PricePerNight { get; set; } = 0;

	public ICollection<RoomFacility> RoomFacilities { get; set; }
}