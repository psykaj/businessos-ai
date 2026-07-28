using backend.Modules.Accounting.DTOs;
using backend.Modules.Accounting.Interfaces;
using backend.Modules.Finance.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Accounting.Controllers;

[Route("api/accounting")]
public class AccountingController : BaseFinanceController
{
    private readonly IAccountingService _accountingService;

    public AccountingController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts([FromQuery] string? type, [FromQuery] bool activeOnly = false)
    {
        var orgId = GetOrganizationId();
        var accounts = await _accountingService.GetChartOfAccountsAsync(orgId, type, activeOnly);
        return Ok(accounts);
    }

    [HttpGet("accounts/{id:guid}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        var orgId = GetOrganizationId();
        var account = await _accountingService.GetAccountByIdAsync(id, orgId);
        if (account == null) return NotFound(new { message = "Account not found" });
        return Ok(account);
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
    {
        var orgId = GetOrganizationId();
        try
        {
            var created = await _accountingService.CreateAccountAsync(orgId, dto);
            return CreatedAtAction(nameof(GetAccountById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("accounts/{id:guid}")]
    public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountDto dto)
    {
        var orgId = GetOrganizationId();
        var updated = await _accountingService.UpdateAccountAsync(id, orgId, dto);
        if (updated == null) return NotFound(new { message = "Account not found" });
        return Ok(updated);
    }

    [HttpDelete("accounts/{id:guid}")]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        var orgId = GetOrganizationId();
        var success = await _accountingService.DeleteAccountAsync(id, orgId);
        if (!success) return NotFound(new { message = "Account not found" });
        return NoContent();
    }
}
