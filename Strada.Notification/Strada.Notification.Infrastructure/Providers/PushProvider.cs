using Microsoft.Extensions.Options;
using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;
using System.Text;
using System.Text.Json;

namespace Strada.Notification.Infrastructure.Providers;

public class PushProvider : INotificationProvider
{
    private readonly PushSettings _settings;
    private readonly HttpClient _httpClient;

    public PushProvider(IOptions<PushSettings> options, HttpClient httpClient)
    {
        _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.Push;
    }

    public async Task<Result> SendAsync(string recipient, string message)
    {
        try
        {
            // Payload para envio da notificação
            var payload = new
            {
                to = recipient,
                notification = new
                {
                    title = "New Notification",
                    body = message
                }
            };

            // Serializa o payload para JSON
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Adiciona o cabeçalho de autorização
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settings.ApiKey);

            // Envia a requisição POST para o serviço de push
            var response = await _httpClient.PostAsync(_settings.ServiceUrl, content);

            // Verifica se a requisição foi bem-sucedida
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return Result.Failure($"Failed to send push notification: {errorMessage}");
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred while sending push notification: {ex.Message}");
        }
    }
}
