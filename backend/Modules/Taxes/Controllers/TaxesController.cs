using backend.Modules.Finance.Controllers;
using backend.Modules.Taxes.DTOs;
using backend.Modules.Taxes.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Taxes.Controllers;

[Route("api/taxes")]
public class TaxesController : BaseFinanceController
{
    private readonly ITaxEngine _taxEngine;

    public TaxesController(ITaxEngine taxEngine)
    {
        _taxEngine = taxEngine;
    }

    [HttpGet]
    public async Task<IActionResult> GetTaxes()
    {
        var orgId = GetOrganizationId();
        var taxes = await _taxEngine.GetTaxesAsync(orgId);
        return Ok(taxes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTax([FromBody] CreateTaxRecordDto dto)
    {
        var orgId = GetOrganizationId();
        var created = await _taxEngine.CreateTaxAsync(orgId, dto);
        return Ok(created);
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateTax([FromBody] CalculateTaxRequestDto request)
    {
        var orgId = GetOrganizationId();
        var result = await _taxEngine.CalculateTaxAsync(orgId, request);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetTaxSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var orgId = GetOrganizationId();
        var summary = await _taxEngine.GetTaxSummaryAsync(orgId, startDate, endDate);
        return Ok(summary);
    }
}
