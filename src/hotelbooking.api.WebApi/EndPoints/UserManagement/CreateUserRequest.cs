namespace hotelbooking.api.WebApi.EndPoints.UserManagement;

public class CreateUserRequest
{
    public const string Route = "api/user-management/users";

    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string[]? RoleIds { get; set; }
}