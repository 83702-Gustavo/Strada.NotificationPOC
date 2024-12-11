using Strada.Notification.Domain.Enums;

namespace Strada.Notification.API.DTOs;

public class NotificacaoRequest
{
    public TipoNotificacao Type { get; set; }
    public string Recipient { get; set; }
    public string Message { get; set; }
}