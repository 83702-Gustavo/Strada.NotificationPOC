using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Domain.Entities;

public class ProviderSettings : BaseEntity
{
    public string Name { get; private set; }
    public bool IsEnabled { get; private set; }
    public int Priority { get; private set; }
    public string? ApiKey { get; private set; }

    public ProviderSettings(string Name, bool isEnabled, int priority, string? apiKey = null)
    {
        Name = Name ?? throw new ArgumentNullException(nameof(Name));
        IsEnabled = isEnabled;
        Priority = priority;
        ApiKey = apiKey;
    }

    public void UpdateSettings(bool isEnabled, int priority, string? apiKey = null)
    {
        IsEnabled = isEnabled;
        Priority = priority;
        ApiKey = apiKey;
    }
}