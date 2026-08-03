using System;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.ServiceQuality.DTOs;
using backend.Modules.CustomerFeedback.ServiceQuality.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerFeedback.ServiceQuality.Controllers;

[ApiController]
[Route("api/v1/service-quality")]
[Authorize]
public class ServiceQualityController : ControllerBase
{
    private readonly IServiceQualityService _service;

    public ServiceQualityController(IServiceQualityService service)
    {
        _service = service;
    }

    [HttpGet("kpis")]
    public async Task<IActionResult> GetLatestKpis([FromQuery] Guid organizationId)
    {
        var res = await _service.GetLatestKpisAsync(organizationId);
        return Ok(res);
    }

    [HttpGet("kpis/history")]
    public async Task<IActionResult> GetKpiHistory([FromQuery] Guid organizationId, [FromQuery] int days = 30)
    {
        var res = await _service.GetKpiHistoryAsync(organizationId, days);
        return Ok(res);
    }

    [HttpPost("record")]
    public async Task<IActionResult> RecordKpi([FromBody] RecordServiceMetricRequestDto request)
    {
        var res = await _service.RecordKpiAsync(request);
        return Ok(res);
    }
}
