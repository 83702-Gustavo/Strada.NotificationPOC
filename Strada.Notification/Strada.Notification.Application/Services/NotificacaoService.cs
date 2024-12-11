using Strada.Notification.Application.Common;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Application.Services;
public class NotificacaoService : INotificacaoService
{
    private readonly IEnumerable<INotificationProvider> _providers;
    private readonly INotificationRepository _repository;

    public NotificacaoService(IEnumerable<INotificationProvider> providers, INotificationRepository repository)
    {
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result> EnviaNotificacaoAsync(TipoNotificacao type, string recipient, string message)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            return Result.Failure("Recipient cannot be null or empty.");

        if (string.IsNullOrWhiteSpace(message))
            return Result.Failure("Message cannot be null or empty.");

        var applicableProviders = _providers.Where(p => p.CanHandle(type)).ToList();
        if (!applicableProviders.Any())
            return Result.Failure($"No providers available for notification type '{type}'.");

        foreach (var provider in applicableProviders)
        {
            try
            {
                await provider.SendAsync(recipient, message);
                
                var notification = new Domain.Entities.Notificacao(recipient, message, type);
                await _repository.AddAsync(notification);

                return Result.Success();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar com o provedor {provider.GetType().Name}: {ex.Message}");
            }
        }

        return Result.Failure($"Failed to send notification of type '{type}' to {recipient}.");
    }
}