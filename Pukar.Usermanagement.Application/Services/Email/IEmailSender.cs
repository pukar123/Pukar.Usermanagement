namespace Pukar.Usermanagement.Application.Services.Email;

/// <summary>Outbound email abstraction; implemented in Infrastructure (e.g. logging or SMTP).</summary>
public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
