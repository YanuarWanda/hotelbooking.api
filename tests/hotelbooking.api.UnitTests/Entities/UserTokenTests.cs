using System;
using FluentAssertions;
using hotelbooking.api.Core.Entities;
using Xunit;

namespace hotelbooking.api.UnitTests.Entities;

public class UserTokenTests
{
	[Fact]
	public void UserTokenConstructorPropertyUserTokenIdShouldNotBeEmpty()
	{
		var entity = new UserToken();

		entity.UserTokenId.Should().NotBe(Guid.Empty);
	}
}