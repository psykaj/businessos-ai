using backend.Modules.Themes.DTOs;

namespace backend.Modules.Themes.Interfaces;

public interface IThemeService
{
    Task<IEnumerable<ThemeDto>> GetThemesAsync(Guid organizationId);
    Task<ThemeDto> GetThemeByIdAsync(Guid organizationId, Guid id);
    Task<ThemeDto> CreateThemeAsync(Guid organizationId, CreateThemeDto dto);
    Task<ThemeDto> UpdateThemeAsync(Guid organizationId, Guid id, UpdateThemeDto dto);
    Task<ThemeDto> SetDefaultThemeAsync(Guid organizationId, Guid id);
    Task DeleteThemeAsync(Guid organizationId, Guid id);
}
