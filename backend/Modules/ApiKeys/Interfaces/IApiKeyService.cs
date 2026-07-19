using backend.Modules.ApiKeys.DTOs;

namespace backend.Modules.ApiKeys.Interfaces;

public interface IApiKeyService
{
    Task<ApiKeyResponseDto> GenerateApiKeyAsync(Guid organizationId, string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApiKeyDto>> ListApiKeysAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task RevokeApiKeyAsync(Guid organizationId, Guid keyId, CancellationToken cancellationToken = default);
    Task<ApiKeyResponseDto> RotateApiKeyAsync(Guid organizationId, Guid keyId, CancellationToken cancellationToken = default);
}
