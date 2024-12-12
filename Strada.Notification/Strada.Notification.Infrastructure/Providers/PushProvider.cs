using Strada.Notification.Application.Common;
using Strada.Notification.Domain.Enums;
using System.Net.Http.Json;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Providers;

public class PushProvider : INotificationProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _pushServiceUrl;
    private readonly string _apiKey;

    public PushProvider(HttpClient httpClient, string pushServiceUrl, string apiKey)
    {
        _httpClient = httpClient;
        _pushServiceUrl = pushServiceUrl;
        _apiKey = apiKey;
    }

    public bool CanHandle(NotificationType type)
    {
        return type == NotificationType.Push;
    }

    public async Task<Result> SendAsync(string recipient, string message)
    {
        try
        {
            var payload = new
            {
                to = recipient,
                notification = new
                {
                    title = "Strada Notification",
                    body = message
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _pushServiceUrl)
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
            return Result.Failure($"Push notification failed: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            return Result.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }
}