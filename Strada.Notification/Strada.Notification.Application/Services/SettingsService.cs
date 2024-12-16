using Strada.Notification.Application.Common;
using Strada.Notification.Application.DTOs;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Entities;
using Strada.Notification.Domain.Interfaces;

namespace Strada.Notification.Application.Services;

public class SettingsService : ISettingsService
{
    private readonly IProviderSettingsRepository _providerSettingsRepository;

    public SettingsService(IProviderSettingsRepository providerSettingsRepository)
    {
        _providerSettingsRepository = providerSettingsRepository;
    }

    public async Task<List<ProviderSettingsDto>> GetAllSettingsAsync()
    {
        var settings = await _providerSettingsRepository.GetAllAsync();

        return settings.Select(s => new ProviderSettingsDto
        {
            Name = s.Name,
            Enabled = s.IsEnabled,
            Priority = s.Priority,
            ApiKey = s.ApiKey
        }).ToList();
    }
    
    public async Task<Result> UpdateProviderSettingsAsync(UpdateProviderSettingsDto updateDto)
    {
        var existingSetting = await _providerSettingsRepository.GetByNameAsync(updateDto.Name);
        if (existingSetting == null)
        {
            return Result.Failure("Provider not found.");
        }
        
        existingSetting.UpdateSettings(updateDto.Enabled, updateDto.Priority, updateDto.ApiKey);
        
        await _providerSettingsRepository.UpdateAsync(existingSetting);

        return Result.Success();
    }
    
    public async Task<Result> AddProviderSettingsAsync(CreateProviderSettingsDto createDto)
    {
        var existingSetting = await _providerSettingsRepository.GetByNameAsync(createDto.Name);
        if (existingSetting != null)
        {
            return Result.Failure("Provider already exists.");
        }
        
        var newSetting = new ProviderSettings(createDto.Name, createDto.Enabled, createDto.Priority, createDto.ApiKey);
        
        await _providerSettingsRepository.AddAsync(newSetting);

        return Result.Success();
    }
    
    public async Task<Result> DeleteProviderSettingsAsync(string name)
    {
        var existingSetting = await _providerSettingsRepository.GetByNameAsync(name);
        if (existingSetting == null)
        {
            return Result.Failure("Provider not found.");
        }

        await _providerSettingsRepository.DeleteAsync(existingSetting);

        return Result.Success();
    }
}