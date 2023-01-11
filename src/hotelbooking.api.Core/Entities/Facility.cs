using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Entities;

public class Facility : BaseEntity
{
	public Facility()
	{
		FacilityId = Guid.NewGuid();
	}

	public Guid FacilityId { get; set; }
	public string Name { get; set; } = String.Empty;
}