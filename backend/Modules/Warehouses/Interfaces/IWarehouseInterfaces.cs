using backend.Modules.Warehouses.Entities;

namespace backend.Modules.Warehouses.Interfaces;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Warehouse>> GetAllByOrgAsync(Guid organizationId, Guid? branchId = null, CancellationToken cancellationToken = default);
    Task<Warehouse> AddAsync(Warehouse warehouse, CancellationToken cancellationToken = default);
    Task UpdateAsync(Warehouse warehouse, CancellationToken cancellationToken = default);
    Task DeleteAsync(Warehouse warehouse, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
}

public interface IWarehouseService
{
    Task<IEnumerable<DTOs.WarehouseResponseDto>> GetWarehousesAsync(Guid organizationId, Guid? branchId = null, CancellationToken cancellationToken = default);
    Task<DTOs.WarehouseResponseDto?> GetWarehouseByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<DTOs.WarehouseResponseDto> CreateWarehouseAsync(Guid organizationId, DTOs.CreateWarehouseDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.WarehouseResponseDto> UpdateWarehouseAsync(Guid id, Guid organizationId, DTOs.UpdateWarehouseDto dto, CancellationToken cancellationToken = default);
    Task DeleteWarehouseAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
}
