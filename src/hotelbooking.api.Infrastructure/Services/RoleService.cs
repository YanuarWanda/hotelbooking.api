using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.Infrastructure.Services;

public class RoleService : IRoleService
{
	private readonly IApplicationDbContext _dbContext;

	public RoleService(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<bool> IsNameExists(string name, CancellationToken cancellationToken)
	{
		string s = name.ToUpperInvariant();

		return _dbContext.Roles.AnyAsync(e => e.NormalizedName == s, cancellationToken);
	}

	public async Task<IReadOnlyList<Role>> GetAllAsync(int page, int size, CancellationToken cancellationToken)
	{
		var results = await _dbContext.Roles.Skip((page - 1) == 0 || (page - 1) < 0 ? 0 : (page - 1) * size).Take(size)
			.ToListAsync(cancellationToken);

		return results;
	}

	public Task<Role?> GetByIdAsync(string id, CancellationToken cancellationToken)
	{
		var gId = new Guid(id);
		return _dbContext.Roles.Include(e => e.RolePermissions)
			.Where(e => e.RoleId == gId).FirstOrDefaultAsync(cancellationToken);
	}

	public Task<bool> IsExistsAsync(string id, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}