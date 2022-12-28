using MediatR;
using Microsoft.Extensions.Logging;
using hotelbooking.api.Core.Events;

namespace hotelbooking.api.Infrastructure.Handlers;

public class LoginSuccessNotificationHandler : INotificationHandler<LoginSuccessEvent>
{
	private readonly ILogger<LoginSuccessNotificationHandler>? _logger;

	public LoginSuccessNotificationHandler(ILogger<LoginSuccessNotificationHandler>? logger)
	{
		_logger = logger;
	}

	public Task Handle(LoginSuccessEvent notification, CancellationToken cancellationToken)
	{
		_logger?.LogInformation($"Event {nameof(LoginSuccessEvent)} fired");

		return Task.CompletedTask;
	}
}