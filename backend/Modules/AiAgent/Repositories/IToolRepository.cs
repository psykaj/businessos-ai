using backend.Modules.AiAgent.Entities;

namespace backend.Modules.AiAgent.Repositories;

public interface IToolRepository
{
    Task<ToolDefinition?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<ToolDefinition>> GetAllAsync(bool enabledOnly = true, CancellationToken cancellationToken = default);
    Task AddAsync(ToolDefinition tool, CancellationToken cancellationToken = default);
    void Update(ToolDefinition tool);
    Task UpsertToolAsync(ToolDefinition tool, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
