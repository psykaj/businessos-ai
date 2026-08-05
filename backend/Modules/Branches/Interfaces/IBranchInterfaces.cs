using backend.Modules.Branches.Entities;

namespace backend.Modules.Branches.Interfaces;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Branch>> GetAllByOrgAsync(Guid organizationId, Guid? locationId = null, BranchStatus? status = null, CancellationToken cancellationToken = default);
    Task<Branch> AddAsync(Branch branch, CancellationToken cancellationToken = default);
    Task UpdateAsync(Branch branch, CancellationToken cancellationToken = default);
    Task DeleteAsync(Branch branch, CancellationToken cancellationToken = default);
    Task<bool> CodeExistsAsync(string code, Guid organizationId, Guid? excludeId = null, CancellationToken cancellationToken = default);
    
    // Manager operations
    Task<BranchManager> AddManagerAsync(BranchManager manager, CancellationToken cancellationToken = default);
    Task<IEnumerable<BranchManager>> GetManagersByBranchAsync(Guid branchId, Guid organizationId, CancellationToken cancellationToken = default);
    Task<BranchManager?> GetManagerAsync(Guid branchId, Guid userId, Guid organizationId, CancellationToken cancellationToken = default);
    Task RemoveManagerAsync(BranchManager manager, CancellationToken cancellationToken = default);
}

public interface IBranchService
{
    Task<IEnumerable<DTOs.BranchResponseDto>> GetBranchesAsync(Guid organizationId, Guid? locationId = null, BranchStatus? status = null, CancellationToken cancellationToken = default);
    Task<DTOs.BranchResponseDto?> GetBranchByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<DTOs.BranchResponseDto> CreateBranchAsync(Guid organizationId, DTOs.CreateBranchDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.BranchResponseDto> UpdateBranchAsync(Guid id, Guid organizationId, DTOs.UpdateBranchDto dto, CancellationToken cancellationToken = default);
    Task DeleteBranchAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<DTOs.BranchResponseDto> UpdateBranchStatusAsync(Guid id, Guid organizationId, DTOs.UpdateBranchStatusDto dto, CancellationToken cancellationToken = default);
    
    Task<DTOs.BranchManagerResponseDto> AssignManagerAsync(Guid branchId, Guid organizationId, DTOs.AssignManagerDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<DTOs.BranchManagerResponseDto>> GetBranchManagersAsync(Guid branchId, Guid organizationId, CancellationToken cancellationToken = default);
    Task RemoveManagerAsync(Guid branchId, Guid userId, Guid organizationId, CancellationToken cancellationToken = default);
}
