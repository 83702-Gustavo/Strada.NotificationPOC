using Microsoft.EntityFrameworkCore;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Interfaces;
using Strada.Notification.Infrastructure.Persistence;

namespace Strada.Notification.Infrastructure.Repositories;

public class ProviderSettingsRepository : IProviderSettingsRepository
{
    private readonly NotificationDbContext _dbContext;

    public ProviderSettingsRepository(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<ProviderSettings>> GetAllAsync()
    {
        return await _dbContext.ProviderSettings.ToListAsync();
    }
    
    public async Task<ProviderSettings> GetByNameAsync(string name)
    {
        return (await _dbContext.ProviderSettings.FirstOrDefaultAsync(p => p.Name == name))!;
    }
    
    public async Task AddAsync(ProviderSettings providerSetting)
    {
        await _dbContext.ProviderSettings.AddAsync(providerSetting);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(ProviderSettings providerSetting)
    {
        _dbContext.ProviderSettings.Update(providerSetting);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(ProviderSettings providerSetting)
    {
        _dbContext.ProviderSettings.Remove(providerSetting);
        await _dbContext.SaveChangesAsync();
    }
}