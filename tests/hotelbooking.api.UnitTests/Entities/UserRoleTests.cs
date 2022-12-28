using System;
using FluentAssertions;
using hotelbooking.api.Core.Entities;
using Xunit;

namespace hotelbooking.api.UnitTests.Entities;

public class UserRoleTests
{
	[Fact]
	public void UserRoleConstructorPropertyUserRoleIdShouldNotBeEmpty()
	{
		var entity = new UserRole();

		entity.UserRoleId.Should().NotBe(Guid.Empty);
	}
}