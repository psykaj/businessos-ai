using backend.Entities;
using backend.Exceptions;
using backend.Modules.LandingPages.DTOs;
using backend.Modules.LandingPages.Interfaces;
using backend.Modules.LandingPages.Repositories;

namespace backend.Modules.LandingPages.Services;

public class LandingPageService : ILandingPageService
{
    private readonly ILandingPageRepository _repository;

    public LandingPageService(ILandingPageRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LandingPageDto>> GetPagesAsync(Guid organizationId)
    {
        var pages = await _repository.GetAllByOrganizationIdAsync(organizationId);
        return pages.Select(MapToDto);
    }

    public async Task<LandingPageDto> GetPageByIdAsync(Guid organizationId, Guid id)
    {
        var page = await _repository.GetByIdAsync(id, organizationId);
        if (page == null) throw new NotFoundException("Landing page not found.");
        return MapToDto(page);
    }

    public async Task<LandingPageDto> GetPageBySlugAsync(Guid organizationId, string slug)
    {
        var page = await _repository.GetBySlugAsync(slug, organizationId);
        if (page == null) throw new NotFoundException("Landing page not found.");
        return MapToDto(page);
    }

    public async Task<LandingPageDto> CreatePageAsync(Guid organizationId, CreateLandingPageDto dto)
    {
        var existing = await _repository.GetBySlugAsync(dto.Slug, organizationId);
        if (existing != null) throw new BadRequestException("Slug already in use.");

        var page = new LandingPage
        {
            OrganizationId = organizationId,
            Title = dto.Title,
            Slug = dto.Slug,
            Status = "Draft"
        };

        await _repository.AddAsync(page);
        return MapToDto(page);
    }

    public async Task<LandingPageDto> UpdatePageAsync(Guid organizationId, Guid id, UpdateLandingPageDto dto)
    {
        var page = await _repository.GetByIdAsync(id, organizationId);
        if (page == null) throw new NotFoundException("Landing page not found.");

        if (dto.Slug != null && dto.Slug != page.Slug)
        {
            var existing = await _repository.GetBySlugAsync(dto.Slug, organizationId);
            if (existing != null) throw new BadRequestException("Slug already in use.");
            page.Slug = dto.Slug;
        }

        if (dto.Title != null) page.Title = dto.Title;
        
        if (dto.Status != null) 
        {
            page.Status = dto.Status;
            if (dto.Status == "Published" && page.PublishedAt == null)
            {
                page.PublishedAt = DateTime.UtcNow;
            }
        }

        if (dto.Sections != null)
        {
            // Simple approach: clear existing sections and recreate them to match the new order/content.
            // A more complex approach would sync them by ID.
            page.Sections.Clear();
            foreach (var secDto in dto.Sections.OrderBy(s => s.SortOrder))
            {
                page.Sections.Add(new LandingPageSection
                {
                    LandingPageId = page.Id,
                    SectionType = secDto.SectionType,
                    SortOrder = secDto.SortOrder,
                    ContentJson = secDto.ContentJson
                });
            }
        }

        await _repository.UpdateAsync(page);
        return MapToDto(page);
    }

    public async Task DeletePageAsync(Guid organizationId, Guid id)
    {
        var page = await _repository.GetByIdAsync(id, organizationId);
        if (page == null) throw new NotFoundException("Landing page not found.");

        await _repository.DeleteAsync(page);
    }

    private static LandingPageDto MapToDto(LandingPage page)
    {
        return new LandingPageDto
        {
            Id = page.Id,
            OrganizationId = page.OrganizationId,
            Title = page.Title,
            Slug = page.Slug,
            Status = page.Status,
            PublishedAt = page.PublishedAt,
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt,
            Sections = page.Sections.Select(s => new LandingPageSectionDto
            {
                Id = s.Id,
                SectionType = s.SectionType,
                SortOrder = s.SortOrder,
                ContentJson = s.ContentJson
            }).ToList()
        };
    }
}
