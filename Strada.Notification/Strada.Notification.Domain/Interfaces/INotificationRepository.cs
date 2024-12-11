using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Domain.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Entities.Notificacao notification);
    Task<IEnumerable<Entities.Notificacao>> GetAllAsync();
    Task<Entities.Notificacao?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    
    Task<IEnumerable<Notificacao>> ConsultarNotificacoesAsync(
        string? destinatario = null, 
        TipoNotificacao? tipo = null, 
        DateTime? dataInicio = null, 
        DateTime? dataFim = null);
}