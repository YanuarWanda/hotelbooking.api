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
	private readonly IUserService _userService;
	private readonly IDateTime _dateTime;
	private readonly IApplicationDbContext _dbContext;

	public GetRooms(IUserService userService, IDateTime dateTime, IApplicationDbContext dbContext)
	{
		_userService = userService;
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

		var query = _dbContext.Rooms.AsQueryable();

		if (request.FacilityIds != null && request.FacilityIds!.Length > 0)
		{
			foreach(var facilityId in request.FacilityIds!)
			{
				query = query.Where(x => x.RoomFacilities.Any(y => y.FacilityId.Equals(new Guid(facilityId))));
			}
		}

		var rooms = query.Include(x => x.RoomFacilities).AsQueryable();

		return Ok(GetRoomsResponseExtension.Build(rooms));
	}
}