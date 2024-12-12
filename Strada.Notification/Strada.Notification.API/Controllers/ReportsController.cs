using Microsoft.AspNetCore.Mvc;
using Strada.Notification.Application.DTOs;
using Strada.Notification.Application.Services;

namespace Strada.Notification.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ReportsService _reportsService;

    public ReportsController(ReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [HttpGet("por-app")]
    public IActionResult RelatorioPorApp()
    {
        var relatorio = _reportsService.GerarRelatorioPorApp();
        return Ok(relatorio);
    }

    [HttpGet("por-provedor")]
    public IActionResult RelatorioPorProvedor()
    {
        var relatorio = _reportsService.GerarRelatorioPorProvedor();
        return Ok(relatorio);
    }
}