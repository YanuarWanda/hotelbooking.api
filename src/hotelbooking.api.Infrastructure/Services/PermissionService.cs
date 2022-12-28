using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.Infrastructure.Services;

public class PermissionService : IPermissionService
{
	private readonly IApplicationDbContext _dbContext;

	public PermissionService(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IReadOnlyList<Permission>> GetAllAsync(int page, int size, CancellationToken cancellationToken)
	{
		var results = await _dbContext.Permissions.Skip((page - 1) == 0 || (page - 1) < 0 ? 0 : (page - 1) * size)
			.Take(size)
			.ToListAsync(cancellationToken);

		return results;
	}

	public Task<Permission?> GetByIdAsync(string id, CancellationToken cancellationToken)
	{
		var gId = new Guid(id);

		return _dbContext.Permissions.Where(e => e.PermissionId == gId).FirstOrDefaultAsync(cancellationToken);
	}

	public Task<bool> IsExistsAsync(string id, CancellationToken cancellationToken)
	{
		var gId = new Guid(id);

		return _dbContext.Permissions.AnyAsync(e => e.PermissionId == gId, cancellationToken);
	}

	public Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken)
	{
		name = name.ToLower();
		return _dbContext.Permissions.Where(e => e.Name == name).FirstOrDefaultAsync(cancellationToken);
	}
}