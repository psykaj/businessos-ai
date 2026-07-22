using backend.Modules.Workflow.Constants;

namespace backend.Modules.Workflow.DTOs;

public record CreateIntegrationDto(
    IntegrationProvider Provider,
    string DisplayName,
    string? ApiKey,
    string? AccessToken,
    string? RefreshToken,
    string? MetadataJson
);

public record UpdateIntegrationDto(
    string DisplayName,
    string? ApiKey,
    string? AccessToken,
    string? RefreshToken,
    IntegrationStatus Status,
    string? MetadataJson
);

public record IntegrationDto(
    Guid Id,
    Guid OrganizationId,
    IntegrationProvider Provider,
    string DisplayName,
    string? MaskedApiKey,
    IntegrationStatus Status,
    string MetadataJson,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record IntegrationTestResultDto(
    bool Success,
    string Message,
    DateTime TestedAt
);
