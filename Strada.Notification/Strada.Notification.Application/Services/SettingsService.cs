using Strada.Notification.Application.DTOs;
using Strada.Notification.Domain.Entities;

namespace Strada.Notification.Application.Services;

public class SettingsService
{
    private readonly List<Provider> _providers; 
    private ConfigurationLimits _limits; 
    private SecuritySettings _securitySettings; 

    public SettingsService()
    {
        _providers = new List<Provider>
        {
            new Provider("Twilio", "SMS", true, 1, "admin"),
            new Provider("Mailgun", "Email", true, 1, "admin"),
            new Provider("Zenvia", "SMS", false, 2, "admin"),
            new Provider("OneSignal", "Push", true, 1, "admin")
        };

        _limits = new ConfigurationLimits(500, 10000, "daily");
        _securitySettings = new SecuritySettings(60, true, "Error");
    }
    
    public IEnumerable<Provider> GetProviders()
    {
        return _providers;
    }

    public Provider GetProviderByName(string name)
    {
        var provider = _providers.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (provider == null)
            throw new ArgumentException($"Provider '{name}' not found.");
        return provider;
    }

    public void ActivateProvider(string name)
    {
        var provider = GetProviderByName(name);
        provider.UpdateStatus(true);
    }

    public void DeactivateProvider(string name)
    {
        var provider = GetProviderByName(name);
        provider.UpdateStatus(false);
    }

    public void UpdateProviderPriority(string name, int priority)
    {
        var provider = GetProviderByName(name);
        provider.UpdatePriority(priority);
    }
    
    public ConfigurationLimits GetLimits()
    {
        return _limits;
    }

    public void UpdateLimits(UpdateLimitsDto dto)
    {
        if (dto.ApplicationLimit <= 0 || dto.ProviderLimit <= 0)
            throw new ArgumentException("Limits must be greater than zero.");

        _limits = new ConfigurationLimits(dto.ApplicationLimit, dto.ProviderLimit, dto.Interval);
    }

    public SecuritySettings GetSecuritySettings()
    {
        return _securitySettings;
    }

    public void UpdateSecuritySettings(UpdateSecurityDto dto)
    {
        if (dto.JwtExpirationTime <= 0)
            throw new ArgumentException("JWT expiration time must be greater than zero.");

        if (string.IsNullOrWhiteSpace(dto.LogLevel))
            throw new ArgumentException("Log level cannot be empty.");

        _securitySettings = new SecuritySettings(dto.JwtExpirationTime, dto.LogElasticSearch, dto.LogLevel);
    }

    public void AddProvider(Provider newProvider)
    {
        if (_providers.Any(p => p.Name.Equals(newProvider.Name, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException($"Provider '{newProvider.Name}' already exists.");

        _providers.Add(newProvider);
    }

    public void RemoveProvider(string name)
    {
        var provider = GetProviderByName(name);
        _providers.Remove(provider);
    }

    public Dictionary<string, object> GetGeneralSettings()
    {
        return new Dictionary<string, object>
        {
            { "ActiveProviders", _providers.Count(p => p.IsActive) },
            { "ApplicationLimit", _limits.ApplicationLimit },
            { "ProviderLimit", _limits.ProviderLimit },
            { "LimitInterval", _limits.Interval },
            { "JwtExpirationTime", _securitySettings.JwtExpirationTime },
            { "LogElasticSearch", _securitySettings.LogElasticSearch },
            { "LogLevel", _securitySettings.LogLevel }
        };
    }
}