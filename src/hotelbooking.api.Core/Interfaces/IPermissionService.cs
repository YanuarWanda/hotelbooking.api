using hotelbooking.api.Core.Entities;
using hotelbooking.api.SharedKernel.Interfaces;

namespace hotelbooking.api.Core.Interfaces;

public interface IPermissionService : IBaseReadRepository<Permission>
{
	Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken);
}