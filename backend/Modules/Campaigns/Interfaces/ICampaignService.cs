using backend.Modules.Campaigns.DTOs;

namespace backend.Modules.Campaigns.Interfaces;

public interface ICampaignService
{
    Task<(IReadOnlyList<CampaignDto> Items, int TotalCount)> GetCampaignsPagedAsync(Guid organizationId, int pageNumber, int pageSize, string? search, string? status, CancellationToken cancellationToken = default);
    Task<CampaignDto> GetCampaignAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default);
    Task<CampaignDto> CreateCampaignAsync(Guid organizationId, CreateCampaignDto dto, CancellationToken cancellationToken = default);
    Task<CampaignDto> UpdateCampaignAsync(Guid organizationId, Guid id, UpdateCampaignDto dto, CancellationToken cancellationToken = default);
    Task DeleteCampaignAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default);
}
