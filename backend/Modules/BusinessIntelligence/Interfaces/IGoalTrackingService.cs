using backend.Modules.BusinessIntelligence.DTOs;

namespace backend.Modules.BusinessIntelligence.Interfaces;

public interface IGoalTrackingService
{
    Task<GoalDto> CreateGoalAsync(Guid organizationId, CreateGoalDto request, CancellationToken cancellationToken = default);
    Task<GoalDto> UpdateGoalAsync(Guid id, Guid organizationId, UpdateGoalDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteGoalAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<GoalDto?> GetGoalByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<GoalDto>> GetGoalsAsync(Guid organizationId, string? status = null, CancellationToken cancellationToken = default);
    Task<List<GoalDto>> SyncGoalProgressAsync(Guid organizationId, CancellationToken cancellationToken = default);
}
