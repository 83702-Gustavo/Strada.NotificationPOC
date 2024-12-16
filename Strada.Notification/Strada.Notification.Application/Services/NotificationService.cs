using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IEnumerable<INotificationProvider> _providers;
    private readonly INotificationRepository _repository;

    public NotificationService(
        IEnumerable<INotificationProvider> providers,
        INotificationRepository repository)
    {
        _providers = providers;
        _repository = repository;
    }

    public async Task<Result> SendNotificationAsync(NotificationType type, string recipient, string message)
    {
        var provider = _providers.FirstOrDefault(p => p.CanHandle(type));
        if (provider == null)
        {
            var notification = new Domain.Entities.Notification(recipient, message, type);
            notification.MarkAsFailed("No provider available for the specified notification type.");
            await _repository.AddAsync(notification);
            return Result.Failure("No provider available.");
        }

        var result = await provider.SendAsync(recipient, message);

        var notificationToSave = new Domain.Entities.Notification(recipient, message, type);
        if (result.IsSuccess)
        {
            notificationToSave.MarkAsSent();
        }
        else
        {
            notificationToSave.MarkAsFailed(result.ErrorMessage);
        }

        await _repository.AddAsync(notificationToSave);
        return result;
    }

    public async Task<IEnumerable<Domain.Entities.Notification>> GetNotificationsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Domain.Entities.Notification?> GetNotificationByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.Notification>> GetNotificationsByStatusAsync(NotificationStatus status)
    {
        return await _repository.GetByStatusAsync(status);
    }
}