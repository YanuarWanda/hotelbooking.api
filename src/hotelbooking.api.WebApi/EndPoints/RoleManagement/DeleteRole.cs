using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace hotelbooking.api.WebApi.EndPoints.RoleManagement;

public class Delete : EndpointBaseAsync.WithRequest<DeleteRequest>.WithActionResult
{
	private readonly IRoleService _roleService;
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;

	public Delete(IRoleService roleService,
		IApplicationDbContext dbContext,
		ICurrentUserService currentUserService)
	{
		_roleService = roleService;
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	[Authorize]
	[HttpDelete(DeleteRequest.Route)]
	[SwaggerResponse((int)HttpStatusCode.NoContent)]
	[SwaggerOperation(
		Summary = "API Delete Roles",
		OperationId = "RoleManagement.Delete",
		Tags = new[] {"RoleManagement"})
	]
	public override async Task<ActionResult> HandleAsync(DeleteRequest request,
		CancellationToken cancellationToken = new CancellationToken())
	{
		var role = await _roleService.GetByIdAsync(request.RoleId, cancellationToken);
		if (role == null || !role.IsActive)
			return NotFound(ErrorResponseExtension.Create("Data not found"));

		if (role.DeletedDt.HasValue)
			return BadRequest(ErrorResponseExtension.Create("Data already deleted"));

		role.SetToSoftDelete(_currentUserService.UserId!, DateTime.UtcNow);

		await _dbContext.SaveChangesAsync(cancellationToken);

		return NoContent();
	}
}