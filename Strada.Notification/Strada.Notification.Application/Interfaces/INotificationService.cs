using Strada.Notification.Domain.Enums;
using Strada.Notification.Application.Common;

namespace Strada.Notification.Application.Interfaces;

public interface INotificationService
{
    Task<Result> SendNotificationAsync(NotificationType type, string recipient, string message);

    Task<IEnumerable<Domain.Entities.Notification>> GetNotificationsAsync();
}