using System;
using System.Threading;
using Moq;
using hotelbooking.api.Core.Entities;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.UnitTests;

public class PermissionServiceBuilder
{
	private readonly Mock<IPermissionService> _mock;

	public PermissionServiceBuilder()
	{
		_mock = new Mock<IPermissionService>();
		_enableAllParam = false;
	}

	private bool _enableAllParam;

	public PermissionServiceBuilder GetByIdAsync(Guid userId, Permission? callbackResult)
	{
		if (_enableAllParam)
			throw new InvalidOperationException("Can not proceed because of GetByIdAllParams invoked");

		string s = userId.ToString();

		_mock.Setup(e => e.GetByIdAsync(s, It.IsAny<CancellationToken>()))
			.ReturnsAsync(callbackResult);

		return this;
	}

	public PermissionServiceBuilder GetByIdAllParams()
	{
		_enableAllParam = true;

		_mock.Setup(e => e.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Permission());

		return this;
	}

	public Mock<IPermissionService> Build()
	{
		return _mock;
	}
}