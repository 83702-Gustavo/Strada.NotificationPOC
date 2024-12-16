using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Domain.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Entities.Notification notification);
    Task UpdateAsync(Entities.Notification notification);
    Task<Entities.Notification?> GetByIdAsync(Guid id);
    Task<IEnumerable<Entities.Notification>> GetAllAsync();
    Task<IEnumerable<Entities.Notification>> GetByStatusAsync(NotificationStatus status);
}