using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Infrastructure.Services;
using Xunit;

namespace hotelbooking.api.UnitTests.Services;

public class UserServiceTests
{
	[Fact]
	public void GetUserByUsernameShouldReturnCorrectUser()
	{
		const string s = "test@test.com";
		var fakeUsers = new List<User>();
		fakeUsers.Add(new UserBuilder().WithDefaultValues().Username(s).Build());

		var expectedUser = fakeUsers.First(e => e.NormalizedUsername == s.ToUpperInvariant());

		var userService =
			new UserService(new InterfaceApplicationDbContextBuilder().SetupUser(fakeUsers).Build().Object);

		var result = userService.GetByUsernameAsync(s, CancellationToken.None).GetAwaiter().GetResult();
		result.Should().Be(expectedUser);
	}

	[Fact]
	public void GetUserByUsernameShouldReturnNull()
	{
		const string s = "vendy@hotelbooking.api.com";
		var fakeUsers = new List<User>();
		fakeUsers.Add(new UserBuilder().WithDefaultValues().Build());

		var userService =
			new UserService(new InterfaceApplicationDbContextBuilder().SetupUser(fakeUsers).Build().Object);

		var result = userService.GetByUsernameAsync(s, CancellationToken.None).GetAwaiter().GetResult();

		result.Should().BeNull();
	}

	[Fact]
	public void GetUserByIdReturnCorrectUser()
	{
		var fakeUsers = new List<User>();
		fakeUsers.Add(new UserBuilder().WithDefaultValues().Build());
		var expectedUser = fakeUsers.First();

		var userService =
			new UserService(new InterfaceApplicationDbContextBuilder().SetupUser(fakeUsers).Build().Object);

		var result = userService.GetByIdAsync(expectedUser.UserId.ToString(), CancellationToken.None).GetAwaiter()
			.GetResult();
		result.Should().Be(expectedUser);
	}

	[Fact]
	public void CheckPasswordShouldReturnTrue()
	{
		string expectedPassword = "Qwerty@123";

		var fakeUsers = new List<User>();
		fakeUsers.Add(new UserBuilder().WithDefaultValues().Password(expectedPassword).Build());

		var expectedUser = fakeUsers.First();

		var userService =
			new UserService(new InterfaceApplicationDbContextBuilder().SetupUser(fakeUsers).Build().Object);

		var result = userService.CheckPasswordAsync(expectedUser.UserId, expectedPassword, CancellationToken.None)
			.GetAwaiter()
			.GetResult();

		result.Should().BeTrue();
	}

	[Fact]
	public void CheckPasswordShouldReturnFalse()
	{
		var fakeUsers = new List<User>();
		fakeUsers.Add(new UserBuilder().WithDefaultValues().Build());

		string fakePassword = "Qwerty@1234567890";
		var expectedUser = fakeUsers.First();

		var userService =
			new UserService(new InterfaceApplicationDbContextBuilder().SetupUser(fakeUsers).Build().Object);

		var result = userService.CheckPasswordAsync(expectedUser.UserId, fakePassword, CancellationToken.None)
			.GetAwaiter()
			.GetResult();

		result.Should().BeFalse();
	}

	[Fact]
	public void IsEmailAddressExistsShouldReturnFalse()
	{
		var fakeUsers = new List<User>();
		fakeUsers.Add(new UserBuilder().WithDefaultValues().Build());

		var interfaceDbContextBuilder = new InterfaceApplicationDbContextBuilder().SetupUser(fakeUsers).Build();

		var userService = new UserService(interfaceDbContextBuilder.Object);

		var result = userService.IsEmailAddressExists("vendy@hotelbooking.api.com", CancellationToken.None)
			.GetAwaiter()
			.GetResult();

		result.Should().BeFalse();
	}

	[Fact]
	public void IsEmailAddressExistsShouldReturnTrue()
	{
		var fakeUsers = new List<User>();
		var fakeUser = new UserBuilder().WithDefaultValues().Build();
		fakeUsers.Add(fakeUser);

		var interfaceDbContextBuilder = new InterfaceApplicationDbContextBuilder().SetupUser(fakeUsers).Build();

		var userService = new UserService(interfaceDbContextBuilder.Object);

		var result = userService.IsEmailAddressExists(fakeUser.Username, CancellationToken.None)
			.GetAwaiter()
			.GetResult();

		result.Should().BeTrue();
	}
}