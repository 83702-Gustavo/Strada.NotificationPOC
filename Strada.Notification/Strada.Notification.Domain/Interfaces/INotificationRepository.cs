using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Domain.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Entities.Notification notification);
    Task<IEnumerable<Entities.Notification>> GetAllAsync();
    Task<Entities.Notification?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    
    Task<IEnumerable<Entities.Notification>> ConsultarNotificacoesAsync(
        string? destinatario = null, 
        NotificationType? tipo = null, 
        DateTime? dataInicio = null, 
        DateTime? dataFim = null);
}