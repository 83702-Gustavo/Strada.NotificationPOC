using Strada.Notification.Domain.Entities;

namespace Strada.Notification.Domain.Interfaces;

public interface IProviderSettingsRepository
{
    Task<List<ProviderSettings>> GetAllAsync();
    Task<ProviderSettings> GetByNameAsync(string name);
    Task AddAsync(ProviderSettings providerSetting);
    Task UpdateAsync(ProviderSettings providerSetting);
    Task DeleteAsync(ProviderSettings providerSetting);
}