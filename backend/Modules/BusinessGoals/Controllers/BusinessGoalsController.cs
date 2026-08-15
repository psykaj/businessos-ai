using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly IGoalProjectionService _projectionService;
    private readonly IGoalGapAnalysisService _gapAnalysisService;
    private readonly IGoalCoachService _coachService;

    public BusinessGoalsController(
        IBusinessGoalRepository repository,
        IGoalProjectionService projectionService,
        IGoalGapAnalysisService gapAnalysisService,
        IGoalCoachService coachService)
    {
        _repository = repository;
        _projectionService = projectionService;
        _gapAnalysisService = gapAnalysisService;
        _coachService = coachService;
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
        return Ok(goals.Select(MapToDto).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BusinessGoalDto>> GetById(Guid id)
    {
        var orgId = GetOrganizationId();
        var goal = await _repository.GetByIdAsync(id, orgId);
        if (goal == null) return NotFound();
        return Ok(MapToDto(goal));
    }

    [HttpGet("summary")]
    public async Task<ActionResult<GoalSummaryDto>> GetSummary()
    {
        var orgId = GetOrganizationId();
        var goals = (await _repository.GetAllAsync(orgId)).ToList();
        
        var summary = new GoalSummaryDto
        {
            TotalGoals = goals.Count,
            OnTrack = goals.Count(g => g.Status == "OnTrack"),
            AtRisk = goals.Count(g => g.Status == "AtRisk"),
            Behind = goals.Count(g => g.Status == "Behind"),
            Completed = goals.Count(g => g.Status == "Completed"),
            TotalProgress = goals.Count > 0 ? goals.Average(g => g.ProgressPercentage) : 0,
            TopPriorityGoal = goals.OrderByDescending(g => g.Priority == "High").ThenBy(g => g.TargetDate).Select(MapToDto).FirstOrDefault()
        };

        return Ok(summary);
    }

    [HttpPost]
    public async Task<ActionResult<BusinessGoalDto>> Create(CreateBusinessGoalDto dto)
    {
        var orgId = GetOrganizationId();
        var goal = new BusinessGoal
        {
            OrganizationId = orgId,
            Name = dto.Name,
            Description = dto.Description,
            GoalType = dto.GoalType,
            TargetValue = dto.TargetValue,
            CurrentValue = 0,
            Unit = dto.Unit,
            Currency = dto.Currency,
            StartDate = dto.StartDate,
            TargetDate = dto.TargetDate,
            Priority = dto.Priority,
            OwnerUserId = dto.OwnerUserId,
            Metadata = dto.Metadata,
            Status = "NotStarted",
            ProgressPercentage = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            KPIs = dto.KPIs.Select(k => new BusinessKPI
            {
                OrganizationId = orgId,
                Name = k.Name,
                KPIType = k.KPIType,
                MetricKey = k.MetricKey,
                Description = k.Description,
                TargetValue = k.TargetValue,
                CurrentValue = 0,
                Unit = k.Unit,
                Currency = k.Currency,
                Direction = k.Direction,
                Frequency = k.Frequency,
                Status = "OnTrack",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList()
        };

        var created = await _repository.AddAsync(goal);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BusinessGoalDto>> Update(Guid id, UpdateBusinessGoalDto dto)
    {
        var orgId = GetOrganizationId();
        var goal = await _repository.GetByIdAsync(id, orgId);
        if (goal == null) return NotFound();

        if (dto.Name != null) goal.Name = dto.Name;
        if (dto.Description != null) goal.Description = dto.Description;
        if (dto.GoalType != null) goal.GoalType = dto.GoalType;
        if (dto.TargetValue.HasValue) goal.TargetValue = dto.TargetValue.Value;
        if (dto.Unit != null) goal.Unit = dto.Unit;
        if (dto.Currency != null) goal.Currency = dto.Currency;
        if (dto.TargetDate.HasValue) goal.TargetDate = dto.TargetDate.Value;
        if (dto.Priority != null) goal.Priority = dto.Priority;
        if (dto.OwnerUserId.HasValue) goal.OwnerUserId = dto.OwnerUserId.Value;
        if (dto.Metadata != null) goal.Metadata = dto.Metadata;

        goal.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(goal);
        return Ok(MapToDto(goal));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var orgId = GetOrganizationId();
        var goal = await _repository.GetByIdAsync(id, orgId);
        if (goal == null) return NotFound();

        await _repository.DeleteAsync(goal);
        return NoContent();
    }

    [HttpPost("{id}/pause")]
    public async Task<IActionResult> Pause(Guid id)
    {
        var orgId = GetOrganizationId();
        var goal = await _repository.GetByIdAsync(id, orgId);
        if (goal == null) return NotFound();

        goal.Status = "Paused";
        goal.IsActive = false;
        goal.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(goal);
        return Ok(new { Message = "Goal paused." });
    }

    [HttpPost("{id}/resume")]
    public async Task<IActionResult> Resume(Guid id)
    {
        var orgId = GetOrganizationId();
        var goal = await _repository.GetByIdAsync(id, orgId);
        if (goal == null) return NotFound();

        goal.Status = "OnTrack";
        goal.IsActive = true;
        goal.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(goal);
        return Ok(new { Message = "Goal resumed." });
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var orgId = GetOrganizationId();
        var goal = await _repository.GetByIdAsync(id, orgId);
        if (goal == null) return NotFound();

        goal.Status = "Completed";
        goal.CompletedAt = DateTime.UtcNow;
        goal.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(goal);
        return Ok(new { Message = "Goal completed." });
    }

    [HttpGet("{id}/progress")]
    public async Task<ActionResult<ProjectionResult>> GetProgress(Guid id)
    {
        var orgId = GetOrganizationId();
        var goal = await _repository.GetByIdAsync(id, orgId);
        if (goal == null) return NotFound();

        var progress = await _projectionService.ProjectGoalOutcomeAsync(goal);
        return Ok(progress);
    }

    [HttpGet("{id}/analysis")]
    public async Task<ActionResult<GoalAnalysisDto>> GetAnalysis(Guid id)
    {
        var orgId = GetOrganizationId();
        var goal = await _repository.GetByIdAsync(id, orgId);
        if (goal == null) return NotFound();

        var analysis = await _gapAnalysisService.AnalyzeGapAsync(goal);
        return Ok(analysis);
    }

    [HttpGet("{id}/ai-coach")]
    public async Task<ActionResult<AiCoachResponseDto>> GetAiCoach(Guid id)
    {
        var orgId = GetOrganizationId();
        var response = await _coachService.GetGoalCoachingAsync(orgId, id);
        return Ok(response);
    }

    private BusinessGoalDto MapToDto(BusinessGoal g)
    {
        return new BusinessGoalDto
        {
            Id = g.Id,
            OrganizationId = g.OrganizationId,
            Name = g.Name,
            Description = g.Description,
            GoalType = g.GoalType,
            TargetValue = g.TargetValue,
            CurrentValue = g.CurrentValue,
            Unit = g.Unit,
            Currency = g.Currency,
            StartDate = g.StartDate,
            TargetDate = g.TargetDate,
            Status = g.Status,
            Priority = g.Priority,
            OwnerUserId = g.OwnerUserId,
            CompletedAt = g.CompletedAt,
            IsActive = g.IsActive,
            Metadata = g.Metadata,
            ProgressPercentage = g.ProgressPercentage,
            CreatedAt = g.CreatedAt,
            UpdatedAt = g.UpdatedAt,
            KPIs = g.KPIs.Select(k => new BusinessKPIDto
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
            }).ToList()
        };
    }
}
