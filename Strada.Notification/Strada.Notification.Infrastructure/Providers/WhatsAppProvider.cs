using Microsoft.Extensions.Options;
using Strada.Notification.Application.Settings;
using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Infrastructure.Providers;

public class WhatsAppProvider : INotificationProvider
{
    private readonly WhatsAppSettings _settings;

    public WhatsAppProvider(IOptions<WhatsAppSettings> options)
    {
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.WhatsApp;
    }

    public async Task<Result> SendAsync(string recipient, string message)
    {
        try
        {
            // Aqui você usará o _settings para configurar e enviar a mensagem via WhatsApp
            // Exemplo:
            // Inicializar o cliente usando _settings.AccountSid e _settings.AuthToken

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred while sending WhatsApp message: {ex.Message}");
        }
    }
}