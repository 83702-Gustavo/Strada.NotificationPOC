namespace Strada.Notification.Domain.Entities;

public class SecuritySettings
{
    public int JwtExpirationTime { get; private set; } 
    public bool LogElasticSearch { get; private set; } 
    public string LogLevel { get; private set; } 

    public SecuritySettings(int jwtExpirationTime, bool logElasticSearch, string logLevel)
    {
        if (jwtExpirationTime <= 0)
            throw new ArgumentException("JWT expiration time must be greater than zero.");

        var validLogLevels = new[] { "Error", "Info", "Debug" };
        if (string.IsNullOrWhiteSpace(logLevel) || !validLogLevels.Contains(logLevel))
            throw new ArgumentException($"Invalid log level. Allowed values: {string.Join(", ", validLogLevels)}");

        JwtExpirationTime = jwtExpirationTime;
        LogElasticSearch = logElasticSearch;
        LogLevel = logLevel;
    }

    public void UpdateJwtExpirationTime(int jwtExpirationTime)
    {
        if (jwtExpirationTime <= 0)
            throw new ArgumentException("JWT expiration time must be greater than zero.");

        JwtExpirationTime = jwtExpirationTime;
    }

    public void UpdateLogElasticSearch(bool logElasticSearch)
    {
        LogElasticSearch = logElasticSearch;
    }

    public void UpdateLogLevel(string logLevel)
    {
        var validLogLevels = new[] { "Error", "Info", "Debug" };
        if (string.IsNullOrWhiteSpace(logLevel) || !validLogLevels.Contains(logLevel))
            throw new ArgumentException($"Invalid log level. Allowed values: {string.Join(", ", validLogLevels)}");

        LogLevel = logLevel;
    }

    public override string ToString()
    {
        return $"JwtExpirationTime={JwtExpirationTime}, LogElasticSearch={LogElasticSearch}, LogLevel={LogLevel}";
    }
}