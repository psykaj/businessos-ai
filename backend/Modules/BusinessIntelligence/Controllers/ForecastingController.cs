using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessIntelligence.Controllers;

[Route("api/v1/bi/forecasting")]
public class ForecastingController : BaseBIController
{
    private readonly IForecastEngineService _forecastEngine;

    public ForecastingController(IForecastEngineService forecastEngine)
    {
        _forecastEngine = forecastEngine;
    }

    [HttpGet("{type}")]
    public async Task<ActionResult<ForecastSummaryDto>> GetForecast(string type, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _forecastEngine.GetForecastAsync(orgId, type, cancellationToken);
        return Ok(result);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ForecastSummaryDto>> GenerateForecast([FromBody] GenerateForecastRequestDto request, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var result = await _forecastEngine.GenerateForecastAsync(orgId, request, cancellationToken);
        return Ok(result);
    }
}
