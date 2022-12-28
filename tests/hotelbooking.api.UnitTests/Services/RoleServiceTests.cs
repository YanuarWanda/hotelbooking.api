using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Infrastructure.Services;
using Xunit;

namespace hotelbooking.api.UnitTests.Services;

public class RoleServiceTests
{
	[Fact]
	public void GetRoleByIdShouldBeNull()
	{
		var fakeApplicationDbContextBuilder = new InterfaceApplicationDbContextBuilder();
		fakeApplicationDbContextBuilder.SetupRole();
		var fakeApplicationDbContext = fakeApplicationDbContextBuilder.Build();

		var service = new RoleService(fakeApplicationDbContext.Object);

		var result = service.GetByIdAsync(Guid.NewGuid().ToString(), CancellationToken.None).GetAwaiter().GetResult();

		result.Should().BeNull();
	}

	[Fact]
	public void GetRoleByIdShouldBeNotNull()
	{
		var fakeApplicationDbContextBuilder = new InterfaceApplicationDbContextBuilder();
		var fakeRole = new RoleBuilder().WithDefaultValues().Build();
		fakeApplicationDbContextBuilder.SetupRole(new List<Role> {fakeRole});
		var fakeApplicationDbContext = fakeApplicationDbContextBuilder.Build();

		var service = new RoleService(fakeApplicationDbContext.Object);

		var result = service.GetByIdAsync(fakeRole.RoleId.ToString(), CancellationToken.None).GetAwaiter().GetResult();

		result.Should().NotBeNull();
	}

	[Fact]
	public void IsNameExistsShouldBeFalse()
	{
		var fakeApplicationDbContextBuilder = new InterfaceApplicationDbContextBuilder();
		fakeApplicationDbContextBuilder.SetupRole();
		var fakeApplicationDbContext = fakeApplicationDbContextBuilder.Build();

		var service = new RoleService(fakeApplicationDbContext.Object);

		var result = service.IsNameExists("abcdefghij", CancellationToken.None).GetAwaiter().GetResult();

		result.Should().BeFalse();
	}

	public static IEnumerable<object[]> TestIsNameExistsCorrectData() =>
		new List<object[]>
		{
			new object[] {"Name using all lowercase", "test"},
			new object[] {"Normal name", "Test"},
			new object[] {"Fixed name", "tEsT"},
			new object[] {"All uppercase", "TEST"}
		};

	[Theory]
	[MemberData(nameof(TestIsNameExistsCorrectData))]
	public void IsNameExistsShouldBeTrue(string message, string value)
	{
		var fakeApplicationDbContextBuilder = new InterfaceApplicationDbContextBuilder();
		var fakeRole = new RoleBuilder().WithDefaultValues().Name(value).Build();
		fakeApplicationDbContextBuilder.SetupRole(new List<Role> {fakeRole});
		var fakeApplicationDbContext = fakeApplicationDbContextBuilder.Build();

		var service = new RoleService(fakeApplicationDbContext.Object);

		var result = service.IsNameExists(value, CancellationToken.None).GetAwaiter().GetResult();

		result.Should().BeTrue(message);
	}
}