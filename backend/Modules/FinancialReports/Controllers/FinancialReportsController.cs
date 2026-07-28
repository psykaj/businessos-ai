using backend.Modules.Finance.Controllers;
using backend.Modules.FinancialReports.DTOs;
using backend.Modules.FinancialReports.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.FinancialReports.Controllers;

[Route("api/financial-reports")]
public class FinancialReportsController : BaseFinanceController
{
    private readonly IFinancialReportGenerator _reportGenerator;

    public FinancialReportsController(IFinancialReportGenerator reportGenerator)
    {
        _reportGenerator = reportGenerator;
    }

    [HttpGet("profit-and-loss")]
    public async Task<IActionResult> GetProfitAndLoss([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var orgId = GetOrganizationId();
        var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
        var end = endDate ?? DateTime.UtcNow;

        var report = await _reportGenerator.GenerateProfitAndLossAsync(orgId, start, end);
        return Ok(report);
    }

    [HttpGet("cash-flow")]
    public async Task<IActionResult> GetCashFlowStatement([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var orgId = GetOrganizationId();
        var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
        var end = endDate ?? DateTime.UtcNow;

        var report = await _reportGenerator.GenerateCashFlowStatementAsync(orgId, start, end);
        return Ok(report);
    }

    [HttpGet("expense-summary")]
    public async Task<IActionResult> GetExpenseReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var orgId = GetOrganizationId();
        var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
        var end = endDate ?? DateTime.UtcNow;

        var report = await _reportGenerator.GenerateExpenseReportAsync(orgId, start, end);
        return Ok(report);
    }

    [HttpGet("revenue-summary")]
    public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var orgId = GetOrganizationId();
        var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
        var end = endDate ?? DateTime.UtcNow;

        var report = await _reportGenerator.GenerateRevenueReportAsync(orgId, start, end);
        return Ok(report);
    }
}
