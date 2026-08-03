using System;
using System.Threading.Tasks;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.DTOs;
using backend.Modules.CustomerFeedback.CustomerSatisfaction.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerFeedback.CustomerSatisfaction.Controllers;

[ApiController]
[Route("api/v1/csat-metrics")]
[Authorize]
public class CustomerSatisfactionController : ControllerBase
{
    private readonly ICsatService _service;

    public CustomerSatisfactionController(ICsatService service)
    {
        _service = service;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard([FromQuery] Guid organizationId)
    {
        var result = await _service.GetDashboardMetricsAsync(organizationId);
        return Ok(result);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetCustomerScore([FromRoute] Guid customerId, [FromQuery] Guid organizationId)
    {
        var result = await _service.GetCustomerScoreAsync(organizationId, customerId);
        return Ok(result);
    }

    [HttpPost("recalculate")]
    public async Task<IActionResult> Recalculate([FromBody] CalculateCsatRequestDto request)
    {
        await _service.RecalculateCustomerMetricsAsync(request.OrganizationId, request.CustomerId);
        var res = await _service.GetCustomerScoreAsync(request.OrganizationId, request.CustomerId);
        return Ok(res);
    }
}
