using MediatR;
using Microsoft.Extensions.Logging;
using hotelbooking.api.Core.Events;

namespace hotelbooking.api.Infrastructure.Handlers;

public class BookingSuccessNotificationHandler : INotificationHandler<BookingSuccessEvent>
{
	private readonly ILogger<BookingSuccessNotificationHandler>? _logger;

	public BookingSuccessNotificationHandler(ILogger<BookingSuccessNotificationHandler>? logger)
	{
		_logger = logger;
	}

	public Task Handle(BookingSuccessEvent notification, CancellationToken cancellationToken)
	{
		_logger?.LogInformation($"Event {nameof(BookingSuccessEvent)} fired");

		return Task.CompletedTask;
	}
}