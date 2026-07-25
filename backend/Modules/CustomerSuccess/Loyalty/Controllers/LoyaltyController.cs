using backend.Modules.CustomerSuccess.Controllers;
using backend.Modules.CustomerSuccess.Loyalty.DTOs;
using backend.Modules.CustomerSuccess.Loyalty.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerSuccess.Loyalty.Controllers;

[Route("api/customer-success/loyalty")]
public class LoyaltyController : BaseCustomerSuccessController
{
    private readonly ILoyaltyService _loyaltyService;

    public LoyaltyController(ILoyaltyService loyaltyService)
    {
        _loyaltyService = loyaltyService;
    }

    [HttpGet("programs")]
    public async Task<ActionResult<IEnumerable<LoyaltyProgramDto>>> GetPrograms()
    {
        var programs = await _loyaltyService.GetProgramsAsync(GetOrganizationId());
        return Ok(programs);
    }

    [HttpGet("programs/{id:guid}")]
    public async Task<ActionResult<LoyaltyProgramDto>> GetProgramById(Guid id)
    {
        var program = await _loyaltyService.GetProgramByIdAsync(GetOrganizationId(), id);
        return Ok(program);
    }

    [HttpPost("programs")]
    public async Task<ActionResult<LoyaltyProgramDto>> CreateProgram([FromBody] CreateLoyaltyProgramDto dto)
    {
        var program = await _loyaltyService.CreateProgramAsync(GetOrganizationId(), dto);
        return CreatedAtAction(nameof(GetProgramById), new { id = program.Id }, program);
    }

    [HttpPut("programs/{id:guid}")]
    public async Task<ActionResult<LoyaltyProgramDto>> UpdateProgram(Guid id, [FromBody] UpdateLoyaltyProgramDto dto)
    {
        var program = await _loyaltyService.UpdateProgramAsync(GetOrganizationId(), id, dto);
        return Ok(program);
    }

    [HttpPost("earn")]
    public async Task<ActionResult<LoyaltyTransactionDto>> EarnPoints([FromBody] EarnPointsDto dto)
    {
        var tx = await _loyaltyService.EarnPointsAsync(GetOrganizationId(), dto);
        return Ok(tx);
    }

    [HttpPost("redeem")]
    public async Task<ActionResult<LoyaltyTransactionDto>> RedeemPoints([FromBody] RedeemPointsDto dto)
    {
        var tx = await _loyaltyService.RedeemPointsAsync(GetOrganizationId(), dto);
        return Ok(tx);
    }

    [HttpPost("adjust")]
    public async Task<ActionResult<LoyaltyTransactionDto>> AdjustPoints([FromBody] AdjustPointsDto dto)
    {
        var tx = await _loyaltyService.AdjustPointsAsync(GetOrganizationId(), dto);
        return Ok(tx);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<ActionResult<CustomerLoyaltySummaryDto>> GetCustomerLoyalty(Guid customerId, [FromQuery] Guid? programId)
    {
        var summary = await _loyaltyService.GetCustomerSummaryAsync(GetOrganizationId(), customerId, programId);
        return Ok(summary);
    }

    [HttpGet("transactions")]
    public async Task<ActionResult> GetTransactions(
        [FromQuery] Guid? customerId,
        [FromQuery] Guid? programId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _loyaltyService.GetTransactionsPagedAsync(
            GetOrganizationId(), customerId, programId, page, pageSize);

        return Ok(new { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize });
    }
}
