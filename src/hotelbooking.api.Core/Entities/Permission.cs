using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core.Entities;

public class Permission : BaseEntity
{
	public Permission()
	{
		PermissionId = Guid.NewGuid();
	}

	public Guid PermissionId { get; set; }
	public string Name { get; set; } = string.Empty;
}