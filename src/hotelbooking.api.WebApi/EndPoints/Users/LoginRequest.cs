namespace hotelbooking.api.WebApi.EndPoints.Users;

public class LoginRequest
{
    public const string Route = "api/users/login";

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}