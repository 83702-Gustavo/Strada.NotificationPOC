using Strada.Notification.Application.Common;
using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Application.Interfaces;

public interface INotificationProvider
{
    bool CanHandle(NotificationType type);

    Task<Result> SendAsync(string recipient, string message);
}