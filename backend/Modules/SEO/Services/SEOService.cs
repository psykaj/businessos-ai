using backend.Entities;
using backend.Modules.SEO.DTOs;
using backend.Modules.SEO.Interfaces;
using backend.Modules.SEO.Repositories;

namespace backend.Modules.SEO.Services;

public class SEOService : ISEOService
{
    private readonly ISEORepository _repository;

    public SEOService(ISEORepository repository)
    {
        _repository = repository;
    }

    public async Task<SEODto> GetSEOAsync(Guid organizationId)
    {
        var seo = await _repository.GetByOrganizationIdAsync(organizationId);
        
        if (seo == null)
        {
            seo = new SEOSettings { OrganizationId = organizationId };
            await _repository.AddAsync(seo);
        }

        return MapToDto(seo);
    }

    public async Task<SEODto> UpdateSEOAsync(Guid organizationId, UpdateSEODto dto)
    {
        var seo = await _repository.GetByOrganizationIdAsync(organizationId);
        if (seo == null)
        {
            seo = new SEOSettings { OrganizationId = organizationId };
            await _repository.AddAsync(seo);
        }

        if (dto.MetaTitle != null) seo.MetaTitle = dto.MetaTitle;
        if (dto.MetaDescription != null) seo.MetaDescription = dto.MetaDescription;
        if (dto.Keywords != null) seo.Keywords = dto.Keywords;
        if (dto.CanonicalUrl != null) seo.CanonicalUrl = dto.CanonicalUrl;
        if (dto.OpenGraphTitle != null) seo.OpenGraphTitle = dto.OpenGraphTitle;
        if (dto.OpenGraphDescription != null) seo.OpenGraphDescription = dto.OpenGraphDescription;
        if (dto.OpenGraphImage != null) seo.OpenGraphImage = dto.OpenGraphImage;
        if (dto.TwitterCard != null) seo.TwitterCard = dto.TwitterCard;
        if (dto.Robots != null) seo.Robots = dto.Robots;

        await _repository.UpdateAsync(seo);
        return MapToDto(seo);
    }

    private static SEODto MapToDto(SEOSettings seo)
    {
        return new SEODto
        {
            Id = seo.Id,
            OrganizationId = seo.OrganizationId,
            MetaTitle = seo.MetaTitle,
            MetaDescription = seo.MetaDescription,
            Keywords = seo.Keywords,
            CanonicalUrl = seo.CanonicalUrl,
            OpenGraphTitle = seo.OpenGraphTitle,
            OpenGraphDescription = seo.OpenGraphDescription,
            OpenGraphImage = seo.OpenGraphImage,
            TwitterCard = seo.TwitterCard,
            Robots = seo.Robots,
            CreatedAt = seo.CreatedAt,
            UpdatedAt = seo.UpdatedAt
        };
    }
}
