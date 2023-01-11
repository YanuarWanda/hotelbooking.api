using hotelbooking.api.Core.Entities;
using hotelbooking.api.SharedKernel.Interfaces;

namespace hotelbooking.api.Core.Interfaces;

public interface IUserService : IBaseReadRepository<User>
{
	Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
	Task<bool> CheckPasswordAsync(Guid userId, string password, CancellationToken cancellationToken);
}