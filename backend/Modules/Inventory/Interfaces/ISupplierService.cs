using backend.Common;
using backend.Modules.Inventory.DTOs;

namespace backend.Modules.Inventory.Interfaces;

public interface ISupplierService
{
    Task<SupplierResponseDto> CreateSupplierAsync(Guid organizationId, CreateSupplierDto dto, CancellationToken cancellationToken = default);
    Task<SupplierResponseDto> UpdateSupplierAsync(Guid id, Guid organizationId, UpdateSupplierDto dto, CancellationToken cancellationToken = default);
    Task<SupplierResponseDto?> GetSupplierByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<PagedResult<SupplierResponseDto>> GetSuppliersAsync(Guid organizationId, string? query, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<SupplierResponseDto>> GetAllSuppliersAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<SupplierPerformanceDto> GetSupplierPerformanceAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
}
