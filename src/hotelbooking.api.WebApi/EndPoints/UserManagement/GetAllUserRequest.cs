using hotelbooking.api.WebApi.Models;

namespace hotelbooking.api.WebApi.EndPoints.UserManagement;

public class GetUserRequest : BaseFilterDto
{
    public const string Route = "api/user-management/users";

    public string? Name { get; set; }
}