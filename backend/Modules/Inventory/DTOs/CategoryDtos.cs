namespace backend.Modules.Inventory.DTOs;

public record CreateCategoryDto(
    string Name,
    string Code,
    string? Description,
    Guid? ParentCategoryId
);

public record UpdateCategoryDto(
    string Name,
    string Code,
    string? Description,
    Guid? ParentCategoryId
);

public record CategoryResponseDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Code,
    string? Description,
    Guid? ParentCategoryId,
    string? ParentCategoryName,
    int ProductsCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
