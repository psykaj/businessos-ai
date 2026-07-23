using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessIntelligence.Controllers;

[Route("api/v1/bi/kpis")]
public class KPIsController : BaseBIController
{
    private readonly IKPICalculationService _kpiCalculationService;

    public KPIsController(IKPICalculationService kpiCalculationService)
    {
        _kpiCalculationService = kpiCalculationService;
    }

    [HttpGet]
    public async Task<ActionResult<List<KPIDto>>> GetKPIs([FromQuery] string? category, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var kpis = await _kpiCalculationService.GetKPIsAsync(orgId, category, cancellationToken);
        return Ok(kpis);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<KPIDto>> GetKPIByName(string name, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var kpi = await _kpiCalculationService.GetKPIByNameAsync(orgId, name, cancellationToken);
        if (kpi == null) return NotFound(new { message = $"KPI '{name}' not found" });
        return Ok(kpi);
    }

    [HttpPost("recalculate")]
    public async Task<ActionResult<RecalculateKPIsResponseDto>> RecalculateAllKPIs(CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _kpiCalculationService.RecalculateAllKPIsAsync(orgId, cancellationToken);
        return Ok(result);
    }
}
