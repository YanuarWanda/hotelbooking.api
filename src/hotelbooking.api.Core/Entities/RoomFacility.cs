using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Entities;

public class RoomFacility : BaseEntity
{
    public RoomFacility()
    {
        RoomFacilityId = Guid.NewGuid();
    }

    public Guid RoomFacilityId { get; set; }
    public Guid RoomId { get; set; }
    public Room? Room { get; set; }
    public Guid FacilityId { get; set; }
    public Facility? Facility { get; set; }
}