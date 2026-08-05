namespace backend.Modules.Locations.DTOs;

public record CreateLocationDto(
    string Name,
    string Region,
    string Country,
    string? State,
    string City,
    string Address,
    string? PostalCode,
    string TimeZone,
    string Currency,
    double? Latitude,
    double? Longitude
);

public record UpdateLocationDto(
    string Name,
    string Region,
    string Country,
    string? State,
    string City,
    string Address,
    string? PostalCode,
    string TimeZone,
    string Currency,
    double? Latitude,
    double? Longitude,
    bool IsActive
);

public record LocationResponseDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Region,
    string Country,
    string? State,
    string City,
    string Address,
    string? PostalCode,
    string TimeZone,
    string Currency,
    double? Latitude,
    double? Longitude,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
