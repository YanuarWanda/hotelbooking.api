using Microsoft.Extensions.Logging;
using hotelbooking.api.Core.Interfaces;

namespace hotelbooking.api.Infrastructure;

public class SmtpEmailSender : IEmailSender
{
    private readonly ILogger<SmtpEmailSender>? _logger;

    public SmtpEmailSender(ILogger<SmtpEmailSender>? logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string @from, string subject, string body)
    {
        _logger?.LogInformation($"Sending email to {to}");

        return Task.CompletedTask;
    }
}