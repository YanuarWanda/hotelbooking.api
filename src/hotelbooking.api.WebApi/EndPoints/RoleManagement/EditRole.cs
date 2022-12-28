using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi.Common;
using hotelbooking.api.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace hotelbooking.api.WebApi.EndPoints.RoleManagement;

public class EditRole : EndpointBaseAsync.WithRequest<EditRoleRequest>.WithActionResult
{
	private readonly IPermissionService _permissionService;
	private readonly IRoleService _roleService;
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;

	public EditRole(IPermissionService permissionService,
		IRoleService roleService,
		IApplicationDbContext dbContext,
		ICurrentUserService currentUserService)
	{
		_permissionService = permissionService;
		_roleService = roleService;
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	[Authorize]
	[HttpPut(EditRoleRequest.Route)]
	[SwaggerResponse((int)HttpStatusCode.OK, "", typeof(KeyDto))]
	[SwaggerOperation(
		Summary = "API Update Roles",
		OperationId = "RoleManagement.Patch",
		Tags = new[] {"RoleManagement"})
	]
	public override async Task<ActionResult> HandleAsync([FromRoute] EditRoleRequest request,
		CancellationToken cancellationToken = new CancellationToken())
	{
		var validator = new EditRoleRequestValidator(_permissionService);
		var validationResult =
			await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
			return BadRequest(ValidationExceptionBuilder.Build(validationResult.Errors));

		var role = await _roleService.GetByIdAsync(request.RoleId, cancellationToken);
		if (role == null)
			return BadRequest(ValidationExceptionBuilder.Build(validationResult.Errors));

		//recheck current
		foreach (var item in role.RolePermissions)
		{
			string s = item.PermissionId.ToString();
			if (!request.Dto!.PermissionIds!.Any(e => e == s))
				item.SetToSoftDelete(_currentUserService.UserId!, DateTime.UtcNow);
		}

		//add new
		foreach (var item in request.Dto!.PermissionIds!)
		{
			foreach (var item2 in role.RolePermissions)
			{
				if (item != item2.PermissionId.ToString()) continue;

				role.RolePermissions.Add(new RolePermission {PermissionId = new Guid(item)});
				break;
			}
		}

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Ok(KeyDtoHelper.Create(role.RoleId.ToString(), KeyDto.Guid));
	}
}