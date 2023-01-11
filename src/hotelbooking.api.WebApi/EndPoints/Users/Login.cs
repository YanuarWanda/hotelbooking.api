using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Events;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi.Common;
using hotelbooking.api.WebApi.Filters;
using hotelbooking.api.WebApi.Models;
using hotelbooking.api.WebApi.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace hotelbooking.api.WebApi.EndPoints.Users;

public class Login : EndpointBaseAsync.WithRequest<LoginRequest>.WithActionResult<LoginResponse>
{
	private readonly IUserService _userService;
	private readonly JwtService _jwtService;
	private readonly IDateTime _dateTime;
	private readonly IApplicationDbContext _dbContext;

	public Login(IUserService userService, JwtService jwtService, IDateTime dateTime, IApplicationDbContext dbContext)
	{
		_userService = userService;
		_jwtService = jwtService;
		_dateTime = dateTime;
		_dbContext = dbContext;
	}

	[Authorize]
	[HttpPost(LoginRequest.Route)]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, "", typeof(ErrorResponse))]
	[SwaggerResponse((int)HttpStatusCode.InternalServerError, "", typeof(ErrorResponse))]
	[SwaggerResponse((int)HttpStatusCode.OK, "", typeof(LoginResponse))]
	[SwaggerOperation(
		Summary = "API Login",
		OperationId = "Users.Login",
		Tags = new[] {"Users"})
	]
	public override async Task<ActionResult<LoginResponse>> HandleAsync([FromBody] LoginRequest request,
		CancellationToken cancellationToken = new())
	{
		var validator = new LoginRequestValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			return BadRequest(ValidationExceptionBuilder.Build(validationResult.Errors));
		}

		var user = await _userService.GetByEmailAsync(request.Email!, cancellationToken);
		if (user == null)
			return BadRequest(ErrorResponseExtension.Create("Invalid email or password"));

		var pass = await _userService.CheckPasswordAsync(user.UserId, request.Password!, cancellationToken);
		if (!pass)
			return BadRequest(ErrorResponseExtension.Create("Invalid email or password"));

		user.Events.Add(new LoginSuccessEvent(user));

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Ok(LoginResponseExtension.Build(_jwtService, user));
	}
}