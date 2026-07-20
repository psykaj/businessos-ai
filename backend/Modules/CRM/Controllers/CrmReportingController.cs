using backend.Modules.CRM.DTOs;
using backend.Modules.CRM.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.CRM.Controllers;

[Route("api/crm/reporting")]
public class CrmReportingController : BaseCrmController
{
    private readonly ICrmReportingService _service;

    public CrmReportingController(ICrmReportingService service)
    {
        _service = service;
    }

    [HttpGet("overview")]
    public async Task<ActionResult<CrmOverviewDto>> GetOverview()
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetOverviewAsync(orgId);
        return Ok(result);
    }

    [HttpGet("performance")]
    public async Task<ActionResult<IReadOnlyList<SalesPerformanceDto>>> GetSalesPerformance([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var orgId = GetOrganizationId();
        var result = await _service.GetSalesPerformanceAsync(orgId, startDate, endDate);
        return Ok(result);
    }
}
