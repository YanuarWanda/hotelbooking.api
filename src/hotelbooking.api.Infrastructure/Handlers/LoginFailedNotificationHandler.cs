using MediatR;
using Microsoft.Extensions.Logging;
using hotelbooking.api.Core.Events;

namespace hotelbooking.api.Infrastructure.Handlers;

public class LoginFailedNotificationHandler : INotificationHandler<LoginFailedEvent>
{
	private readonly ILogger<LoginFailedNotificationHandler>? _logger;

	public LoginFailedNotificationHandler(ILogger<LoginFailedNotificationHandler>? logger)
	{
		_logger = logger;
	}

	public Task Handle(LoginFailedEvent notification, CancellationToken cancellationToken)
	{
		_logger?.LogInformation($"Event {nameof(LoginFailedEvent)} fired");

		return Task.CompletedTask;
	}
}