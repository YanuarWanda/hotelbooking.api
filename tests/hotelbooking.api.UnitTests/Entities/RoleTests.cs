using System;
using FluentAssertions;
using hotelbooking.api.Core.Entities;
using Xunit;

namespace hotelbooking.api.UnitTests.Entities;

public class RoleTests
{
	[Fact]
	public void RoleConstructorPropertyRoleIdShouldNotBeEmpty()
	{
		var role = new Role();

		role.RoleId.Should().NotBe(Guid.Empty);
	}
}