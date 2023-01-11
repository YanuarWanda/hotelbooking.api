using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Core;

public static class CoreSeed
{
	public static async Task Seeder(IApplicationDbContext dbContext)
	{
		if (!await dbContext.Users.AnyAsync(e => e.Email == "user@mail.com"))
		{
			string s = RandomHelper.GetSecureRandomString(64);
			var user = new User
			{
				Email = "user@mail.com",
				Salt = s,
				HashedPassword = string.Concat(s, "Qwerty@1234").ToSHA512(),
			};

			await dbContext.Users.AddAsync(user);
			await dbContext.SaveChangesAsync(CancellationToken.None);
		}

		if (!await dbContext.Rooms.AnyAsync(e => e.Name == "Kamar A"))
		{
			var room = new Room
			{
				Name = "Kamar A",
				Description = "Deskripsi Kamar A",
				Pax = 2,
				PricePerNight = 250000
			};

			await dbContext.Rooms.AddAsync(room);
			await dbContext.SaveChangesAsync(CancellationToken.None);
		}

		if (!await dbContext.Facilities.AnyAsync(e => e.Name == "TV"))
		{
			var facility = new Facility
			{
				Name = "TV"
			};

			await dbContext.Facilities.AddAsync(facility);
			await dbContext.SaveChangesAsync(CancellationToken.None);
		}

		if (!await dbContext.Facilities.AnyAsync(e => e.Name == "AC"))
		{
			var facility = new Facility
			{
				Name = "AC"
			};

			await dbContext.Facilities.AddAsync(facility);
			await dbContext.SaveChangesAsync(CancellationToken.None);
		}

		if (!await dbContext.RoomFacilities.AnyAsync(e => e.Room!.Name == "Kamar A" && e.Facility!.Name == "TV"))
		{
			var room = await dbContext.Rooms.FirstOrDefaultAsync(x => x.Name == "Kamar A");
			var facility = await dbContext.Facilities.FirstOrDefaultAsync(x => x.Name == "TV");
			var roomFacility = new RoomFacility
			{
				RoomId = room!.RoomId,
				FacilityId = facility!.FacilityId
			};

			await dbContext.RoomFacilities.AddAsync(roomFacility);
			await dbContext.SaveChangesAsync(CancellationToken.None);
		}

		if (!await dbContext.RoomFacilities.AnyAsync(e => e.Room!.Name == "Kamar A" && e.Facility!.Name == "AC"))
		{
			var room = await dbContext.Rooms.FirstOrDefaultAsync(x => x.Name == "Kamar A");
			var facility = await dbContext.Facilities.FirstOrDefaultAsync(x => x.Name == "AC");
			var roomFacility = new RoomFacility
			{
				RoomId = room!.RoomId,
				FacilityId = facility!.FacilityId
			};

			await dbContext.RoomFacilities.AddAsync(roomFacility);
			await dbContext.SaveChangesAsync(CancellationToken.None);
		}
	}
}