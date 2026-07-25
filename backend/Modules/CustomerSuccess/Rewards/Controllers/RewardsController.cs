using backend.Modules.CustomerSuccess.Controllers;
using backend.Modules.CustomerSuccess.Loyalty.DTOs;
using backend.Modules.CustomerSuccess.Loyalty.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerSuccess.Rewards.Controllers;

[Route("api/customer-success/rewards")]
public class RewardsController : BaseCustomerSuccessController
{
    private readonly ILoyaltyService _loyaltyService;

    public RewardsController(ILoyaltyService loyaltyService)
    {
        _loyaltyService = loyaltyService;
    }

    [HttpGet("summary/customer/{customerId:guid}")]
    public async Task<ActionResult<CustomerLoyaltySummaryDto>> GetRewardsSummary(Guid customerId)
    {
        var summary = await _loyaltyService.GetCustomerSummaryAsync(GetOrganizationId(), customerId, null);
        return Ok(summary);
    }

    [HttpPost("redeem")]
    public async Task<ActionResult<LoyaltyTransactionDto>> RedeemReward([FromBody] RedeemPointsDto dto)
    {
        var result = await _loyaltyService.RedeemPointsAsync(GetOrganizationId(), dto);
        return Ok(result);
    }
}
