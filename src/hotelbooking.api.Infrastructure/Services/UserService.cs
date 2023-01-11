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

		return query;
	}

	public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
	{
		var s = email.ToUpper();

		return GetBaseUserQueryable().Where(e => e.Email.ToUpper() == s).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<bool> CheckPasswordAsync(Guid userId, string password, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users.Where(e => e.UserId == userId).FirstOrDefaultAsync(cancellationToken);
		if (user == null || string.IsNullOrWhiteSpace(user.HashedPassword))
			return false;

		return string.Concat(user.Salt, password).ToSHA512() == user.HashedPassword;
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