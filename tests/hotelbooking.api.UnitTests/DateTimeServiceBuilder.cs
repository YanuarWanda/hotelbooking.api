using hotelbooking.api.Core.Interfaces;
using hotelbooking.api.Infrastructure;

namespace hotelbooking.api.UnitTests;

public class InterfaceDateTimeServiceBuilder
{
    private readonly IDateTime _dateTime;

    public InterfaceDateTimeServiceBuilder()
    {
        _dateTime = new DateTimeService();
    }

    public IDateTime Build()
    {
        return _dateTime;
    }
}