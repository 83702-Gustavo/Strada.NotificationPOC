using Microsoft.AspNetCore.Mvc;
using Strada.Notification.Application.DTOs;
using Strada.Notification.Application.Interfaces;

namespace Strada.Notification.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSettings()
    {
        var result = await _settingsService.GetAllSettingsAsync();
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProviderSettings([FromBody] CreateProviderSettingsDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _settingsService.AddProviderSettingsAsync(createDto);

        if (result.IsSuccess)
        {
            return Ok("Provider settings created successfully.");
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProviderSettings([FromBody] UpdateProviderSettingsDto updateDto)
    {
        var result = await _settingsService.UpdateProviderSettingsAsync(updateDto);

        if (result.IsSuccess)
        {
            return Ok("Provider settings updated successfully.");
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("delete/{name}")]
    public async Task<IActionResult> DeleteProviderSettings(string name)
    {
        var result = await _settingsService.DeleteProviderSettingsAsync(name);

        if (result.IsSuccess)
        {
            return Ok("Provider settings deleted successfully.");
        }

        return BadRequest(result.ErrorMessage);
    }
}