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
            return Result.Failure($"No provider found for notification type '{type}'.");
        }

        try
        {
            var sendResult = await provider.SendAsync(recipient, message);

            if (!sendResult.IsSuccess)
            {
                return Result.Failure($"Notification failed: {sendResult.ErrorMessage}");
            }

            var notification = new Domain.Entities.Notification(recipient, message, type, provider.GetType().Name);
            await _repository.AddAsync(notification);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred while sending the notification: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Domain.Entities.Notification>> GetNotificationsAsync()
    {
        return await _repository.GetAllAsync();
    }
}