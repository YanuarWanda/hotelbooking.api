using System;
using System.Threading;
using Moq;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.UnitTests;

public class RoleServiceBuilder
{
	private readonly Mock<IRoleService> _mock;

	public RoleServiceBuilder()
	{
		_mock = new Mock<IRoleService>();
	}

	public RoleServiceBuilder GetByIdAsync(Guid roleId, Role? returnValue)
	{
		_mock.Setup(e => e.GetByIdAsync(roleId.ToString(), It.IsAny<CancellationToken>())).ReturnsAsync(returnValue);
		return this;
	}

	public RoleServiceBuilder GetByIdAsync(string roleId, Role? returnValue)
	{
		_mock.Setup(e => e.GetByIdAsync(roleId, It.IsAny<CancellationToken>())).ReturnsAsync(returnValue);
		return this;
	}

	public RoleServiceBuilder IsNameExists(string name, bool returnValue)
	{
		_mock.Setup(e => e.IsNameExists(name, It.IsAny<CancellationToken>())).ReturnsAsync(returnValue);
		return this;
	}

	public RoleServiceBuilder GetByIdAllParams()
	{
		_mock.Setup(e => e.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Role());
		return this;
	}

	public Mock<IRoleService> Build()
	{
		return _mock;
	}
}