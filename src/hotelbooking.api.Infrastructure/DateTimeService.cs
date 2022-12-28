using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.Infrastructure;

public class DateTimeService : IDateTime
{
    public DateTimeService()
    {
        ScopedNow = DateTime.Now;
        ScopedUtcNow = DateTime.UtcNow;
    }

    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime ScopedNow { get; }
    public DateTime ScopedUtcNow { get; }
}