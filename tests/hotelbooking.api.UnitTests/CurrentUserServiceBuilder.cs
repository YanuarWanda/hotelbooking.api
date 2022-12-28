using System;
using Moq;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.UnitTests;

public class InterfaceCurrentUserServiceBuilder
{
    private readonly Mock<ICurrentUserService> _mock;

    public InterfaceCurrentUserServiceBuilder()
    {
        _mock = new Mock<ICurrentUserService>();
    }

    public InterfaceCurrentUserServiceBuilder SetupUserId(string userId)
    {
        _mock.Setup(e => e.UserId).Returns(userId);
        return this;
    }

    public InterfaceCurrentUserServiceBuilder SetupUserId(Guid userId)
    {
        _mock.Setup(e => e.UserId).Returns(userId.ToString);
        return this;
    }

    public InterfaceCurrentUserServiceBuilder SetupIdt(string idt)
    {
        _mock.Setup(e => e.Idt).Returns(idt);
        return this;
    }

    public Mock<ICurrentUserService> Build()
    {
        return _mock;
    }
}