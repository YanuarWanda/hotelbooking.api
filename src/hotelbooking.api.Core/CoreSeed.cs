using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core;

public static class CoreSeed
{
	public static async Task Seeder(IApplicationDbContext dbContext)
	{
		if (!await dbContext.Users.AnyAsync(e => e.Email == "sa@mail.com"))
		{
			string s = RandomHelper.GetSecureRandomString(64);
			var sa = new User
			{
				Email = "sa@mail.com",
				Salt = s,
				HashedPassword = string.Concat(s, "Qwerty@1234").ToSHA512()
			};

			await dbContext.Users.AddAsync(sa);
			await dbContext.SaveChangesAsync(CancellationToken.None);
		}

		if (!await dbContext.Users.AnyAsync(e => e.Email == "customer@mail.com"))
		{
			string s = RandomHelper.GetSecureRandomString(64);
			var customer = new User
			{
				Email = "customer@mail.com",
				Salt = s,
				HashedPassword = string.Concat(s, "Qwerty@1234").ToSHA512()
			};

			await dbContext.Users.AddAsync(customer);
			await dbContext.SaveChangesAsync(CancellationToken.None);
		}
	}
}