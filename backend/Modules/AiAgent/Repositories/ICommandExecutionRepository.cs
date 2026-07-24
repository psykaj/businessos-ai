using backend.Modules.AiAgent.Entities;

namespace backend.Modules.AiAgent.Repositories;

public interface ICommandExecutionRepository
{
    Task<CommandExecution?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<CommandExecution>> GetRecentByOrganizationIdAsync(Guid organizationId, int limit = 20, CancellationToken cancellationToken = default);
    Task<(List<CommandExecution> Items, int TotalCount)> GetPagedAsync(
        Guid organizationId,
        int page = 1,
        int pageSize = 20,
        string? status = null,
        string? tool = null,
        CancellationToken cancellationToken = default);
    Task AddAsync(CommandExecution execution, CancellationToken cancellationToken = default);
    void Update(CommandExecution execution);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
