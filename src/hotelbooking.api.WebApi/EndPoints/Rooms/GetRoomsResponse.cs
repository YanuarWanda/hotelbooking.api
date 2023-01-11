using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.WebApi.EndPoints.Rooms;

public class GetRoomsResponse
{
	public Guid RoomId { get; init; } = new Guid();
	public string Name { get; init;} = string.Empty;
	public string Description { get; init;} = string.Empty;
	public int Pax { get; init;} = 0;
	public int PricePerNight { get; init;} = 0;
	public List<string>? Facilities { get; init; }
}

public static class GetRoomsResponseExtension
{
	public static List<GetRoomsResponse> Build(IQueryable<Room> rooms)
	{
		return rooms.Include(x => x.RoomFacilities).ThenInclude(x => x.Facility).Select(room => new GetRoomsResponse
		{
			RoomId = room.RoomId,
			Name = room.Name,
			Description = room.Description,
			Pax = room.Pax,
			PricePerNight = room.PricePerNight,
			Facilities = room.RoomFacilities.Select(x => x.Facility!.Name).ToList()
		}).ToList();
	}
}