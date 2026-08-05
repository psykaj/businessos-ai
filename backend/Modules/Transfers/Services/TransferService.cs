using AutoMapper;
using backend.Exceptions;
using backend.Modules.Branches.Interfaces;
using backend.Modules.Transfers.DTOs;
using backend.Modules.Transfers.Entities;
using backend.Modules.Transfers.Interfaces;
using backend.Modules.Warehouses.Interfaces;
using System.Text.Json;

namespace backend.Modules.Transfers.Services;

public class TransferService : ITransferService
{
    private readonly ITransferRepository _repository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public TransferService(ITransferRepository repository, IWarehouseRepository warehouseRepository, IBranchRepository branchRepository, IMapper mapper)
    {
        _repository = repository;
        _warehouseRepository = warehouseRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<WarehouseTransferResponseDto> RequestTransferAsync(Guid organizationId, Guid userId, CreateTransferRequestDto dto, CancellationToken cancellationToken = default)
    {
        var sourceWh = await _warehouseRepository.GetByIdAsync(dto.SourceWarehouseId, organizationId, cancellationToken);
        if (sourceWh == null) throw new NotFoundException("Source Warehouse", dto.SourceWarehouseId);

        var destWh = await _warehouseRepository.GetByIdAsync(dto.DestinationWarehouseId, organizationId, cancellationToken);
        if (destWh == null) throw new NotFoundException("Destination Warehouse", dto.DestinationWarehouseId);

        int totalQty = dto.Items.Sum(x => x.Quantity);
        decimal totalValue = dto.Items.Sum(x => x.Quantity * x.UnitPrice);

        var count = await _repository.GetCountByOrgAsync(organizationId, cancellationToken);
        string transferNo = $"TRF-{DateTime.UtcNow.Year}-{(count + 1):D4}";

        var entity = new WarehouseTransfer
        {
            OrganizationId = organizationId,
            SourceWarehouseId = dto.SourceWarehouseId,
            DestinationWarehouseId = dto.DestinationWarehouseId,
            TransferNumber = transferNo,
            Status = TransferStatus.Pending,
            ApprovalStatus = "Pending",
            RequestedById = userId,
            RequestedByName = dto.RequestedByName,
            TransferredItemsJson = JsonSerializer.Serialize(dto.Items),
            TotalItemsCount = totalQty,
            TotalTransferValue = totalValue,
            RequestedAt = DateTime.UtcNow,
            TrackingNotes = dto.Notes
        };

        // Check if auto-approval threshold applies or manager is directly requesting
        var sourceManagers = await _branchRepository.GetManagersByBranchAsync(sourceWh.BranchId, organizationId, cancellationToken);
        var manager = sourceManagers.FirstOrDefault(m => m.UserId == userId && m.CanApproveTransfers);
        if (manager != null && totalValue <= manager.MaxTransferApprovalLimit)
        {
            entity.Status = TransferStatus.Approved;
            entity.ApprovalStatus = "AutoApproved";
            entity.ApprovedById = userId;
            entity.ApprovedByName = dto.RequestedByName + " (Auto)";
            entity.ApprovedAt = DateTime.UtcNow;
        }

        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<WarehouseTransferResponseDto>(entity);
    }

    public async Task<WarehouseTransferResponseDto> ProcessApprovalAsync(Guid id, Guid organizationId, Guid userId, TransferApprovalDto dto, CancellationToken cancellationToken = default)
    {
        var transfer = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (transfer == null) throw new NotFoundException("WarehouseTransfer", id);

        if (transfer.Status != TransferStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot change approval status for transfer in '{transfer.Status}' status.");
        }

        if (dto.IsApproved)
        {
            transfer.Status = TransferStatus.Approved;
            transfer.ApprovalStatus = "Approved";
            transfer.ApprovedById = userId;
            transfer.ApprovedByName = dto.ApprovedByName;
            transfer.ApprovedAt = DateTime.UtcNow;
        }
        else
        {
            transfer.Status = TransferStatus.Rejected;
            transfer.ApprovalStatus = "Rejected";
            transfer.RejectionReason = dto.RejectionReason;
        }

        await _repository.UpdateAsync(transfer, cancellationToken);
        return _mapper.Map<WarehouseTransferResponseDto>(transfer);
    }

    public async Task<WarehouseTransferResponseDto> UpdateTrackingAsync(Guid id, Guid organizationId, UpdateTransferTrackingDto dto, CancellationToken cancellationToken = default)
    {
        var transfer = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (transfer == null) throw new NotFoundException("WarehouseTransfer", id);

        transfer.Status = dto.Status;
        if (dto.Status == TransferStatus.InTransit && !transfer.ShippedAt.HasValue)
        {
            transfer.ShippedAt = DateTime.UtcNow;
        }
        if (!string.IsNullOrEmpty(dto.TrackingNotes))
        {
            transfer.TrackingNotes = string.IsNullOrEmpty(transfer.TrackingNotes) 
                ? dto.TrackingNotes 
                : transfer.TrackingNotes + $" | {DateTime.UtcNow:MM/dd/yy}: {dto.TrackingNotes}";
        }

        await _repository.UpdateAsync(transfer, cancellationToken);
        return _mapper.Map<WarehouseTransferResponseDto>(transfer);
    }

    public async Task<WarehouseTransferResponseDto> ReceiveTransferAsync(Guid id, Guid organizationId, ReceiveTransferDto dto, CancellationToken cancellationToken = default)
    {
        var transfer = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        if (transfer == null) throw new NotFoundException("WarehouseTransfer", id);

        if (transfer.Status == TransferStatus.Received)
        {
            return _mapper.Map<WarehouseTransferResponseDto>(transfer);
        }

        transfer.Status = TransferStatus.Received;
        transfer.ReceivedAt = DateTime.UtcNow;
        if (!string.IsNullOrEmpty(dto.ReceiptNotes))
        {
            transfer.TrackingNotes = (transfer.TrackingNotes ?? "") + $" | Received: {dto.ReceiptNotes}";
        }

        await _repository.UpdateAsync(transfer, cancellationToken);

        // Update destination warehouse stock values and utilization
        var destWh = await _warehouseRepository.GetByIdAsync(transfer.DestinationWarehouseId, organizationId, cancellationToken);
        if (destWh != null)
        {
            destWh.TotalStockItemsCount += transfer.TotalItemsCount;
            destWh.EstimatedStockValue += transfer.TotalTransferValue;
            if (destWh.StorageCapacitySqFt > 0)
            {
                // Simple utilization modeling based on inventory density
                var calcUtil = Math.Min(100m, (destWh.TotalStockItemsCount * 2.5m / destWh.StorageCapacitySqFt) * 100m);
                destWh.CurrentUtilizationPercentage = Math.Round(calcUtil, 2);
            }
            await _warehouseRepository.UpdateAsync(destWh, cancellationToken);
        }

        // Deduct from source warehouse
        var sourceWh = await _warehouseRepository.GetByIdAsync(transfer.SourceWarehouseId, organizationId, cancellationToken);
        if (sourceWh != null)
        {
            sourceWh.TotalStockItemsCount = Math.Max(0, sourceWh.TotalStockItemsCount - transfer.TotalItemsCount);
            sourceWh.EstimatedStockValue = Math.Max(0m, sourceWh.EstimatedStockValue - transfer.TotalTransferValue);
            if (sourceWh.StorageCapacitySqFt > 0)
            {
                var calcUtil = Math.Min(100m, (sourceWh.TotalStockItemsCount * 2.5m / sourceWh.StorageCapacitySqFt) * 100m);
                sourceWh.CurrentUtilizationPercentage = Math.Round(calcUtil, 2);
            }
            await _warehouseRepository.UpdateAsync(sourceWh, cancellationToken);
        }

        return _mapper.Map<WarehouseTransferResponseDto>(transfer);
    }

    public async Task<IEnumerable<WarehouseTransferResponseDto>> GetHistoryAsync(Guid organizationId, Guid? warehouseId = null, TransferStatus? status = null, CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetHistoryAsync(organizationId, warehouseId, status, cancellationToken);
        return _mapper.Map<IEnumerable<WarehouseTransferResponseDto>>(list);
    }

    public async Task<WarehouseTransferResponseDto?> GetTransferByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var transfer = await _repository.GetByIdAsync(id, organizationId, cancellationToken);
        return transfer != null ? _mapper.Map<WarehouseTransferResponseDto>(transfer) : null;
    }
}
