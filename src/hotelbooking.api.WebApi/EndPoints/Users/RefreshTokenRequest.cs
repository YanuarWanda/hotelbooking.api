using Microsoft.AspNetCore.Mvc;

namespace hotelbooking.api.WebApi.EndPoints.Users;

public class RefreshTokenRequest
{
    public const string Route = "api/users/refresh-token";

    [FromQuery] public string Token { get; init; } = string.Empty;
}