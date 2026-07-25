using backend.Modules.CustomerSuccess.Controllers;
using backend.Modules.CustomerSuccess.CustomerHealth.DTOs;
using backend.Modules.CustomerSuccess.CustomerHealth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerSuccess.CustomerHealth.Controllers;

[Route("api/customer-success/health")]
public class CustomerHealthController : BaseCustomerSuccessController
{
    private readonly ICustomerHealthService _healthService;

    public CustomerHealthController(ICustomerHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<CustomerHealthSummaryDto>> GetSummary()
    {
        var summary = await _healthService.GetSummaryAsync(GetOrganizationId());
        return Ok(summary);
    }

    [HttpGet]
    public async Task<ActionResult> GetPaged(
        [FromQuery] string? riskLevel,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = "UpdatedAt",
        [FromQuery] bool descending = true)
    {
        var (items, totalCount) = await _healthService.GetPagedAsync(
            GetOrganizationId(), riskLevel, search, page, pageSize, sortBy, descending);

        return Ok(new { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize });
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<ActionResult<CustomerHealthDto>> GetByCustomer(Guid customerId)
    {
        var result = await _healthService.GetByCustomerIdAsync(GetOrganizationId(), customerId);
        return Ok(result);
    }

    [HttpPost("calculate/{customerId:guid}")]
    public async Task<ActionResult<CustomerHealthDto>> Calculate(Guid customerId)
    {
        var result = await _healthService.CalculateHealthAsync(GetOrganizationId(), customerId);
        return Ok(result);
    }

    [HttpPost("update-metrics")]
    public async Task<ActionResult<CustomerHealthDto>> UpdateMetrics([FromBody] UpdateCustomerHealthMetricsDto dto)
    {
        var result = await _healthService.UpdateMetricsAsync(GetOrganizationId(), dto);
        return Ok(result);
    }

    [HttpPost("recalculate-all")]
    public async Task<ActionResult> RecalculateAll()
    {
        await _healthService.RecalculateAllAsync(GetOrganizationId());
        return Ok(new { Message = "Health scores recalculated for all customers." });
    }
}
