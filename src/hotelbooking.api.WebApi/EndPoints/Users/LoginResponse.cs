using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.WebApi.EndPoints.Users;

public class LoginResponse
{
	public string Email { get; init; } = string.Empty;
	public string Name { get; init;} = string.Empty;
}

public static class LoginResponseExtension
{
	public static LoginResponse Build(User user)
	{
		return new LoginResponse()
		{
			Email = user.Email,
			Name = user.Name
		};
	}
}