using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi.Common;
using hotelbooking.api.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace hotelbooking.api.WebApi.EndPoints.Users;

public class GetUserByAuth : EndpointBaseAsync.WithoutRequest.WithActionResult<UserResponse>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IUserService _userService;

	public GetUserByAuth(ICurrentUserService currentUserService, IUserService userService)
	{
		_currentUserService = currentUserService;
		_userService = userService;
	}

	[Authorize]
	[HttpGet("api/users/me")]
	[SwaggerResponse((int)HttpStatusCode.OK, "", typeof(UserResponse))]
	[SwaggerOperation(
		Summary = "API Get User by Auth",
		OperationId = "Login.GetByAuth",
		Tags = new[] {"Login"})
	]
	public override async Task<ActionResult<UserResponse>> HandleAsync(
		CancellationToken cancellationToken = new CancellationToken())
	{
		var user = await _userService.GetByIdAsync(_currentUserService.UserId!, cancellationToken);
		if (user == null)
			return new NotFoundObjectResult(new ErrorResponse() {Message = "Data not found"});

		return new UserResponse(user.UserId, user.FirstName, user.MiddleName, user.LastName, user.FullName);
	}
}