namespace Strada.Notification.Application.DTOs;

public class UpdateSecurityDto
{
    public int JwtExpirationTime { get; set; } 
    public bool LogElasticSearch { get; set; } 
    public string LogLevel { get; set; }
}