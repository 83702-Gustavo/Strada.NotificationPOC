using Strada.Notification.Domain.Entities;

namespace Strada.Notification.Application.Interfaces;

public interface IProviderSettingsService
{
    Task<IEnumerable<ProviderSettings>> GetAllAsync();

    Task<ProviderSettings?> GetByNameAsync(string Name);

    Task UpdateSettingsAsync(string Name, bool isEnabled, int priority, string? apiKey = null);
}