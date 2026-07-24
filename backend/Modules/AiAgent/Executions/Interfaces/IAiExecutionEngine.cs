using backend.Modules.AiAgent.Executions.DTOs;

namespace backend.Modules.AiAgent.Executions.Interfaces;

public interface IAiExecutionEngine
{
    Task<ExecutionResponseDto> ExecuteCommandAsync(
        Guid organizationId,
        string userId,
        ExecuteCommandRequestDto request,
        CancellationToken cancellationToken = default);
}
