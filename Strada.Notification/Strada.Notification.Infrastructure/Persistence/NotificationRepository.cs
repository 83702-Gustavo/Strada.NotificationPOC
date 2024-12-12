using Microsoft.EntityFrameworkCore;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Infrastructure.Persistence
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.Entities.Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Notification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Domain.Entities.Notification?> GetByIdAsync(Guid id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public Task<IEnumerable<Domain.Entities.Notification>> ConsultarNotificacoesAsync(string? destinatario = null, NotificationType? tipo = null, DateTime? dataInicio = null,
            DateTime? dataFim = null)
        {
            throw new NotImplementedException();
        }
    }
}