using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Providers;
public class EmailProvider : INotificationProvider
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;

    public EmailProvider(IConfiguration configuration)
    {
        _smtpServer = configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentNullException("SMTP Server not configured.");
        _port = int.Parse(configuration["EmailSettings:Port"] ?? "587");
        _username = configuration["EmailSettings:Username"] ?? throw new ArgumentNullException("SMTP Username not configured.");
        _password = configuration["EmailSettings:Password"] ?? throw new ArgumentNullException("SMTP Password not configured.");
    }

    public bool CanHandle(TipoNotificacao type) => type == TipoNotificacao.Email;

    public async Task SendAsync(string recipient, string message)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Strada Notification", _username));
            emailMessage.To.Add(new MailboxAddress("", recipient));
            emailMessage.Subject = "Nova Notificação";
            emailMessage.Body = new TextPart("plain") { Text = message };

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_username, _password);
            await smtpClient.SendAsync(emailMessage);
            await smtpClient.DisconnectAsync(true);

            Console.WriteLine($"Email enviado com sucesso para {recipient}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar email: {ex.Message}");
            throw; 
        }
    }
}