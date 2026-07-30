using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.Scorecards.DTOs;
using backend.Modules.Scorecards.Entities;
using backend.Modules.Scorecards.Repositories;
using backend.Modules.Scorecards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.Scorecards.Controllers;

[ApiController]
[Route("api/v1/scorecards")]
[Authorize]
public class ScorecardsController : ControllerBase
{
    private readonly IScorecardRepository _repository;
    private readonly ScorecardService _service;

    public ScorecardsController(IScorecardRepository repository, ScorecardService service)
    {
        _repository = repository;
        _service = service;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScorecardDto>>> GetAll()
    {
        var orgId = GetOrganizationId();
        var scorecards = await _repository.GetAllAsync(orgId);
        
        var dtos = new List<ScorecardDto>();
        foreach (var s in scorecards) dtos.Add(MapToDto(s));
        return Ok(dtos);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateScorecardRequest request)
    {
        var orgId = GetOrganizationId();
        await _service.GenerateScorecardAsync(orgId, request.Name, request.Department, request.PeriodStart, request.PeriodEnd);
        return Ok(new { Message = "Scorecard generated." });
    }

    private ScorecardDto MapToDto(Scorecard s)
    {
        return new ScorecardDto
        {
            Id = s.Id,
            OrganizationId = s.OrganizationId,
            Name = s.Name,
            Description = s.Description,
            Department = s.Department,
            PeriodStart = s.PeriodStart,
            PeriodEnd = s.PeriodEnd,
            OverallScore = s.OverallScore,
            KpiSummaryJson = s.KpiSummaryJson,
            CreatedAt = s.CreatedAt
        };
    }
}

public class GenerateScorecardRequest
{
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}
