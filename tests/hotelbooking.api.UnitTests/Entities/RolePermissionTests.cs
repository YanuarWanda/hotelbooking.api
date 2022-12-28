using System;
using FluentAssertions;
using hotelbooking.api.Core.Entities;
using Xunit;

namespace hotelbooking.api.UnitTests.Entities;

public class RolePermissionTests
{
	[Fact]
	public void RolePermissionConstructorShouldBeCorrect()
	{
		var rolePermission = new RolePermission();

		rolePermission.RolePermissionId.Should().NotBe(Guid.Empty);
	}
}