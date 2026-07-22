using backend.Modules.Workflow.Constants;
using backend.Modules.Workflow.DTOs;

namespace backend.Modules.Workflow.Interfaces;

public interface IIntegrationService
{
    Task<IntegrationDto?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<List<IntegrationDto>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<IntegrationDto> CreateIntegrationAsync(Guid organizationId, CreateIntegrationDto dto, CancellationToken cancellationToken = default);
    Task<IntegrationDto> UpdateIntegrationAsync(Guid id, Guid organizationId, UpdateIntegrationDto dto, CancellationToken cancellationToken = default);
    Task DeleteIntegrationAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<IntegrationTestResultDto> TestConnectionAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
}
