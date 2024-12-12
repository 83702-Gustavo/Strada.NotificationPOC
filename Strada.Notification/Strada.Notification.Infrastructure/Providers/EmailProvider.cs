using System.Net;
using System.Net.Mail;
using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Providers;

public class EmailProvider : INotificationProvider
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public EmailProvider(string smtpHost, int smtpPort, string smtpUsername, string smtpPassword)
    {
        _smtpHost = smtpHost;
        _smtpPort = smtpPort;
        _smtpUsername = smtpUsername;
        _smtpPassword = smtpPassword;
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.Email;
    }

    public async Task<Result> SendAsync(string recipient, string message)
    {
        try
        {
            using var smtpClient = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername, "Strada Notifications"),
                Subject = "Notification",
                Body = message,
                IsBodyHtml = false
            };

            mailMessage.To.Add(recipient);

            await smtpClient.SendMailAsync(mailMessage);

            return Result.Success();
        }
        catch (SmtpException ex)
        {
            return Result.Failure($"SMTP error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }
}