using hotelbooking.api.Core.Entities;
using hotelbooking.api.SharedKernel.Interfaces;

namespace hotelbooking.api.Core.Interfaces;

public interface IRoleService : IBaseReadRepository<Role>
{
	Task<bool> IsNameExists(string name, CancellationToken cancellationToken);
}