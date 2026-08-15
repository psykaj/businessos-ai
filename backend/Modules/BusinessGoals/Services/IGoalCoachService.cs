using System;
using System.Threading;
using System.Threading.Tasks;
using backend.Modules.BusinessGoals.DTOs;

namespace backend.Modules.BusinessGoals.Services;

public interface IGoalCoachService
{
    Task<AiCoachResponseDto> GetGoalCoachingAsync(Guid organizationId, Guid goalId, CancellationToken cancellationToken = default);
}
