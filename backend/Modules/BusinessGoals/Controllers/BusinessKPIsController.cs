using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.DTOs;
using backend.Modules.BusinessGoals.Entities;
using backend.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.BusinessGoals.Controllers;

[ApiController]
[Route("api/v1/business-kpis")]
[Authorize]
public class BusinessKPIsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public BusinessKPIsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BusinessKPIDto>>> GetAll()
    {
        var orgId = GetOrganizationId();
        var kpis = await _dbContext.BusinessKPIs
            .Where(k => k.OrganizationId == orgId && !k.IsDeleted)
            .ToListAsync();
        
        return Ok(kpis.Select(MapToDto).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BusinessKPIDto>> GetById(Guid id)
    {
        var orgId = GetOrganizationId();
        var kpi = await _dbContext.BusinessKPIs
            .FirstOrDefaultAsync(k => k.Id == id && k.OrganizationId == orgId && !k.IsDeleted);
            
        if (kpi == null) return NotFound();
        return Ok(MapToDto(kpi));
    }

    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetSummary()
    {
        var orgId = GetOrganizationId();
        var kpis = await _dbContext.BusinessKPIs
            .Where(k => k.OrganizationId == orgId && !k.IsDeleted)
            .ToListAsync();

        var summary = new
        {
            TotalKPIs = kpis.Count,
            OnTrack = kpis.Count(k => k.Status == "OnTrack"),
            AtRisk = kpis.Count(k => k.Status == "AtRisk"),
            Behind = kpis.Count(k => k.Status == "Behind")
        };

        return Ok(summary);
    }

    [HttpGet("{id}/history")]
    public async Task<ActionResult<object>> GetHistory(Guid id)
    {
        var orgId = GetOrganizationId();
        var kpi = await _dbContext.BusinessKPIs
            .FirstOrDefaultAsync(k => k.Id == id && k.OrganizationId == orgId && !k.IsDeleted);
            
        if (kpi == null) return NotFound();

        // In MVP, history might just be calculated on the fly or pulled from another table.
        // Returning a mock array as a placeholder.
        return Ok(new[]
        {
            new { Date = DateTime.UtcNow.AddMonths(-2), Value = kpi.CurrentValue * 0.8m },
            new { Date = DateTime.UtcNow.AddMonths(-1), Value = kpi.CurrentValue * 0.9m },
            new { Date = DateTime.UtcNow, Value = kpi.CurrentValue }
        });
    }

    [HttpGet("{id}/trend")]
    public async Task<ActionResult<object>> GetTrend(Guid id)
    {
        var orgId = GetOrganizationId();
        var kpi = await _dbContext.BusinessKPIs
            .FirstOrDefaultAsync(k => k.Id == id && k.OrganizationId == orgId && !k.IsDeleted);
            
        if (kpi == null) return NotFound();

        var trend = kpi.CurrentValue > kpi.PreviousValue ? "Up" : 
                    (kpi.CurrentValue < kpi.PreviousValue ? "Down" : "Flat");
        
        return Ok(new { Trend = trend, Direction = kpi.Direction });
    }

    private BusinessKPIDto MapToDto(BusinessKPI k)
    {
        return new BusinessKPIDto
        {
            Id = k.Id,
            OrganizationId = k.OrganizationId,
            BusinessGoalId = k.BusinessGoalId,
            Name = k.Name,
            KPIType = k.KPIType,
            MetricKey = k.MetricKey,
            Description = k.Description,
            CurrentValue = k.CurrentValue,
            TargetValue = k.TargetValue,
            PreviousValue = k.PreviousValue,
            Unit = k.Unit,
            Currency = k.Currency,
            Direction = k.Direction,
            Frequency = k.Frequency,
            Status = k.Status,
            LastCalculatedAt = k.LastCalculatedAt,
            CreatedAt = k.CreatedAt,
            UpdatedAt = k.UpdatedAt,
            IsActive = k.IsActive
        };
    }
}
