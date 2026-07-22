using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Entities;

namespace backend.Modules.Workflow.Integrations;

public interface IIntegrationProvider
{
    IntegrationProvider Provider { get; }
    Task<IntegrationTestResultDto> TestConnectionAsync(Integration integration, CancellationToken cancellationToken = default);
    Task<Dictionary<string, string>> ExecuteActionAsync(Integration integration, string actionName, Dictionary<string, string> parameters, CancellationToken cancellationToken = default);
}
