using backend.Entities;
using backend.Exceptions;
using backend.Modules.Themes.DTOs;
using backend.Modules.Themes.Interfaces;
using backend.Modules.Themes.Repositories;

namespace backend.Modules.Themes.Services;

public class ThemeService : IThemeService
{
    private readonly IThemeRepository _repository;

    public ThemeService(IThemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ThemeDto>> GetThemesAsync(Guid organizationId)
    {
        var themes = await _repository.GetAllByOrganizationIdAsync(organizationId);
        return themes.Select(MapToDto);
    }

    public async Task<ThemeDto> GetThemeByIdAsync(Guid organizationId, Guid id)
    {
        var theme = await _repository.GetByIdAsync(id, organizationId);
        if (theme == null) throw new NotFoundException("Theme not found.");
        return MapToDto(theme);
    }

    public async Task<ThemeDto> CreateThemeAsync(Guid organizationId, CreateThemeDto dto)
    {
        var allThemes = await _repository.GetAllByOrganizationIdAsync(organizationId);
        
        var theme = new Theme
        {
            OrganizationId = organizationId,
            ThemeName = dto.ThemeName,
            ThemeMode = dto.ThemeMode,
            ThemeJson = dto.ThemeJson,
            IsDefault = !allThemes.Any() // make default if first
        };

        await _repository.AddAsync(theme);
        return MapToDto(theme);
    }

    public async Task<ThemeDto> UpdateThemeAsync(Guid organizationId, Guid id, UpdateThemeDto dto)
    {
        var theme = await _repository.GetByIdAsync(id, organizationId);
        if (theme == null) throw new NotFoundException("Theme not found.");

        if (dto.ThemeName != null) theme.ThemeName = dto.ThemeName;
        if (dto.ThemeMode != null) theme.ThemeMode = dto.ThemeMode;
        if (dto.ThemeJson != null) theme.ThemeJson = dto.ThemeJson;

        await _repository.UpdateAsync(theme);
        return MapToDto(theme);
    }

    public async Task<ThemeDto> SetDefaultThemeAsync(Guid organizationId, Guid id)
    {
        var theme = await _repository.GetByIdAsync(id, organizationId);
        if (theme == null) throw new NotFoundException("Theme not found.");

        var allThemes = await _repository.GetAllByOrganizationIdAsync(organizationId);
        foreach (var t in allThemes)
        {
            t.IsDefault = t.Id == id;
            await _repository.UpdateAsync(t);
        }

        return MapToDto(theme);
    }

    public async Task DeleteThemeAsync(Guid organizationId, Guid id)
    {
        var theme = await _repository.GetByIdAsync(id, organizationId);
        if (theme == null) throw new NotFoundException("Theme not found.");
        if (theme.IsDefault) throw new BadRequestException("Cannot delete default theme.");

        await _repository.DeleteAsync(theme);
    }

    private static ThemeDto MapToDto(Theme theme)
    {
        return new ThemeDto
        {
            Id = theme.Id,
            OrganizationId = theme.OrganizationId,
            ThemeName = theme.ThemeName,
            ThemeMode = theme.ThemeMode,
            ThemeJson = theme.ThemeJson,
            IsDefault = theme.IsDefault,
            CreatedAt = theme.CreatedAt,
            UpdatedAt = theme.UpdatedAt
        };
    }
}
