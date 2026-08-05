using backend.Modules.Transfers.Entities;

namespace backend.Modules.Transfers.Interfaces;

public interface ITransferRepository
{
    Task<WarehouseTransfer?> GetByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WarehouseTransfer>> GetHistoryAsync(Guid organizationId, Guid? warehouseId = null, TransferStatus? status = null, CancellationToken cancellationToken = default);
    Task<WarehouseTransfer> AddAsync(WarehouseTransfer transfer, CancellationToken cancellationToken = default);
    Task UpdateAsync(WarehouseTransfer transfer, CancellationToken cancellationToken = default);
    Task<int> GetCountByOrgAsync(Guid organizationId, CancellationToken cancellationToken = default);
}

public interface ITransferService
{
    Task<DTOs.WarehouseTransferResponseDto> RequestTransferAsync(Guid organizationId, Guid userId, DTOs.CreateTransferRequestDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.WarehouseTransferResponseDto> ProcessApprovalAsync(Guid id, Guid organizationId, Guid userId, DTOs.TransferApprovalDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.WarehouseTransferResponseDto> UpdateTrackingAsync(Guid id, Guid organizationId, DTOs.UpdateTransferTrackingDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.WarehouseTransferResponseDto> ReceiveTransferAsync(Guid id, Guid organizationId, DTOs.ReceiveTransferDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<DTOs.WarehouseTransferResponseDto>> GetHistoryAsync(Guid organizationId, Guid? warehouseId = null, TransferStatus? status = null, CancellationToken cancellationToken = default);
    Task<DTOs.WarehouseTransferResponseDto?> GetTransferByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default);
}
