using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Infrastructure.Services;

public class UserService : IUserService
{
	private readonly IApplicationDbContext _dbContext;

	public UserService(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	private IQueryable<User> GetBaseUserQueryable()
	{
		var query = _dbContext.Users.AsQueryable();

		query = query.AsSplitQuery()
			.Include(e => e.UserPasswords)
			.Include(e => e.UserRoles)
			.ThenInclude(e => e.Role)
			.ThenInclude(e => e!.RolePermissions)
			.ThenInclude(e => e.Permission);

		return query;
	}

	public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
	{
		var s = username.ToUpperInvariant();

		return GetBaseUserQueryable().Where(e => e.NormalizedUsername == s).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<bool> CheckPasswordAsync(Guid userId, string password, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users.Where(e => e.UserId == userId).FirstOrDefaultAsync(cancellationToken);
		if (user == null || string.IsNullOrWhiteSpace(user.HashedPassword))
			return false;

		return string.Concat(user.Salt, password).ToSHA512() == user.HashedPassword;
	}

	public async Task<bool> IsEmailAddressExists(string email, CancellationToken cancellationToken)
	{
		var emailUpperCase = email.ToUpperInvariant();

		return await _dbContext.Users.AnyAsync(e => e.NormalizedUsername == emailUpperCase, cancellationToken);
	}

	public async Task<IReadOnlyList<User>> GetAllAsync(int page, int size, CancellationToken cancellationToken)
	{
		var results = await _dbContext.Users.Skip((page - 1) == 0 || (page - 1) < 0 ? 0 : (page - 1) * size)
			.Take(size)
			.ToListAsync(cancellationToken);

		return results;
	}

	public Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken)
	{
		var gId = new Guid(id);

		return GetBaseUserQueryable().Where(e => e.UserId == gId).FirstOrDefaultAsync(cancellationToken);
	}

	public Task<bool> IsExistsAsync(string id, CancellationToken cancellationToken)
	{
		var gId = new Guid(id);

		return _dbContext.Users.AnyAsync(e => e.UserId == gId, cancellationToken);
	}
}