using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessIntelligence.Controllers;

[Route("api/v1/bi/dashboard")]
public class ExecutiveDashboardController : BaseBIController
{
    private readonly IExecutiveDashboardService _dashboardService;

    public ExecutiveDashboardController(IExecutiveDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("ceo")]
    public async Task<ActionResult<CEODashboardDto>> GetCEODashboard([FromQuery] DashboardFilterDto filter, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var data = await _dashboardService.GetCEODashboardAsync(orgId, filter, cancellationToken);
        return Ok(data);
    }

    [HttpGet("sales")]
    public async Task<ActionResult<SalesDashboardDto>> GetSalesDashboard([FromQuery] DashboardFilterDto filter, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var data = await _dashboardService.GetSalesDashboardAsync(orgId, filter, cancellationToken);
        return Ok(data);
    }

    [HttpGet("marketing")]
    public async Task<ActionResult<MarketingDashboardDto>> GetMarketingDashboard([FromQuery] DashboardFilterDto filter, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var data = await _dashboardService.GetMarketingDashboardAsync(orgId, filter, cancellationToken);
        return Ok(data);
    }

    [HttpGet("finance")]
    public async Task<ActionResult<FinanceDashboardDto>> GetFinanceDashboard([FromQuery] DashboardFilterDto filter, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var data = await _dashboardService.GetFinanceDashboardAsync(orgId, filter, cancellationToken);
        return Ok(data);
    }

    [HttpGet("operations")]
    public async Task<ActionResult<OperationsDashboardDto>> GetOperationsDashboard([FromQuery] DashboardFilterDto filter, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var data = await _dashboardService.GetOperationsDashboardAsync(orgId, filter, cancellationToken);
        return Ok(data);
    }

    [HttpGet("team")]
    public async Task<ActionResult<TeamPerformanceDashboardDto>> GetTeamPerformanceDashboard([FromQuery] DashboardFilterDto filter, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var data = await _dashboardService.GetTeamPerformanceDashboardAsync(orgId, filter, cancellationToken);
        return Ok(data);
    }
}
