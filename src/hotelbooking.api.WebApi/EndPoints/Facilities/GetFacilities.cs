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

namespace hotelbooking.api.WebApi.EndPoints.Facilities;

public class GetFacilities : EndpointBaseAsync.WithoutRequest.WithActionResult<GetFacilitiesResponse>
{
	private readonly IUserService _userService;
	private readonly IDateTime _dateTime;
	private readonly IApplicationDbContext _dbContext;

	public GetFacilities(IUserService userService, IDateTime dateTime, IApplicationDbContext dbContext)
	{
		_userService = userService;
		_dateTime = dateTime;
		_dbContext = dbContext;
	}

	[Authorize]
	[HttpGet(GetFacilitiesRequest.Route)]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, "", typeof(ErrorResponse))]
	[SwaggerResponse((int)HttpStatusCode.InternalServerError, "", typeof(ErrorResponse))]
	[SwaggerResponse((int)HttpStatusCode.OK, "", typeof(GetFacilitiesResponse))]
	[SwaggerOperation(
		Summary = "API Get Facilities",
		OperationId = "Facilities.Get",
		Tags = new[] {"Facilities"})
	]
	public override async Task<ActionResult<GetFacilitiesResponse>> HandleAsync(CancellationToken cancellationToken = new())
	{
		var facilities = await _dbContext.Facilities.ToListAsync();

		return Ok(GetFacilitiesResponseExtension.Build(facilities));
	}
}