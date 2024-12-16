using Microsoft.Extensions.Options;
using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Application.Settings;
using Strada.Notification.Domain.Enums;
using System.Net;
using System.Net.Mail;

namespace Strada.Notification.Infrastructure.Providers;

public class EmailProvider : INotificationProvider
{
    private readonly EmailSettings _settings;

    public EmailProvider(IOptions<EmailSettings> options)
    {
        _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.Email;
    }

    public async Task<Result> SendAsync(string recipient, string message)
    {
        try
        {
            using var smtpClient = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.SmtpUsername, _settings.SmtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.SmtpUsername, "Strada Notifications"),
                Subject = "Notification",
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(recipient);

            await smtpClient.SendMailAsync(mailMessage);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred while sending email: {ex.Message}");
        }
    }
}