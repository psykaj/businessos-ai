namespace backend.Modules.BranchSettings.DTOs;

public record BranchConfigurationResponseDto(
    Guid BranchId,
    string BranchName,
    string BranchCode,
    string WorkingHoursJson,
    string OperationalSettingsJson
);

public record UpdateBranchConfigurationDto(
    string WorkingHoursJson,
    string OperationalSettingsJson
);
