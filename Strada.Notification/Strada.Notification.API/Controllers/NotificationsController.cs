using Microsoft.AspNetCore.Mvc;
using Strada.Notification.API.DTOs.Request;
using Strada.Notification.Application.Interfaces;
using Strada.Notification.Domain.Enums;

namespace Strada.Notification.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto request)
    {
        if (!Enum.IsDefined(typeof(NotificationType), request.Type))
        {
            return BadRequest($"Invalid notification type: {request.Type}");
        }

        var result = await _notificationService.SendNotificationAsync(request.Type, request.Recipient, request.Message);

        if (result.IsSuccess)
        {
            return Ok("Notification sent successfully.");
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotifications()
    {
        var notifications = await _notificationService.GetNotificationsAsync();
        return Ok(notifications);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNotificationById(Guid id)
    {
        var notification = await _notificationService.GetNotificationByIdAsync(id);
        if (notification == null)
            return NotFound(new { Message = "Notification not found." });

        return Ok(notification);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetNotificationsByStatus(NotificationStatus status)
    {
        var notifications = await _notificationService.GetNotificationsByStatusAsync(status);
        return Ok(notifications);
    }
}