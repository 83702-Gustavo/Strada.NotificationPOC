using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Providers
{
    public class WhatsAppProvider : INotificationProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly string _phoneNumberId;

        public WhatsAppProvider(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;

            _accessToken = configuration["WhatsAppSettings:AccessToken"] 
                           ?? throw new ArgumentNullException("WhatsApp AccessToken is not configured.");
            
            _phoneNumberId = configuration["WhatsAppSettings:PhoneNumberId"] 
                             ?? throw new ArgumentNullException("WhatsApp PhoneNumberId is not configured.");
        }

        public bool CanHandle(TipoNotificacao type) => type == TipoNotificacao.WhatsApp;

        public async Task SendAsync(string recipient, string message)
        {
            try
            {
                var requestUrl = $"https://graph.facebook.com/v16.0/{_phoneNumberId}/messages";

                var requestBody = new
                {
                    messaging_product = "whatsapp",
                    to = recipient,
                    type = "text",
                    text = new
                    {
                        body = message
                    }
                };

                using var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Headers = { { "Authorization", $"Bearer {_accessToken}" } },
                    Content = JsonContent.Create(requestBody)
                };

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"Erro ao enviar WhatsApp: {error}");
                }

                Console.WriteLine($"WhatsApp enviado com sucesso para {recipient}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem pelo WhatsApp: {ex.Message}");
                throw;
            }
        }
    }
}