using Microsoft.EntityFrameworkCore;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Persistence;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(NotificationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(Domain.Entities.Notification notification)
    {
        if (notification == null)
            throw new ArgumentNullException(nameof(notification));

        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Domain.Entities.Notification notification)
    {
        if (notification == null)
            throw new ArgumentNullException(nameof(notification));

        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task<Domain.Entities.Notification?> GetByIdAsync(Guid id)
    {
        return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<Domain.Entities.Notification>> GetAllAsync()
    {
        return await _context.Notifications.ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Notification>> GetByStatusAsync(NotificationStatus status)
    {
        return await _context.Notifications
            .Where(n => n.Status == status)
            .ToListAsync();
    }
}