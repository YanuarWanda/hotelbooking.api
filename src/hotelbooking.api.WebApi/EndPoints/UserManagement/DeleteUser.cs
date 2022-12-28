using System.Net;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.WebApi.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace hotelbooking.api.WebApi.EndPoints.UserManagement;

public class DeleteUser : EndpointBaseAsync.WithRequest<DeleteUserRequest>.WithActionResult
{
    private readonly IUserService _userService;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUser(IUserService userService, IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpDelete(DeleteUserRequest.Route)]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(
        Summary = "API Delete Users",
        OperationId = "UserManagement.Delete",
        Tags = new[] {"UserManagement"})
    ]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteUserRequest userRequest,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var user = await _userService.GetByIdAsync(userRequest.UserId, cancellationToken);
        if (user == null)
            return NotFound(ErrorResponseExtension.Create("User not found"));

        user.SetToSoftDelete(_currentUserService.UserId!, DateTime.UtcNow);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}