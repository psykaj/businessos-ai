using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.Forecasting.DTOs;
using backend.Modules.Forecasting.Entities;
using backend.Modules.Forecasting.Repositories;
using backend.Modules.Forecasting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Forecasting.Controllers;

[ApiController]
[Route("api/v1/forecasting")]
[Authorize]
public class ForecastingController : ControllerBase
{
    private readonly IForecastRepository _forecastRepository;
    private readonly IForecastingService _forecastingService;

    public ForecastingController(IForecastRepository forecastRepository, IForecastingService forecastingService)
    {
        _forecastRepository = forecastRepository;
        _forecastingService = forecastingService;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ForecastDto>>> GetAllForecasts()
    {
        var orgId = GetOrganizationId();
        var forecasts = await _forecastRepository.GetAllAsync(orgId);
        
        var dtos = new List<ForecastDto>();
        foreach (var f in forecasts) dtos.Add(MapToDto(f));
        return Ok(dtos);
    }

    [HttpGet("metric/{metricName}")]
    public async Task<ActionResult<IEnumerable<ForecastDto>>> GetForecastByMetric(string metricName)
    {
        var orgId = GetOrganizationId();
        var forecasts = await _forecastRepository.GetByMetricAsync(metricName, orgId);
        
        var dtos = new List<ForecastDto>();
        foreach (var f in forecasts) dtos.Add(MapToDto(f));
        return Ok(dtos);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<IEnumerable<ForecastDto>>> GenerateForecast([FromBody] GenerateForecastDto request)
    {
        var orgId = GetOrganizationId();
        try
        {
            var forecasts = await _forecastingService.GenerateForecastAsync(orgId, request.MetricName, request.ForecastMonths);
            var dtos = new List<ForecastDto>();
            foreach (var f in forecasts) dtos.Add(MapToDto(f));
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    private ForecastDto MapToDto(Forecast forecast)
    {
        return new ForecastDto
        {
            Id = forecast.Id,
            OrganizationId = forecast.OrganizationId,
            MetricName = forecast.MetricName,
            PredictedValue = forecast.PredictedValue,
            LowerBound = forecast.LowerBound,
            UpperBound = forecast.UpperBound,
            ConfidenceLevel = forecast.ConfidenceLevel,
            ForecastDate = forecast.ForecastDate,
            GeneratedAt = forecast.GeneratedAt,
            ModelUsed = forecast.ModelUsed,
            Factors = forecast.Factors
        };
    }
}
