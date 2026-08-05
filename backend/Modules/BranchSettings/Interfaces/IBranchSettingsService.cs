using backend.Modules.BranchSettings.DTOs;

namespace backend.Modules.BranchSettings.Interfaces;

public interface IBranchSettingsService
{
    Task<BranchConfigurationResponseDto> GetConfigurationAsync(Guid branchId, Guid organizationId, CancellationToken cancellationToken = default);
    Task<BranchConfigurationResponseDto> UpdateConfigurationAsync(Guid branchId, Guid organizationId, UpdateBranchConfigurationDto dto, CancellationToken cancellationToken = default);
}
