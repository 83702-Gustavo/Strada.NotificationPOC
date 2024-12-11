using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Providers;

public class ZenviaSmsProvider : INotificationProvider
{
    public bool CanHandle(TipoNotificacao type) => type == TipoNotificacao.Sms;

    public async Task SendAsync(string recipient, string message)
    {
        // Lógica para envio via Zenvia
        Console.WriteLine($"[Zenvia] SMS enviado para {recipient}: {message}");
        await Task.CompletedTask;
    }
}