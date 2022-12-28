using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Events;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;
using hotelbooking.api.WebApi.Common;
using hotelbooking.api.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace hotelbooking.api.WebApi.EndPoints.UserManagement;

public class CreateUser : EndpointBaseAsync.WithRequest<CreateUserRequest>.WithActionResult
{
	private readonly IRoleService _roleService;
	private readonly IUserService _userService;
	private readonly IApplicationDbContext _dbContext;

	public CreateUser(IRoleService roleService, IUserService userService,
		IApplicationDbContext dbContext)
	{
		_roleService = roleService;
		_userService = userService;
		_dbContext = dbContext;
	}

	[Authorize]
	[HttpPost(CreateUserRequest.Route)]
	[SwaggerResponse((int)HttpStatusCode.OK, "", typeof(KeyDto))]
	[SwaggerOperation(
		Summary = "API Create Users",
		OperationId = "UserManagement.Post",
		Tags = new[] {"UserManagement"})
	]
	public override async Task<ActionResult> HandleAsync(CreateUserRequest request,
		CancellationToken cancellationToken = new CancellationToken())
	{
		var validator = new CreateUserRequestValidator(_roleService);
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
			return BadRequest(ValidationExceptionBuilder.Build(validationResult.Errors));

		var user = await _userService.GetByUsernameAsync(request.Username!, cancellationToken);
		if (user != null)
			return BadRequest(ErrorResponseExtension.Create("Username already exists"));

		var newUser = new User();
		newUser.Username = request.Username!.ToLowerInvariant();
		newUser.NormalizedUsername = newUser.Username.ToUpperInvariant();
		newUser.Salt = RandomHelper.GetSecureRandomString(length: 64);
		newUser.UpdateName(request.FirstName!, request.MiddleName, request.LastName);
		newUser.HashedPassword = string.Concat(newUser.Salt, request.Password).ToSHA512();

		newUser.UserPasswords.Add(new UserPassword {Salt = newUser.Salt, HashedPassword = newUser.HashedPassword});

		foreach (var item in request.RoleIds!)
			newUser.UserRoles.Add(new UserRole {RoleId = new Guid(item)});

		newUser.Events.Add(new UserRegisteredEvent(newUser));

		_dbContext.Users.Add(newUser);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Ok(KeyDtoHelper.Create(newUser.UserId.ToString(), KeyDto.Guid));
	}
}