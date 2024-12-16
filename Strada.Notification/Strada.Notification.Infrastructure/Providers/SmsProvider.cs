using Microsoft.Extensions.Logging;
using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Providers;

/// <summary>
/// Orquestrador para gerenciar o envio de mensagens SMS usando provedores com base na prioridade.
/// </summary>
public class SmsProvider : INotificationProvider
{
    private readonly IEnumerable<ISmsProvider> _smsProviders;
    private readonly IProviderSettingsRepository _settingsRepository;
    private readonly ILogger<SmsProvider> _logger;

    public SmsProvider(IEnumerable<ISmsProvider> smsProviders, IProviderSettingsRepository settingsRepository, ILogger<SmsProvider> logger)
    {
        _smsProviders = smsProviders ?? throw new ArgumentNullException(nameof(smsProviders));
        _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.Sms;
    }

    public async Task<Result> SendAsync(string recipient, string message)
    {
        // Recupera todos os provedores configurados e habilitados, ordenados por prioridade
        var settings = (await _settingsRepository.GetAllAsync())
            .Where(s => s.IsEnabled && _smsProviders.Any(p => p.Name == s.Name))
            .OrderBy(s => s.Priority)
            .ToList();

        if (!settings.Any())
        {
            _logger.LogWarning("No SMS providers are enabled or configured.");
            return Result.Failure("No SMS providers are available.");
        }

        foreach (var setting in settings)
        {
            var provider = _smsProviders.FirstOrDefault(p => p.Name == setting.Name);

            if (provider != null)
            {
                _logger.LogInformation($"Attempting to send SMS using provider '{provider.Name}' with priority {setting.Priority}.");

                try
                {
                    var result = await provider.SendSmsAsync(recipient, message);

                    if (result.IsSuccess)
                    {
                        _logger.LogInformation($"SMS sent successfully using provider '{provider.Name}'.");
                        return result;
                    }
                    else
                    {
                        _logger.LogWarning($"Provider '{provider.Name}' failed to send SMS: {result.ErrorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Exception occurred while sending SMS using provider '{provider.Name}'.");
                }
            }
        }

        _logger.LogError("All SMS providers failed to send the message.");
        return Result.Failure("All SMS providers failed.");
    }
}