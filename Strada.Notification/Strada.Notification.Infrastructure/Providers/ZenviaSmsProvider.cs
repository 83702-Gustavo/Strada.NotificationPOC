using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Providers;

/// <summary>
/// Provedor para envio de SMS usando a API da Zenvia.
/// </summary>
public class ZenviaSmsProvider : ISmsProvider
{
    private readonly IProviderSettingsRepository _settingsRepository;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ZenviaSmsProvider> _logger;

    public string Name => "Zenvia";

    public ZenviaSmsProvider(IProviderSettingsRepository settingsRepository, HttpClient httpClient, ILogger<ZenviaSmsProvider> logger)
    {
        _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.Sms;
    }

    public async Task<Result> SendSmsAsync(string recipient, string message)
    {
        // Recupera as configurações do provedor do repositório
        var settings = await _settingsRepository.GetByNameAsync(Name);
        if (settings == null || !settings.IsEnabled)
        {
            _logger.LogWarning("Zenvia provider is not enabled or configured.");
            return Result.Failure("Zenvia provider is not enabled or configured.");
        }

        if (string.IsNullOrWhiteSpace(settings.ApiKey))
        {
            _logger.LogError("Zenvia API key is missing.");
            return Result.Failure("Zenvia API key is missing.");
        }

        // Criação do payload para a API da Zenvia
        var payload = new
        {
            from = "YourSenderName", // Substituir pelo remetente configurado
            to = recipient,
            contents = new[] { new { type = "text", text = message } }
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Configuração da requisição
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ApiKey}");

        try
        {
            // Envia a requisição para a API da Zenvia
            var response = await _httpClient.PostAsync("https://api.zenvia.com/v2/channels/sms/messages", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Zenvia API responded with error: {response.StatusCode} - {errorContent}");
                return Result.Failure($"Zenvia API error: {response.StatusCode} - {errorContent}");
            }

            _logger.LogInformation("SMS sent successfully using Zenvia.");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while sending SMS via Zenvia.");
            return Result.Failure($"Zenvia exception: {ex.Message}");
        }
    }
}
