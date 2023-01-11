using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;
using hotelbooking.api.WebApi.Services;

namespace hotelbooking.api.WebApi.EndPoints.Users;

public class LoginResponse
{
	public string Token { get; init; } = string.Empty;
	public string Email { get; init; } = string.Empty;
	public string Name { get; init;} = string.Empty;
}

public static class LoginResponseExtension
{
	public static LoginResponse Build(JwtService jwtService, User user)
	{
		return new LoginResponse()
		{
			Token = jwtService.GenerateJwtToken(user),
			Email = user.Email,
			Name = user.Name
		};
	}
}