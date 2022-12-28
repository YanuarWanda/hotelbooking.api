using System;
using FluentAssertions;
using hotelbooking.api.Core.Entities;
using Xunit;

namespace hotelbooking.api.UnitTests.Entities;

public class UserLoginTests
{
	[Fact]
	public void UserLoginConstructEntityPropertyUserLoginIdShouldNotBeEmpty()
	{
		var entity = new UserLogin();

		entity.UserLoginId.Should().NotBe(Guid.Empty);
	}
}