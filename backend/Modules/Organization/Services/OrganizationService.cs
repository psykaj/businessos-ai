using backend.Exceptions;
using backend.Interfaces;
using backend.Modules.Organization.DTOs;
using backend.Modules.Organization.Interfaces;
using backend.Modules.Organization.Repositories;

namespace backend.Modules.Organization.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationService(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrganizationDto> GetOrganizationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var org = await _organizationRepository.GetByIdAsync(id, cancellationToken);
        if (org == null)
            throw new NotFoundException($"Organization with id {id} not found.");

        return MapToDto(org);
    }

    public async Task<OrganizationDto> UpdateOrganizationAsync(Guid id, UpdateOrganizationDto dto, CancellationToken cancellationToken = default)
    {
        var org = await _organizationRepository.GetByIdAsync(id, cancellationToken);
        if (org == null)
            throw new NotFoundException($"Organization with id {id} not found.");

        org.Name = dto.Name;
        org.Industry = dto.Industry;
        org.Website = dto.Website;
        org.Address = dto.Address;
        org.TimeZone = dto.TimeZone;
        org.Language = dto.Language;
        org.Currency = dto.Currency;

        _organizationRepository.Update(org);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToDto(org);
    }

    public async Task<string> UploadLogoAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default)
    {
        var org = await _organizationRepository.GetByIdAsync(id, cancellationToken);
        if (org == null)
            throw new NotFoundException($"Organization with id {id} not found.");

        // For this task, we can just simulate file upload and return a dummy URL. 
        // A real implementation would use Azure Blob Storage, AWS S3, or similar.
        var fileUrl = $"https://storage.businessos.ai/logos/{id}/{file.FileName}";
        
        org.LogoUrl = fileUrl;
        _organizationRepository.Update(org);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return fileUrl;
    }

    private static OrganizationDto MapToDto(Entities.Organization org)
    {
        return new OrganizationDto
        {
            Id = org.Id,
            Name = org.Name,
            Slug = org.Slug,
            Industry = org.Industry,
            LogoUrl = org.LogoUrl,
            Website = org.Website,
            Address = org.Address,
            TimeZone = org.TimeZone,
            Language = org.Language,
            Currency = org.Currency,
            SubscriptionId = org.SubscriptionId,
            IsActive = org.IsActive,
            CreatedAt = org.CreatedAt,
            UpdatedAt = org.UpdatedAt
        };
    }
}
