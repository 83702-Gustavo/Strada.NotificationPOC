using Strada.Notification.Domain.Enums;

namespace Strada.Notification.API.DTOs.Request;

public class SendNotificationDto
{
    public NotificationType Type { get; set; }
    public string Recipient { get; set; }
    public string Message { get; set; }
}