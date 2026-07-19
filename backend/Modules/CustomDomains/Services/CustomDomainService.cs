using backend.Entities;
using backend.Exceptions;
using backend.Modules.CustomDomains.DTOs;
using backend.Modules.CustomDomains.Interfaces;
using backend.Modules.CustomDomains.Repositories;

namespace backend.Modules.CustomDomains.Services;

public class CustomDomainService : ICustomDomainService
{
    private readonly ICustomDomainRepository _repository;

    public CustomDomainService(ICustomDomainRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomDomainDto>> GetDomainsAsync(Guid organizationId)
    {
        var domains = await _repository.GetAllByOrganizationIdAsync(organizationId);
        return domains.Select(MapToDto);
    }

    public async Task<CustomDomainDto> AddDomainAsync(Guid organizationId, CreateCustomDomainDto dto)
    {
        var existing = await _repository.GetByDomainAsync(dto.Domain);
        if (existing != null)
            throw new BadRequestException("Domain is already registered by an organization.");

        var allDomains = await _repository.GetAllByOrganizationIdAsync(organizationId);

        var customDomain = new CustomDomain
        {
            OrganizationId = organizationId,
            Domain = dto.Domain.ToLowerInvariant(),
            VerificationToken = Guid.NewGuid().ToString("N"),
            VerificationStatus = "Pending",
            SSLStatus = "Pending",
            IsPrimary = !allDomains.Any() // Make primary if it's the first one
        };

        await _repository.AddAsync(customDomain);
        return MapToDto(customDomain);
    }

    public async Task<CustomDomainDto> SetPrimaryDomainAsync(Guid organizationId, Guid domainId)
    {
        var domain = await _repository.GetByIdAsync(domainId, organizationId);
        if (domain == null)
            throw new NotFoundException("Domain not found.");

        if (domain.VerificationStatus != "Verified")
            throw new BadRequestException("Cannot set unverified domain as primary.");

        var allDomains = await _repository.GetAllByOrganizationIdAsync(organizationId);
        foreach (var d in allDomains)
        {
            d.IsPrimary = d.Id == domainId;
            await _repository.UpdateAsync(d);
        }

        return MapToDto(domain);
    }

    public async Task DeleteDomainAsync(Guid organizationId, Guid domainId)
    {
        var domain = await _repository.GetByIdAsync(domainId, organizationId);
        if (domain == null)
            throw new NotFoundException("Domain not found.");

        await _repository.DeleteAsync(domain);
    }

    public async Task<bool> VerifyDomainAsync(Guid organizationId, Guid domainId)
    {
        var domain = await _repository.GetByIdAsync(domainId, organizationId);
        if (domain == null)
            throw new NotFoundException("Domain not found.");

        // Here we would typically make a DNS lookup (e.g., using DnsClient.NET) 
        // to verify TXT records. For now, we mock success.
        
        // Mock verification
        domain.VerificationStatus = "Verified";
        domain.SSLStatus = "Active"; // Assuming auto-SSL via Cloudflare/Vercel
        
        await _repository.UpdateAsync(domain);
        return true;
    }

    public async Task<object> GetDnsInstructionsAsync(Guid organizationId, Guid domainId)
    {
        var domain = await _repository.GetByIdAsync(domainId, organizationId);
        if (domain == null)
            throw new NotFoundException("Domain not found.");

        return new
        {
            Domain = domain.Domain,
            Instructions = "Add a TXT record to your domain's DNS settings.",
            RecordType = "TXT",
            Name = "_businessos-verification",
            Value = domain.VerificationToken
        };
    }

    private static CustomDomainDto MapToDto(CustomDomain domain)
    {
        return new CustomDomainDto
        {
            Id = domain.Id,
            OrganizationId = domain.OrganizationId,
            Domain = domain.Domain,
            VerificationStatus = domain.VerificationStatus,
            SSLStatus = domain.SSLStatus,
            IsPrimary = domain.IsPrimary,
            CreatedAt = domain.CreatedAt
        };
    }
}
