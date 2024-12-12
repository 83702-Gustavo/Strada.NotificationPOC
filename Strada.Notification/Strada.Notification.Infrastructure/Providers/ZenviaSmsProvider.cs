using System.Net.Http.Json;
using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Infrastructure.Providers;

public class ZenviaSmsProvider : INotificationProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _fromPhoneNumber;

    public ZenviaSmsProvider(HttpClient httpClient, string apiKey, string fromPhoneNumber)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _fromPhoneNumber = fromPhoneNumber ?? throw new ArgumentNullException(nameof(fromPhoneNumber));
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.Sms;
    }

    public async Task<Result> SendAsync(string recipient, string message)
    {
        try
        {
            var payload = new
            {
                from = _fromPhoneNumber,
                to = recipient,
                contents = new[]
                {
                    new { type = "text", text = message }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.zenvia.com/v2/channels/sms/messages")
            {
                Content = JsonContent.Create(payload)
            };

            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            var error = await response.Content.ReadAsStringAsync();
            return Result.Failure($"Zenvia SMS failed: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            return Result.Failure($"An unexpected error occurred while sending SMS via Zenvia: {ex.Message}");
        }
    }
}
