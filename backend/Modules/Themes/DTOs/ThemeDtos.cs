namespace backend.Modules.Themes.DTOs;

public class ThemeDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string ThemeName { get; set; } = string.Empty;
    public string ThemeMode { get; set; } = string.Empty;
    public string ThemeJson { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateThemeDto
{
    public string ThemeName { get; set; } = string.Empty;
    public string ThemeMode { get; set; } = "Light";
    public string ThemeJson { get; set; } = "{}";
}

public class UpdateThemeDto
{
    public string? ThemeName { get; set; }
    public string? ThemeMode { get; set; }
    public string? ThemeJson { get; set; }
}
