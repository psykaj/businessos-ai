using backend.Modules.AccountsReceivable.DTOs;
using backend.Modules.AccountsReceivable.Interfaces;
using backend.Modules.Finance.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AccountsReceivable.Controllers;

[Route("api/accounts-receivable")]
public class AccountsReceivableController : BaseFinanceController
{
    private readonly IAccountsReceivableService _arService;

    public AccountsReceivableController(IAccountsReceivableService arService)
    {
        _arService = arService;
    }

    [HttpGet]
    public async Task<IActionResult> GetReceivables([FromQuery] string? status)
    {
        var orgId = GetOrganizationId();
        var receivables = await _arService.GetReceivablesAsync(orgId, status);
        return Ok(receivables);
    }

    [HttpGet("aging")]
    public async Task<IActionResult> GetAgingReport()
    {
        var orgId = GetOrganizationId();
        var aging = await _arService.GetAgingReportAsync(orgId);
        return Ok(aging);
    }

    [HttpPost("{id:guid}/remind-overdue")]
    public async Task<IActionResult> SendReminder(Guid id, [FromBody] SendReminderRequestDto? request)
    {
        var orgId = GetOrganizationId();
        var sent = await _arService.SendOverdueReminderAsync(id, orgId, request?.CustomNote);
        if (!sent) return NotFound(new { message = "Accounts Receivable record not found" });
        return Ok(new { message = "Overdue reminder sent successfully." });
    }
}
