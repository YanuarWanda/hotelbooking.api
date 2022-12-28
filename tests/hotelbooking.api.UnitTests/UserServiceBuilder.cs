using System;
using System.Threading;
using Moq;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.UnitTests;

public class UserServiceBuilder
{
	private readonly Mock<IUserService> _mock;

	public UserServiceBuilder()
	{
		_mock = new Mock<IUserService>();
	}

	public UserServiceBuilder GetUserByIdAsync(Guid userId, User? callbackResult)
	{
		string s = userId.ToString();

		_mock.Setup(e => e.GetByIdAsync(s, It.IsAny<CancellationToken>()))
			.ReturnsAsync(callbackResult);

		return this;
	}

	public UserServiceBuilder SetupGetUserByUsername(string? username, User? callbackResult)
	{
		if (string.IsNullOrWhiteSpace(username))
		{
			_mock.Setup(e => e.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(callbackResult);
		}
		else
		{
			_mock.Setup(e => e.GetByUsernameAsync(username, It.IsAny<CancellationToken>()))
				.ReturnsAsync(callbackResult);
		}

		return this;
	}

	public UserServiceBuilder SetupCheckPassword(Guid userId, string password, bool callbackResult)
	{
		_mock.Setup(e => e.CheckPasswordAsync(userId, password, It.IsAny<CancellationToken>()))
			.ReturnsAsync(callbackResult);

		return this;
	}

	public UserServiceBuilder SetupIsEmailAddressExists(string password, bool callbackResult)
	{
		_mock.Setup(e => e.IsEmailAddressExists(password, It.IsAny<CancellationToken>()))
			.ReturnsAsync(callbackResult);

		return this;
	}

	public Mock<IUserService> Build()
	{
		return _mock;
	}
}