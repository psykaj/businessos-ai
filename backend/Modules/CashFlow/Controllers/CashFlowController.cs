using backend.Modules.CashFlow.DTOs;
using backend.Modules.CashFlow.Interfaces;
using backend.Modules.Finance.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CashFlow.Controllers;

[Route("api/cashflow")]
public class CashFlowController : BaseFinanceController
{
    private readonly ICashFlowEngine _cashFlowEngine;

    public CashFlowController(ICashFlowEngine cashFlowEngine)
    {
        _cashFlowEngine = cashFlowEngine;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var orgId = GetOrganizationId();
        var summary = await _cashFlowEngine.GetSummaryAsync(orgId, startDate, endDate);
        return Ok(summary);
    }

    [HttpGet("monthly-breakdown")]
    public async Task<IActionResult> GetMonthlyBreakdown([FromQuery] int months = 6)
    {
        var orgId = GetOrganizationId();
        var breakdown = await _cashFlowEngine.GetMonthlyCashFlowAsync(orgId, months);
        return Ok(breakdown);
    }

    [HttpGet("forecast")]
    public async Task<IActionResult> GetForecast()
    {
        var orgId = GetOrganizationId();
        var forecast = await _cashFlowEngine.GetForecastAsync(orgId);
        return Ok(forecast);
    }

    [HttpPost("entries")]
    public async Task<IActionResult> AddEntry([FromBody] CreateCashFlowEntryDto dto)
    {
        var orgId = GetOrganizationId();
        var created = await _cashFlowEngine.AddEntryAsync(orgId, dto);
        return Ok(created);
    }
}
