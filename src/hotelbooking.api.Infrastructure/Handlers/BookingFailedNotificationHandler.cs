using MediatR;
using Microsoft.Extensions.Logging;
using hotelbooking.api.Core.Events;

namespace hotelbooking.api.Infrastructure.Handlers;

public class BookingFailedNotificationHandler : INotificationHandler<BookingFailedEvent>
{
	private readonly ILogger<BookingFailedNotificationHandler>? _logger;

	public BookingFailedNotificationHandler(ILogger<BookingFailedNotificationHandler>? logger)
	{
		_logger = logger;
	}

	public Task Handle(BookingFailedEvent notification, CancellationToken cancellationToken)
	{
		_logger?.LogInformation($"Event {nameof(BookingFailedEvent)} fired");

		return Task.CompletedTask;
	}
}