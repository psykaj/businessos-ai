using backend.Modules.BusinessIntelligence.Constants;
using backend.Modules.BusinessIntelligence.DTOs;
using backend.Modules.BusinessIntelligence.Entities;
using backend.Modules.BusinessIntelligence.Interfaces;

namespace backend.Modules.BusinessIntelligence.Services;

public class GoalTrackingService : IGoalTrackingService
{
    private readonly IGoalRepository _goalRepository;
    private readonly IKPICalculationService _kpiCalculationService;

    public GoalTrackingService(IGoalRepository goalRepository, IKPICalculationService kpiCalculationService)
    {
        _goalRepository = goalRepository;
        _kpiCalculationService = kpiCalculationService;
    }

    private GoalDto MapToDto(Goal g)
    {
        var pct = g.TargetValue > 0 ? Math.Min(100.0, Math.Round((double)(g.CurrentValue / g.TargetValue * 100m), 2)) : 0.0;
        return new GoalDto
        {
            Id = g.Id,
            OrganizationId = g.OrganizationId,
            Name = g.Name,
            TargetValue = g.TargetValue,
            CurrentValue = g.CurrentValue,
            ProgressPercentage = pct,
            StartDate = g.StartDate,
            EndDate = g.EndDate,
            Status = g.Status,
            CreatedAt = g.CreatedAt
        };
    }

    public async Task<GoalDto> CreateGoalAsync(Guid organizationId, CreateGoalDto request, CancellationToken cancellationToken = default)
    {
        var goal = new Goal
        {
            OrganizationId = organizationId,
            Name = request.Name,
            TargetValue = request.TargetValue,
            CurrentValue = request.InitialValue,
            StartDate = request.StartDate == default ? DateTime.UtcNow : request.StartDate,
            EndDate = request.EndDate == default ? DateTime.UtcNow.AddMonths(1) : request.EndDate,
            Status = BIConstants.GoalStatuses.InProgress
        };

        await _goalRepository.AddAsync(goal, cancellationToken);
        await _goalRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(goal);
    }

    public async Task<GoalDto> UpdateGoalAsync(Guid id, Guid organizationId, UpdateGoalDto request, CancellationToken cancellationToken = default)
    {
        var goal = await _goalRepository.GetByIdAsync(id, organizationId, cancellationToken);
        if (goal == null)
            throw new KeyNotFoundException($"Goal with id {id} not found");

        if (!string.IsNullOrWhiteSpace(request.Name)) goal.Name = request.Name;
        if (request.TargetValue.HasValue) goal.TargetValue = request.TargetValue.Value;
        if (request.CurrentValue.HasValue) goal.CurrentValue = request.CurrentValue.Value;
        if (request.StartDate.HasValue) goal.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue) goal.EndDate = request.EndDate.Value;
        if (!string.IsNullOrWhiteSpace(request.Status)) goal.Status = request.Status;

        // Auto calculate status based on progress
        if (goal.CurrentValue >= goal.TargetValue)
        {
            goal.Status = BIConstants.GoalStatuses.Achieved;
        }
        else if (DateTime.UtcNow > goal.EndDate && goal.CurrentValue < goal.TargetValue)
        {
            goal.Status = BIConstants.GoalStatuses.Failed;
        }

        await _goalRepository.UpdateAsync(goal, cancellationToken);
        await _goalRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(goal);
    }

    public async Task<bool> DeleteGoalAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var goal = await _goalRepository.GetByIdAsync(id, organizationId, cancellationToken);
        if (goal == null) return false;

        await _goalRepository.DeleteAsync(goal, cancellationToken);
        await _goalRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<GoalDto?> GetGoalByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var goal = await _goalRepository.GetByIdAsync(id, organizationId, cancellationToken);
        return goal == null ? null : MapToDto(goal);
    }

    public async Task<List<GoalDto>> GetGoalsAsync(Guid organizationId, string? status = null, CancellationToken cancellationToken = default)
    {
        var goals = await _goalRepository.GetAllByOrganizationIdAsync(organizationId, status, cancellationToken);

        if (goals.Count == 0)
        {
            // Seed initial organization goals if none exist
            var defaultGoals = new List<Goal>
            {
                new() { OrganizationId = organizationId, Name = "Monthly Revenue Target", TargetValue = 50000m, CurrentValue = 38500m, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(15), Status = BIConstants.GoalStatuses.InProgress },
                new() { OrganizationId = organizationId, Name = "New Lead Acquisition Target", TargetValue = 200m, CurrentValue = 165m, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(15), Status = BIConstants.GoalStatuses.InProgress },
                new() { OrganizationId = organizationId, Name = "New Customer Signups", TargetValue = 30m, CurrentValue = 28m, StartDate = DateTime.UtcNow.AddDays(-15), EndDate = DateTime.UtcNow.AddDays(15), Status = BIConstants.GoalStatuses.InProgress }
            };

            foreach (var g in defaultGoals)
            {
                await _goalRepository.AddAsync(g, cancellationToken);
            }
            await _goalRepository.SaveChangesAsync(cancellationToken);
            goals = defaultGoals;
        }

        return goals.Select(MapToDto).ToList();
    }

    public async Task<List<GoalDto>> SyncGoalProgressAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var goals = await _goalRepository.GetAllByOrganizationIdAsync(organizationId, null, cancellationToken);
        var kpis = await _kpiCalculationService.GetKPIsAsync(organizationId, null, cancellationToken);

        foreach (var goal in goals)
        {
            var matchingKpi = kpis.FirstOrDefault(k => goal.Name.ToLower().Contains(k.Name.ToLower()) || k.Name.ToLower().Contains(goal.Name.ToLower()));
            if (matchingKpi != null)
            {
                goal.CurrentValue = matchingKpi.CurrentValue;
                if (goal.CurrentValue >= goal.TargetValue)
                {
                    goal.Status = BIConstants.GoalStatuses.Achieved;
                }
                await _goalRepository.UpdateAsync(goal, cancellationToken);
            }
        }

        await _goalRepository.SaveChangesAsync(cancellationToken);
        return goals.Select(MapToDto).ToList();
    }
}
