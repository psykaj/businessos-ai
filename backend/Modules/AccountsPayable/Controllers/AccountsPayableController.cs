using backend.Modules.AccountsPayable.DTOs;
using backend.Modules.AccountsPayable.Interfaces;
using backend.Modules.Finance.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.AccountsPayable.Controllers;

[Route("api/accounts-payable")]
public class AccountsPayableController : BaseFinanceController
{
    private readonly IAccountsPayableService _apService;

    public AccountsPayableController(IAccountsPayableService apService)
    {
        _apService = apService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPayables([FromQuery] string? status)
    {
        var orgId = GetOrganizationId();
        var payables = await _apService.GetPayablesAsync(orgId, status);
        return Ok(payables);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBill([FromBody] CreateAccountsPayableDto dto)
    {
        var orgId = GetOrganizationId();
        var created = await _apService.CreateBillAsync(orgId, dto);
        return Ok(created);
    }

    [HttpPost("{id:guid}/pay")]
    public async Task<IActionResult> RecordPayment(Guid id, [FromBody] RecordBillPaymentDto request)
    {
        var orgId = GetOrganizationId();
        var updated = await _apService.RecordPaymentAsync(id, orgId, request.Amount);
        if (updated == null) return NotFound(new { message = "Bill record not found" });
        return Ok(updated);
    }

    [HttpGet("aging")]
    public async Task<IActionResult> GetAgingReport()
    {
        var orgId = GetOrganizationId();
        var aging = await _apService.GetAgingReportAsync(orgId);
        return Ok(aging);
    }
}

public record RecordBillPaymentDto(decimal Amount);
