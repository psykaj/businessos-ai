namespace backend.DTOs.Auth;

public sealed record UserDto(
    Guid Id,
    string FullName,
    string Email,
    string Role,
    bool IsActive,
    DateTime CreatedAt,
    Guid? OrganizationId
);
