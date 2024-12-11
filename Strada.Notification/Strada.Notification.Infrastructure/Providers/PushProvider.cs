using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Providers
{
    public class PushProvider : INotificationProvider
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        public PushProvider(IConfiguration configuration)
        {
            // Inicializa o Firebase com as credenciais
            var firebaseCredentialPath = configuration["PushSettings:FirebaseCredentialPath"];
            if (string.IsNullOrWhiteSpace(firebaseCredentialPath))
            {
                throw new ArgumentException("Firebase credentials path is not configured.");
            }

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(firebaseCredentialPath)
            });

            _firebaseMessaging = FirebaseMessaging.DefaultInstance;
        }

        public bool CanHandle(TipoNotificacao type) => type == TipoNotificacao.Push;

        public async Task SendAsync(string recipient, string message)
        {
            try
            {
                var notification = new Message
                {
                    Token = recipient, 
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = "Nova Notificação",
                        Body = message
                    }
                };
                
                var response = await _firebaseMessaging.SendAsync(notification);
                Console.WriteLine($"Push enviado com sucesso. ID: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar push: {ex.Message}");
                throw; 
            }
        }
    }
}