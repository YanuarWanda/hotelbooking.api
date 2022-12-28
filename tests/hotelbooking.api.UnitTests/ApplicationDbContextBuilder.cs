using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;
using Moq;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.UnitTests;

public class InterfaceApplicationDbContextBuilder
{
	private readonly Mock<IApplicationDbContext> _mockDbContext;

	public InterfaceApplicationDbContextBuilder()
	{
		_mockDbContext = new Mock<IApplicationDbContext>();
	}

	public InterfaceApplicationDbContextBuilder SetupUser(List<User> users)
	{
		_mockDbContext.Setup(e => e.Users).Returns(users.AsQueryable().BuildMockDbSet().Object);
		return this;
	}

	public InterfaceApplicationDbContextBuilder SetupUserToken(List<UserToken> userTokens)
	{
		_mockDbContext.Setup(e => e.UserTokens).Returns(userTokens.AsQueryable().BuildMockDbSet().Object);
		return this;
	}

	public InterfaceApplicationDbContextBuilder SetupRole()
	{
		_mockDbContext.Setup(e => e.Roles).Returns(new List<Role>().AsQueryable().BuildMockDbSet().Object);
		return this;
	}

	public InterfaceApplicationDbContextBuilder SetupRole(List<Role> roles)
	{
		_mockDbContext.Setup(e => e.Roles).Returns(roles.AsQueryable().BuildMockDbSet().Object);
		return this;
	}

	public Mock<IApplicationDbContext> Build()
	{
		return _mockDbContext;
	}
}