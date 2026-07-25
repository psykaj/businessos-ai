using backend.Modules.CustomerSuccess.Controllers;
using backend.Modules.CustomerSuccess.Retention.DTOs;
using backend.Modules.CustomerSuccess.Retention.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CustomerSuccess.Retention.Controllers;

[Route("api/customer-success/retention")]
public class RetentionController : BaseCustomerSuccessController
{
    private readonly IRetentionService _retentionService;

    public RetentionController(IRetentionService retentionService)
    {
        _retentionService = retentionService;
    }

    [HttpGet("overview")]
    public async Task<ActionResult<RetentionOverviewDto>> GetOverview()
    {
        var overview = await _retentionService.GetRetentionOverviewAsync(GetOrganizationId());
        return Ok(overview);
    }
}
