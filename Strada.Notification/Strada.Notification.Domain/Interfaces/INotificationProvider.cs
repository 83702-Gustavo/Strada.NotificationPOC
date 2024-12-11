using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Domain.Interfaces;

public interface INotificationProvider
{
    /// <summary>
    /// Verifica se o fornecedor pode lidar com o tipo de notificação.
    /// </summary>
    bool CanHandle(TipoNotificacao type);

    /// <summary>
    /// Envia a notificação.
    /// </summary>
    Task SendAsync(string recipient, string message);
}