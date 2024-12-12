namespace Strada.Notification.Domain.Entities;

public class ConfigurationLimits
{
    public int ApplicationLimit { get; private set; }
    public int ProviderLimit { get; private set; }
    public string Interval { get; private set; }

    public ConfigurationLimits(int applicationLimit, int providerLimit, string interval)
    {
        ApplicationLimit = applicationLimit;
        ProviderLimit = providerLimit;
        Interval = interval;
    }
}