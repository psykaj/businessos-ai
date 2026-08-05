using backend.Modules.Branches.Entities;

namespace backend.Modules.Branches.DTOs;

public record CreateBranchDto(
    string Name,
    string Code,
    Guid? LocationId,
    string? ContactEmail,
    string? ContactPhone,
    string? CostCenterCode,
    bool IsPrimary,
    string? WorkingHoursJson,
    string? OperationalSettingsJson
);

public record UpdateBranchDto(
    string Name,
    string Code,
    Guid? LocationId,
    string? ContactEmail,
    string? ContactPhone,
    string? CostCenterCode,
    bool IsPrimary,
    string? WorkingHoursJson,
    string? OperationalSettingsJson
);

public record AssignManagerDto(
    Guid UserId,
    string ManagerName,
    string ManagerEmail,
    string? ManagerPhone,
    bool CanApproveTransfers,
    decimal MaxTransferApprovalLimit
);

public record UpdateBranchStatusDto(
    BranchStatus Status
);

public record BranchResponseDto(
    Guid Id,
    Guid OrganizationId,
    Guid? LocationId,
    string Name,
    string Code,
    BranchStatus Status,
    string? ContactEmail,
    string? ContactPhone,
    string? CostCenterCode,
    string WorkingHoursJson,
    string OperationalSettingsJson,
    bool IsPrimary,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record BranchManagerResponseDto(
    Guid Id,
    Guid OrganizationId,
    Guid BranchId,
    Guid UserId,
    string ManagerName,
    string ManagerEmail,
    string? ManagerPhone,
    DateTime AssignedDate,
    bool CanApproveTransfers,
    decimal MaxTransferApprovalLimit,
    bool IsActive
);
