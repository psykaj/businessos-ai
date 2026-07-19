using backend.Entities;
using backend.Modules.Branding.DTOs;
using backend.Modules.Branding.Interfaces;
using backend.Modules.Branding.Repositories;
using backend.Modules.Media.Interfaces;
using Microsoft.AspNetCore.Http;

namespace backend.Modules.Branding.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _repository;
    private readonly IMediaService _mediaService;

    public BrandService(IBrandRepository repository, IMediaService mediaService)
    {
        _repository = repository;
        _mediaService = mediaService;
    }

    public async Task<BrandDto> GetBrandAsync(Guid organizationId)
    {
        var brand = await _repository.GetByOrganizationIdAsync(organizationId);
        
        if (brand == null)
        {
            brand = new Brand { OrganizationId = organizationId, CompanyName = "New Company" };
            await _repository.AddAsync(brand);
        }

        return MapToDto(brand);
    }

    public async Task<BrandDto> UpdateBrandAsync(Guid organizationId, UpdateBrandDto dto)
    {
        var brand = await _repository.GetByOrganizationIdAsync(organizationId);
        if (brand == null)
        {
            brand = new Brand { OrganizationId = organizationId, CompanyName = dto.CompanyName ?? "New Company" };
            await _repository.AddAsync(brand);
        }

        if (dto.CompanyName != null) brand.CompanyName = dto.CompanyName;
        if (dto.PrimaryColor != null) brand.PrimaryColor = dto.PrimaryColor;
        if (dto.SecondaryColor != null) brand.SecondaryColor = dto.SecondaryColor;
        if (dto.AccentColor != null) brand.AccentColor = dto.AccentColor;
        if (dto.FontFamily != null) brand.FontFamily = dto.FontFamily;
        if (dto.FooterText != null) brand.FooterText = dto.FooterText;
        if (dto.SupportEmail != null) brand.SupportEmail = dto.SupportEmail;

        await _repository.UpdateAsync(brand);
        return MapToDto(brand);
    }

    public async Task<string> UploadLogoAsync(Guid organizationId, IFormFile file)
    {
        var url = await _mediaService.UploadImageAsync(file, "branding/logos", organizationId);
        var brand = await _repository.GetByOrganizationIdAsync(organizationId);
        if (brand != null)
        {
            if (!string.IsNullOrEmpty(brand.LogoUrl))
            {
                await _mediaService.DeleteImageAsync(brand.LogoUrl);
            }
            brand.LogoUrl = url;
            await _repository.UpdateAsync(brand);
        }
        return url;
    }

    public async Task<string> UploadFaviconAsync(Guid organizationId, IFormFile file)
    {
        var url = await _mediaService.UploadImageAsync(file, "branding/favicons", organizationId);
        var brand = await _repository.GetByOrganizationIdAsync(organizationId);
        if (brand != null)
        {
            if (!string.IsNullOrEmpty(brand.FaviconUrl))
            {
                await _mediaService.DeleteImageAsync(brand.FaviconUrl);
            }
            brand.FaviconUrl = url;
            await _repository.UpdateAsync(brand);
        }
        return url;
    }

    private static BrandDto MapToDto(Brand brand)
    {
        return new BrandDto
        {
            Id = brand.Id,
            OrganizationId = brand.OrganizationId,
            CompanyName = brand.CompanyName,
            LogoUrl = brand.LogoUrl,
            FaviconUrl = brand.FaviconUrl,
            PrimaryColor = brand.PrimaryColor,
            SecondaryColor = brand.SecondaryColor,
            AccentColor = brand.AccentColor,
            FontFamily = brand.FontFamily,
            FooterText = brand.FooterText,
            SupportEmail = brand.SupportEmail,
            CreatedAt = brand.CreatedAt,
            UpdatedAt = brand.UpdatedAt
        };
    }
}
