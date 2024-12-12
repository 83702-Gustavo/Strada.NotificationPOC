using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; } 
    public string Recipient { get; private set; } 
    public string Message { get; private set; } 
    public NotificationType Type { get; private set; } 
    public string Provider { get; private set; } 
    public DateTime CreatedAt { get; private set; } 
    public DateTime? DeliveredAt { get; private set; } 
    public string Status { get; private set; } 

    public Notification(string recipient, string message, NotificationType type, string provider)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty.");

        if (message.Length > 500)
            throw new ArgumentException("Message cannot exceed 500 characters.");

        Id = Guid.NewGuid();
        Recipient = recipient;
        Message = message;
        Type = type;
        Provider = provider;
        CreatedAt = DateTime.UtcNow;
        Status = "Pending"; 
    }

    public void MarkAsDelivered()
    {
        DeliveredAt = DateTime.UtcNow;
        Status = "Delivered";
    }

    public void MarkAsFailed(string errorMessage)
    {
        DeliveredAt = DateTime.UtcNow;
        Status = $"Failed: {errorMessage}";
    }

    public override string ToString()
    {
        return $"Notification [Id={Id}, Recipient={Recipient}, Type={Type}, Status={Status}, CreatedAt={CreatedAt}, DeliveredAt={DeliveredAt}]";
    }
}