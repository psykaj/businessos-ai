using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.DecisionCenter.DTOs;
using backend.Modules.DecisionCenter.Entities;
using backend.Modules.DecisionCenter.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.DecisionCenter.Controllers;

[ApiController]
[Route("api/v1/decision-center")]
[Authorize]
public class DecisionCenterController : ControllerBase
{
    private readonly IDecisionLogRepository _repository;

    public DecisionCenterController(IDecisionLogRepository repository)
    {
        _repository = repository;
    }

    private Guid GetOrganizationId()
    {
        var claim = User.FindFirst("OrganizationId")?.Value;
        return claim != null ? Guid.Parse(claim) : Guid.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DecisionLogDto>>> GetAll()
    {
        var orgId = GetOrganizationId();
        var logs = await _repository.GetAllAsync(orgId);
        
        var dtos = new List<DecisionLogDto>();
        foreach (var log in logs) dtos.Add(MapToDto(log));
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<DecisionLogDto>> Create(DecisionLogDto request)
    {
        var orgId = GetOrganizationId();
        var log = new DecisionLog
        {
            OrganizationId = orgId,
            DecisionTitle = request.DecisionTitle,
            Description = request.Description,
            RelatedInsightId = request.RelatedInsightId,
            ExpectedOutcome = request.ExpectedOutcome,
            Status = "Pending",
            DecisionDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(log);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, MapToDto(created));
    }

    private DecisionLogDto MapToDto(DecisionLog log)
    {
        return new DecisionLogDto
        {
            Id = log.Id,
            OrganizationId = log.OrganizationId,
            DecisionTitle = log.DecisionTitle,
            Description = log.Description,
            RelatedInsightId = log.RelatedInsightId,
            DecisionMakerId = log.DecisionMakerId,
            ExpectedOutcome = log.ExpectedOutcome,
            Status = log.Status,
            DecisionDate = log.DecisionDate,
            CreatedAt = log.CreatedAt
        };
    }
}
