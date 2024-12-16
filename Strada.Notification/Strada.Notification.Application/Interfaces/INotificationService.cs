using Strada.Notification.Application.Common;
using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Application.Interfaces;

public interface INotificationService
{
    Task<Result> SendNotificationAsync(NotificationType type, string recipient, string message);
    Task<IEnumerable<Domain.Entities.Notification>> GetNotificationsAsync();
    Task<Domain.Entities.Notification?> GetNotificationByIdAsync(Guid id);
    Task<IEnumerable<Domain.Entities.Notification>> GetNotificationsByStatusAsync(NotificationStatus status);
}