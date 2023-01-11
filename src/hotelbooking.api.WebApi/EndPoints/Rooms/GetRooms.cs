using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Events;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi.Common;
using hotelbooking.api.WebApi.Filters;
using hotelbooking.api.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;

namespace hotelbooking.api.WebApi.EndPoints.Rooms;

public class GetRooms : EndpointBaseAsync.WithRequest<GetRoomsRequest>.WithActionResult<GetRoomsResponse>
{
	private readonly IBookingService _bookingService;
	private readonly IDateTime _dateTime;
	private readonly IApplicationDbContext _dbContext;

	public GetRooms(IBookingService bookingService, IDateTime dateTime, IApplicationDbContext dbContext)
	{
		_bookingService = bookingService;
		_dateTime = dateTime;
		_dbContext = dbContext;
	}

	[Authorize]
	[HttpGet(GetRoomsRequest.Route)]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, "", typeof(ErrorResponse))]
	[SwaggerResponse((int)HttpStatusCode.InternalServerError, "", typeof(ErrorResponse))]
	[SwaggerResponse((int)HttpStatusCode.OK, "", typeof(GetRoomsResponse))]
	[SwaggerOperation(
		Summary = "API Get Rooms",
		OperationId = "Rooms.Get",
		Tags = new[] {"Rooms"})
	]
	public override async Task<ActionResult<GetRoomsResponse>> HandleAsync([FromQuery] GetRoomsRequest request,
		CancellationToken cancellationToken = new())
	{
		var validator = new GetRoomsRequestValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			return BadRequest(ValidationExceptionBuilder.Build(validationResult.Errors));
		}

		var query = _dbContext.Rooms.Include(x => x.RoomFacilities)
				.ThenInclude(x => x.Facility)
				.AsQueryable();

		if (request.FacilityIds != null && request.FacilityIds!.Length > 0)
		{
			foreach(var facilityId in request.FacilityIds!)
			{
				query = query.Where(x => x.RoomFacilities.Any(y => y.FacilityId.Equals(new Guid(facilityId))));
			}
		}

		if (request.CheckInDate != null && request.CheckOutDate != null)
		{
			query = query.ToList()
				.Where(x => !_bookingService.IsRoomBooked(x.RoomId, request.CheckInDate, request.CheckOutDate))
				.AsQueryable();
		}

		var rooms = query.ToList();

		return Ok(GetRoomsResponseExtension.Build(rooms));
	}
}