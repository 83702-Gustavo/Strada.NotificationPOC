using Microsoft.AspNetCore.Mvc;
using Strada.Notification.Application.Services;
using Strada.Notification.Application.DTOs;

namespace Strada.Notification.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly SettingsService _settingsService;

    public SettingsController(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpGet("providers")]
    public IActionResult GetProviders()
    {
        var providers = _settingsService.GetProviders();
        return Ok(providers);
    }

    [HttpPost("providers/activate")]
    public IActionResult ActivateProvider([FromBody] UpdateProviderStatusDto dto)
    {
        _settingsService.ActivateProvider(dto.Name);
        return Ok($"Provider '{dto.Name}' activated successfully.");
    }

    [HttpPost("providers/deactivate")]
    public IActionResult DeactivateProvider([FromBody] UpdateProviderStatusDto dto)
    {
        _settingsService.DeactivateProvider(dto.Name);
        return Ok($"Provider '{dto.Name}' deactivated successfully.");
    }

    [HttpPut("providers/priority")]
    public IActionResult UpdateProviderPriority([FromBody] UpdateProviderPriorityDto dto)
    {
        _settingsService.UpdateProviderPriority(dto.Name, dto.Priority);
        return Ok($"Provider '{dto.Name}' priority updated to {dto.Priority}.");
    }

    [HttpGet("limits")]
    public IActionResult GetLimits()
    {
        var limits = _settingsService.GetLimits();
        return Ok(limits);
    }

    [HttpPut("limits")]
    public IActionResult UpdateLimits([FromBody] UpdateLimitsDto dto)
    {
        _settingsService.UpdateLimits(dto);
        return Ok("Limits updated successfully.");
    }

    [HttpGet("security")]
    public IActionResult GetSecuritySettings()
    {
        var security = _settingsService.GetSecuritySettings();
        return Ok(security);
    }

    [HttpPut("security")]
    public IActionResult UpdateSecuritySettings([FromBody] UpdateSecurityDto dto)
    {
        _settingsService.UpdateSecuritySettings(dto);
        return Ok("Security settings updated successfully.");
    }
}