using Microsoft.AspNetCore.Mvc;
using Strada.Notification.API.DTOs;
using Strada.Notification.Application.Services;

namespace Strada.Notification.API.Controllers;

[ApiController]
[Route("api/notificacoes")]
public class NotificacaoController : ControllerBase
{
    private readonly NotificacaoService _notificacaoService;

    public NotificacaoController(NotificacaoService notificacaoService)
    {
        _notificacaoService = notificacaoService;
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] NotificacaoRequest request)
    {
        var result = await _notificacaoService.EnviaNotificacaoAsync(request.Type, request.Recipient, request.Message);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(new { message = "Notification sent successfully." });
    }
}