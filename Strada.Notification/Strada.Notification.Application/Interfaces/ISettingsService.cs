using Strada.Notification.Application.Common;
using Strada.Notification.Application.DTOs;

namespace Strada.Notification.Application.Interfaces;

public interface ISettingsService
{
    Task<List<ProviderSettingsDto>> GetAllSettingsAsync();
    Task<Result> UpdateProviderSettingsAsync(UpdateProviderSettingsDto updateDto);
    Task<Result> AddProviderSettingsAsync(CreateProviderSettingsDto createDto);
    Task<Result> DeleteProviderSettingsAsync(string name);
}