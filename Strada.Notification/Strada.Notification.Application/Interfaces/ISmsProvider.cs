using Strada.Notification.Application.Common;
using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Application.Interfaces;

public interface ISmsProvider
{
    string Name { get; }
    bool CanHandle(NotificationType type);
    Task<Result> SendSmsAsync(string recipient, string message);
}