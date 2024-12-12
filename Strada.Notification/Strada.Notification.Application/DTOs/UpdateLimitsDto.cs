namespace Strada.Notification.Application.DTOs;

public class UpdateLimitsDto
{
    public int ApplicationLimit { get; set; } // Maximum notifications per application
    public int ProviderLimit { get; set; } // Maximum notifications per provider
    public string Interval { get; set; } // Interval for the limits (e.g., "daily", "hourly")
}