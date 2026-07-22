using AutoMapper;
using backend.Modules.Workflow.DTOs;
using backend.Modules.Workflow.Entities;
using backend.Modules.Workflow.Interfaces;
using backend.Modules.Workflow.Integrations;

namespace backend.Modules.Workflow.Services;

public class IntegrationService : IIntegrationService
{
    private readonly IIntegrationRepository _repository;
    private readonly IEncryptionService _encryptionService;
    private readonly IntegrationRegistry _registry;
    private readonly IMapper _mapper;

    public IntegrationService(
        IIntegrationRepository repository,
        IEncryptionService encryptionService,
        IntegrationRegistry registry,
        IMapper mapper)
    {
        _repository = repository;
        _encryptionService = encryptionService;
        _registry = registry;
        _mapper = mapper;
    }

    public async Task<IntegrationDto?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<List<IntegrationDto>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetByOrganizationIdAsync(organizationId, cancellationToken);
        return entities.Select(MapToDto).ToList();
    }

    public async Task<IntegrationDto> CreateIntegrationAsync(Guid organizationId, CreateIntegrationDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByProviderAsync(organizationId, dto.Provider, cancellationToken);
        if (existing != null)
        {
            throw new InvalidOperationException($"An integration for provider {dto.Provider} already exists.");
        }

        var entity = new Integration
        {
            OrganizationId = organizationId,
            Provider = dto.Provider,
            DisplayName = dto.DisplayName,
            ApiKey = !string.IsNullOrEmpty(dto.ApiKey) ? _encryptionService.Encrypt(dto.ApiKey) : null,
            AccessToken = !string.IsNullOrEmpty(dto.AccessToken) ? _encryptionService.Encrypt(dto.AccessToken) : null,
            RefreshToken = !string.IsNullOrEmpty(dto.RefreshToken) ? _encryptionService.Encrypt(dto.RefreshToken) : null,
            MetadataJson = dto.MetadataJson ?? "{}",
            Status = Constants.IntegrationStatus.Active
        };

        var created = await _repository.AddAsync(entity, cancellationToken);
        return MapToDto(created);
    }

    public async Task<IntegrationDto> UpdateIntegrationAsync(Guid id, Guid organizationId, UpdateIntegrationDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, organizationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Integration with ID {id} not found.");

        entity.DisplayName = dto.DisplayName;
        entity.Status = dto.Status;

        if (!string.IsNullOrEmpty(dto.ApiKey))
            entity.ApiKey = _encryptionService.Encrypt(dto.ApiKey);

        if (!string.IsNullOrEmpty(dto.AccessToken))
            entity.AccessToken = _encryptionService.Encrypt(dto.AccessToken);

        if (!string.IsNullOrEmpty(dto.RefreshToken))
            entity.RefreshToken = _encryptionService.Encrypt(dto.RefreshToken);

        if (!string.IsNullOrEmpty(dto.MetadataJson))
            entity.MetadataJson = dto.MetadataJson;

        await _repository.UpdateAsync(entity, cancellationToken);
        return MapToDto(entity);
    }

    public async Task DeleteIntegrationAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, organizationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Integration with ID {id} not found.");

        await _repository.DeleteAsync(entity, cancellationToken);
    }

    public async Task<IntegrationTestResultDto> TestConnectionAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, organizationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Integration with ID {id} not found.");

        var provider = _registry.GetProvider(entity.Provider);
        if (provider == null)
        {
            return new IntegrationTestResultDto(false, $"Provider implementation for {entity.Provider} not found.", DateTime.UtcNow);
        }

        return await provider.TestConnectionAsync(entity, cancellationToken);
    }

    private IntegrationDto MapToDto(Integration entity)
    {
        var decryptedKey = !string.IsNullOrEmpty(entity.ApiKey) ? _encryptionService.Decrypt(entity.ApiKey) : string.Empty;
        var maskedKey = !string.IsNullOrEmpty(decryptedKey) ? _encryptionService.MaskSensitiveData(decryptedKey) : null;

        return new IntegrationDto(
            entity.Id,
            entity.OrganizationId,
            entity.Provider,
            entity.DisplayName,
            maskedKey,
            entity.Status,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt
        );
    }
}
