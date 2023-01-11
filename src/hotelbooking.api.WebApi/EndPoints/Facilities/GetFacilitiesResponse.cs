using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.WebApi.EndPoints.Facilities;

public class GetFacilitiesResponse
{
	public Guid FacilityId { get; init; } = new Guid();
	public string Name { get; init;} = string.Empty;
}

public static class GetFacilitiesResponseExtension
{
	public static List<GetFacilitiesResponse> Build(List<Facility> facilities)
	{
		return facilities.Select(x => new GetFacilitiesResponse
		{
			FacilityId = x.FacilityId,
			Name = x.Name
		}).ToList();
	}
}