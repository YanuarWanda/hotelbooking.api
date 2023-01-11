namespace hotelbooking.api.WebApi.EndPoints.Users;

public class RegistrationRequest
{
    public const string Route = "api/users/registration";

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}