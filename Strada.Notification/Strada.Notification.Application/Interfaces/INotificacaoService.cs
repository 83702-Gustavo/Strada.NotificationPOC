using Strada.Notification.Application.Common;
using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Application.Interfaces;

public interface INotificacaoService
{
    Task<Result> EnviaNotificacaoAsync(TipoNotificacao type, string recipient, string message);
}