using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Domain.Entities;

public class Notification : BaseEntity
{
    public string Recipient { get; private set; }
    public string Message { get; private set; }
    public NotificationType Type { get; private set; }
    public string Provider { get; private set; } 
    public NotificationStatus Status { get; private set; }
    public string? ErrorMessage { get; private set; }
    public DateTime? DeliveredAt { get; private set; } 

    public Notification(string recipient, string message, NotificationType type) : base()
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty.");

        if (message.Length > 500)
            throw new ArgumentException("Message cannot exceed 500 characters.");

        Recipient = recipient;
        Message = message;
        Type = type;
        Status = NotificationStatus.Pending;
        ErrorMessage = null;
        DeliveredAt = null;
    }

    public void MarkAsSent()
    {
        Status = NotificationStatus.Sent;
        ErrorMessage = null;
    }

    public void MarkAsFailed(string error)
    {
        Status = NotificationStatus.Failed;
        ErrorMessage = error;
    }
    
    public void MarkAsDelivered()
    {
        if (Status != NotificationStatus.Sent)
            throw new InvalidOperationException("Notification must be sent before marking as delivered.");

        DeliveredAt = DateTime.UtcNow;
    }
}