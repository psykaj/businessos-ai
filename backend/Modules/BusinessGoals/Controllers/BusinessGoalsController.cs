using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.DTOs;
using backend.Modules.BusinessGoals.Entities;
using backend.Modules.BusinessGoals.Repositories;
using backend.Modules.BusinessGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Modules.BusinessGoals.Controllers;

[ApiController]
[Route("api/v1/business-goals")]
[Authorize]
public class BusinessGoalsController : ControllerBase
{
    private readonly IBusinessGoalRepository _repository;
    private readonly GoalTrackingService _service;

    public BusinessGoalsController(IBusinessGoalRepository repository, GoalTrackingService service)
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
    public async Task<ActionResult<IEnumerable<BusinessGoalDto>>> GetAll()
    {
        var orgId = GetOrganizationId();
        var goals = await _repository.GetAllAsync(orgId);
        
        var dtos = new List<BusinessGoalDto>();
        foreach (var g in goals) dtos.Add(MapToDto(g));
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<BusinessGoalDto>> Create(BusinessGoalDto dto)
    {
        var orgId = GetOrganizationId();
        var goal = new BusinessGoal
        {
            OrganizationId = orgId,
            Title = dto.Title,
            Description = dto.Description,
            Department = dto.Department,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            KpiId = dto.KpiId,
            TargetValue = dto.TargetValue,
            Status = "OnTrack",
            ProgressPercentage = 0,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(goal);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, MapToDto(created));
    }

    [HttpPost("{id}/track")]
    public async Task<IActionResult> TrackProgress(Guid id)
    {
        var orgId = GetOrganizationId();
        await _service.UpdateGoalProgressAsync(orgId, id);
        return Ok(new { Message = "Goal progress updated." });
    }

    private BusinessGoalDto MapToDto(BusinessGoal g)
    {
        return new BusinessGoalDto
        {
            Id = g.Id,
            OrganizationId = g.OrganizationId,
            Title = g.Title,
            Description = g.Description,
            Department = g.Department,
            StartDate = g.StartDate,
            EndDate = g.EndDate,
            KpiId = g.KpiId,
            TargetValue = g.TargetValue,
            Status = g.Status,
            ProgressPercentage = g.ProgressPercentage,
            CreatedAt = g.CreatedAt
        };
    }
}
