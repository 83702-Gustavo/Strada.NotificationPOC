namespace Strada.Notification.Application.DTOs;

public class CreateProviderSettingsDto
{
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public int Priority { get; set; }
    public string ApiKey { get; set; }
}