using Microsoft.Extensions.Logging;
using Pukar.Usermanagement.Application.Services.Email;

namespace Pukar.Usermanagement.Infrastructure.Services;

/// <summary>Development-friendly email implementation; replace with SMTP or a provider in production hosts.</summary>
public sealed class LoggingEmailSender : IEmailSender
{
    private readonly ILogger<LoggingEmailSender> _logger;

    public LoggingEmailSender(ILogger<LoggingEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Email to {To} — {Subject}{NewLine}{Body}",
            to,
            subject,
            Environment.NewLine,
            body);
        return Task.CompletedTask;
    }
}
