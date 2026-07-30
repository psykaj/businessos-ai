using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.KpiEngine.DTOs;
using backend.Modules.KpiEngine.Entities;
using backend.Modules.KpiEngine.Repositories;
using backend.Modules.KpiEngine.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;

namespace backend.Modules.KpiEngine.Controllers;

[ApiController]
[Route("api/v1/kpis")]
[Authorize]
public class KpiController : ControllerBase
{
    private readonly IKpiRepository _kpiRepository;
    private readonly IKpiCalculationService _kpiCalculationService;

    public KpiController(IKpiRepository kpiRepository, IKpiCalculationService kpiCalculationService)
    {
        _kpiRepository = kpiRepository;
        _kpiCalculationService = kpiCalculationService;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<KpiDto>>> GetAllKpis()
    {
        var orgId = GetOrganizationId();
        var kpis = await _kpiRepository.GetAllAsync(orgId);
        
        var dtos = new List<KpiDto>();
        foreach (var k in kpis)
        {
            dtos.Add(MapToDto(k));
        }
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<KpiDto>> GetKpi(Guid id)
    {
        var orgId = GetOrganizationId();
        var kpi = await _kpiRepository.GetByIdAsync(id, orgId);
        if (kpi == null) return NotFound();

        return Ok(MapToDto(kpi));
    }

    [HttpPost]
    public async Task<ActionResult<KpiDto>> CreateKpi([FromBody] CreateKpiDto request)
    {
        var orgId = GetOrganizationId();
        var kpi = new KPI
        {
            OrganizationId = orgId,
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            Unit = request.Unit,
            TargetValue = request.TargetValue,
            Frequency = request.Frequency,
            IsHigherBetter = request.IsHigherBetter,
            LastCalculatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _kpiRepository.AddAsync(kpi);
        return CreatedAtAction(nameof(GetKpi), new { id = created.Id }, MapToDto(created));
    }

    [HttpPost("{id}/calculate")]
    public async Task<ActionResult<KpiDto>> CalculateKpi(Guid id)
    {
        var orgId = GetOrganizationId();
        try
        {
            var kpi = await _kpiCalculationService.CalculateKpiAsync(orgId, id);
            return Ok(MapToDto(kpi));
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("calculate-all")]
    public async Task<IActionResult> CalculateAll()
    {
        var orgId = GetOrganizationId();
        await _kpiCalculationService.CalculateAllKpisAsync(orgId);
        return Ok(new { Message = "Calculation triggered for all KPIs." });
    }

    private KpiDto MapToDto(KPI kpi)
    {
        return new KpiDto
        {
            Id = kpi.Id,
            OrganizationId = kpi.OrganizationId,
            Name = kpi.Name,
            Description = kpi.Description,
            Category = kpi.Category,
            Unit = kpi.Unit,
            CurrentValue = kpi.CurrentValue,
            TargetValue = kpi.TargetValue,
            PreviousValue = kpi.PreviousValue,
            Frequency = kpi.Frequency,
            IsHigherBetter = kpi.IsHigherBetter,
            LastCalculatedAt = kpi.LastCalculatedAt,
            CreatedAt = kpi.CreatedAt,
            UpdatedAt = kpi.UpdatedAt
        };
    }
}
