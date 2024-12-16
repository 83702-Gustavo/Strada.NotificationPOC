using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Strada.Notification.Infrastructure.Providers;

public class TwilioSmsProvider : ISmsProvider
{
    private readonly IProviderSettingsRepository _settingsRepository;

    public string Name => "Twilio";

    public TwilioSmsProvider(IProviderSettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.Sms;
    }

    public async Task<Result> SendSmsAsync(string recipient, string message)
    {
        // Busca as configurações do provedor no repositório
        var settings = await _settingsRepository.GetByNameAsync(Name);
        if (settings == null || !settings.IsEnabled)
        {
            return Result.Failure("Twilio provider is not enabled or configured.");
        }

        if (string.IsNullOrWhiteSpace(settings.ApiKey))
        {
            return Result.Failure("Twilio API key is missing.");
        }

        try
        {
            TwilioClient.Init(settings.ApiKey, settings.ApiKey); // Ajuste se necessário para AccountSid e AuthToken
            
            var messageResponse = await MessageResource.CreateAsync(
                to: new Twilio.Types.PhoneNumber(recipient),
                from: new Twilio.Types.PhoneNumber("+1234567890"), // Número de envio (ajustar conforme configuração futura)
                body: message
            );
            
            if (messageResponse.Status == MessageResource.StatusEnum.Failed ||
                messageResponse.Status == MessageResource.StatusEnum.Undelivered)
            {
                return Result.Failure($"Twilio failed to send message: {messageResponse.ErrorMessage}");
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Twilio exception: {ex.Message}");
        }
    }
}
