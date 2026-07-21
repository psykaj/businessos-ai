using backend.Entities;
using backend.Exceptions;
using backend.Interfaces;
using backend.Modules.Campaigns.DTOs;
using backend.Modules.Campaigns.Interfaces;

namespace backend.Modules.Campaigns.Services;

public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CampaignService(ICampaignRepository campaignRepository, IUnitOfWork unitOfWork)
    {
        _campaignRepository = campaignRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<(IReadOnlyList<CampaignDto> Items, int TotalCount)> GetCampaignsPagedAsync(Guid organizationId, int pageNumber, int pageSize, string? search, string? status, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _campaignRepository.GetCampaignsPagedAsync(organizationId, pageNumber, pageSize, search, status, cancellationToken);
        var dtos = items.Select(MapToDto).ToList();
        return (dtos, totalCount);
    }

    public async Task<CampaignDto> GetCampaignAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);
        if (campaign == null || campaign.OrganizationId != organizationId)
            throw new NotFoundException("Campaign not found");

        return MapToDto(campaign);
    }

    public async Task<CampaignDto> CreateCampaignAsync(Guid organizationId, CreateCampaignDto dto, CancellationToken cancellationToken = default)
    {
        var campaign = new Campaign
        {
            OrganizationId = organizationId,
            Name = dto.Name,
            CampaignType = dto.CampaignType,
            Source = dto.Source,
            Medium = dto.Medium,
            Budget = dto.Budget,
            Status = "Draft"
        };

        await _campaignRepository.AddAsync(campaign, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToDto(campaign);
    }

    public async Task<CampaignDto> UpdateCampaignAsync(Guid organizationId, Guid id, UpdateCampaignDto dto, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);
        if (campaign == null || campaign.OrganizationId != organizationId)
            throw new NotFoundException("Campaign not found");

        campaign.Name = dto.Name;
        campaign.CampaignType = dto.CampaignType;
        campaign.Source = dto.Source;
        campaign.Medium = dto.Medium;
        campaign.Budget = dto.Budget;
        
        if (!string.IsNullOrWhiteSpace(dto.Status))
        {
            campaign.Status = dto.Status;
        }

        _campaignRepository.Update(campaign);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToDto(campaign);
    }

    public async Task DeleteCampaignAsync(Guid organizationId, Guid id, CancellationToken cancellationToken = default)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);
        if (campaign == null || campaign.OrganizationId != organizationId)
            throw new NotFoundException("Campaign not found");

        _campaignRepository.Delete(campaign);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    private static CampaignDto MapToDto(Campaign c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        CampaignType = c.CampaignType,
        Source = c.Source,
        Medium = c.Medium,
        Budget = c.Budget,
        Status = c.Status,
        CreatedAt = c.CreatedAt
    };
}
