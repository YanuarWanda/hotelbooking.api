using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Events;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;
using hotelbooking.api.WebApi.Common;
using hotelbooking.api.WebApi.Filters;
using hotelbooking.api.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace hotelbooking.api.WebApi.EndPoints.Users;

public class CreateUser : EndpointBaseAsync.WithRequest<RegistrationRequest>.WithActionResult
{
	private readonly IUserService _userService;
	private readonly IApplicationDbContext _dbContext;

	public CreateUser(IUserService userService, IApplicationDbContext dbContext)
	{
		_userService = userService;
		_dbContext = dbContext;
	}

	[Authorize]
	[HttpPost(RegistrationRequest.Route)]
	[SwaggerResponse((int)HttpStatusCode.OK, "", typeof(KeyDto))]
	[SwaggerOperation(
		Summary = "API Registration",
		OperationId = "Users.Registration",
		Tags = new[] {"Users"})
	]
	public override async Task<ActionResult> HandleAsync(RegistrationRequest request,
		CancellationToken cancellationToken = new CancellationToken())
	{
		var validator = new RegistrationRequestValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
			return BadRequest(ValidationExceptionBuilder.Build(validationResult.Errors));

		var user = await _userService.GetByEmailAsync(request.Email!, cancellationToken);
		if (user != null)
			return BadRequest(ErrorResponseExtension.Create("Email already exists"));

		var newUser = new User();
		newUser.Email = request.Email!.ToLowerInvariant();
		newUser.Name = request.Name!;
		newUser.Salt = RandomHelper.GetSecureRandomString(length: 64);
		newUser.HashedPassword = string.Concat(newUser.Salt, request.Password).ToSHA512();

		newUser.Events.Add(new UserRegisteredEvent(newUser));

		_dbContext.Users.Add(newUser);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Ok(KeyDtoHelper.Create(newUser.UserId.ToString(), KeyDto.Guid));
	}
}