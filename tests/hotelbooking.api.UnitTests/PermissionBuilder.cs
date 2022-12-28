using System;
using hotelbooking.api.Core.Entities;

namespace hotelbooking.api.UnitTests;

public class PermissionBuilder
{
	private readonly Permission _permission = new();

	public PermissionBuilder Id(Guid id)
	{
		_permission.PermissionId = id;
		return this;
	}

	public PermissionBuilder WithDefaultValues()
	{
		_permission.Name = "users.readwrite";
		return this;
	}

	public Permission Build()
	{
		return _permission;
	}
}