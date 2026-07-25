using backend.Modules.CustomerSuccess.Controllers;
using backend.Modules.CustomerSuccess.Referrals.DTOs;
using backend.Modules.CustomerSuccess.Referrals.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerSuccess.Referrals.Controllers;

[Route("api/customer-success/referrals")]
public class ReferralsController : BaseCustomerSuccessController
{
    private readonly IReferralService _referralService;

    public ReferralsController(IReferralService referralService)
    {
        _referralService = referralService;
    }

    [HttpPost]
    public async Task<ActionResult<ReferralDto>> Create([FromBody] CreateReferralDto dto)
    {
        var referral = await _referralService.CreateReferralAsync(GetOrganizationId(), dto);
        return Ok(referral);
    }

    [HttpGet]
    public async Task<ActionResult> GetPaged(
        [FromQuery] string? status,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _referralService.GetPagedAsync(
            GetOrganizationId(), status, search, page, pageSize);

        return Ok(new { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize });
    }

    [HttpGet("analytics")]
    public async Task<ActionResult<ReferralAnalyticsDto>> GetAnalytics()
    {
        var analytics = await _referralService.GetAnalyticsAsync(GetOrganizationId());
        return Ok(analytics);
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<ReferralDto>> GetByCode(string code)
    {
        var referral = await _referralService.GetByCodeAsync(GetOrganizationId(), code);
        return Ok(referral);
    }

    [HttpPost("convert")]
    public async Task<ActionResult<ReferralDto>> Convert([FromBody] ConvertReferralDto dto)
    {
        var referral = await _referralService.ConvertReferralAsync(GetOrganizationId(), dto);
        return Ok(referral);
    }

    [HttpPost("{id:guid}/issue-reward")]
    public async Task<ActionResult<ReferralDto>> IssueReward(Guid id)
    {
        var referral = await _referralService.IssueRewardAsync(GetOrganizationId(), id);
        return Ok(referral);
    }

    [HttpGet("referrer/{referrerId:guid}")]
    public async Task<ActionResult<IEnumerable<ReferralDto>>> GetByReferrer(Guid referrerId)
    {
        var referrals = await _referralService.GetByReferrerAsync(GetOrganizationId(), referrerId);
        return Ok(referrals);
    }
}
