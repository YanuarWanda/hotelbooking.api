using System;
using FluentAssertions;
using hotelbooking.api.Core.Entities;
using Xunit;

namespace hotelbooking.api.UnitTests.Entities;

public class PermissionTests
{
	[Fact]
	public void PermissionConstuctorShouldCorrect()
	{
		var permission = new Permission();

		permission.PermissionId.Should().NotBe(Guid.Empty);
	}
}