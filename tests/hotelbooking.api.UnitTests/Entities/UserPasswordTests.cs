using System;
using FluentAssertions;
using hotelbooking.api.Core.Entities;
using Xunit;

namespace hotelbooking.api.UnitTests.Entities;

public class UserPasswordTests
{
	[Fact]
	public void UserPasswordConstructorEntityPropertyUserPasswordIdShouldNotBeEmpty()
	{
		var entity = new UserPassword();

		entity.UserPasswordId.Should().NotBe(Guid.Empty);
	}
}