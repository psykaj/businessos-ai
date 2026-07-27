using AutoMapper;
using backend.Common;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Enums;
using backend.Modules.Inventory.Interfaces;

namespace backend.Modules.Inventory.Services;

public class StockMovementService
{
    private readonly IStockRepository _stockRepository;
    private readonly IStockMovementRepository _movementRepository;
    private readonly IStockAdjustmentRepository _adjustmentRepository;
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IMapper _mapper;

    public StockMovementService(
        IStockRepository stockRepository,
        IStockMovementRepository movementRepository,
        IStockAdjustmentRepository adjustmentRepository,
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IMapper mapper)
    {
        _stockRepository = stockRepository;
        _movementRepository = movementRepository;
        _adjustmentRepository = adjustmentRepository;
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }

    public async Task<StockMovementResponseDto> RecordStockInAsync(Guid organizationId, StockOperationRequestDto dto, string userId, CancellationToken cancellationToken = default)
    {
        if (dto.Quantity <= 0) throw new ArgumentException("Quantity for Stock In must be greater than zero.");

        var stock = await _stockRepository.GetByProductAndWarehouseAsync(dto.ProductId, dto.WarehouseId, organizationId, cancellationToken);
        if (stock == null)
        {
            stock = new InventoryStock
            {
                OrganizationId = organizationId,
                ProductId = dto.ProductId,
                WarehouseId = dto.WarehouseId,
                QuantityOnHand = 0,
                QuantityReserved = 0
            };
            await _stockRepository.AddAsync(stock, cancellationToken);
        }

        stock.QuantityOnHand += dto.Quantity;
        _stockRepository.Update(stock);

        var movement = new StockMovement
        {
            OrganizationId = organizationId,
            ProductId = dto.ProductId,
            DestinationWarehouseId = dto.WarehouseId,
            MovementType = MovementType.StockIn,
            Quantity = dto.Quantity,
            ReferenceType = dto.ReferenceType ?? "ManualStockIn",
            ReferenceId = dto.ReferenceId,
            UnitCost = dto.UnitCost,
            BatchNumber = dto.BatchNumber,
            ExpiryDate = dto.ExpiryDate,
            Reason = dto.Reason,
            MovementDate = DateTime.UtcNow,
            CreatedBy = userId
        };

        await _movementRepository.AddAsync(movement, cancellationToken);
        await _stockRepository.SaveChangesAsync(cancellationToken);
        await _movementRepository.SaveChangesAsync(cancellationToken);

        var createdMovement = await _movementRepository.GetByIdAsync(movement.Id, organizationId, cancellationToken);
        return _mapper.Map<StockMovementResponseDto>(createdMovement!);
    }

    public async Task<StockMovementResponseDto> RecordStockOutAsync(Guid organizationId, StockOperationRequestDto dto, string userId, CancellationToken cancellationToken = default)
    {
        if (dto.Quantity <= 0) throw new ArgumentException("Quantity for Stock Out must be greater than zero.");

        var stock = await _stockRepository.GetByProductAndWarehouseAsync(dto.ProductId, dto.WarehouseId, organizationId, cancellationToken);
        if (stock == null || stock.QuantityAvailable < dto.Quantity)
        {
            throw new InvalidOperationException($"Insufficient available stock. Available: {stock?.QuantityAvailable ?? 0}, Requested: {dto.Quantity}");
        }

        stock.QuantityOnHand -= dto.Quantity;
        _stockRepository.Update(stock);

        var movement = new StockMovement
        {
            OrganizationId = organizationId,
            ProductId = dto.ProductId,
            SourceWarehouseId = dto.WarehouseId,
            MovementType = MovementType.StockOut,
            Quantity = dto.Quantity,
            ReferenceType = dto.ReferenceType ?? "ManualStockOut",
            ReferenceId = dto.ReferenceId,
            UnitCost = dto.UnitCost,
            BatchNumber = dto.BatchNumber,
            ExpiryDate = dto.ExpiryDate,
            Reason = dto.Reason,
            MovementDate = DateTime.UtcNow,
            CreatedBy = userId
        };

        await _movementRepository.AddAsync(movement, cancellationToken);
        await _stockRepository.SaveChangesAsync(cancellationToken);
        await _movementRepository.SaveChangesAsync(cancellationToken);

        var createdMovement = await _movementRepository.GetByIdAsync(movement.Id, organizationId, cancellationToken);
        return _mapper.Map<StockMovementResponseDto>(createdMovement!);
    }

    public async Task<StockMovementResponseDto> TransferStockAsync(Guid organizationId, StockTransferRequestDto dto, string userId, CancellationToken cancellationToken = default)
    {
        if (dto.Quantity <= 0) throw new ArgumentException("Transfer quantity must be greater than zero.");
        if (dto.SourceWarehouseId == dto.DestinationWarehouseId) throw new ArgumentException("Source and Destination warehouses cannot be identical.");

        var sourceStock = await _stockRepository.GetByProductAndWarehouseAsync(dto.ProductId, dto.SourceWarehouseId, organizationId, cancellationToken);
        if (sourceStock == null || sourceStock.QuantityAvailable < dto.Quantity)
        {
            throw new InvalidOperationException($"Insufficient available stock at source warehouse. Available: {sourceStock?.QuantityAvailable ?? 0}, Transfer: {dto.Quantity}");
        }

        var destStock = await _stockRepository.GetByProductAndWarehouseAsync(dto.ProductId, dto.DestinationWarehouseId, organizationId, cancellationToken);
        if (destStock == null)
        {
            destStock = new InventoryStock
            {
                OrganizationId = organizationId,
                ProductId = dto.ProductId,
                WarehouseId = dto.DestinationWarehouseId,
                QuantityOnHand = 0,
                QuantityReserved = 0
            };
            await _stockRepository.AddAsync(destStock, cancellationToken);
        }

        // Deduct from source
        sourceStock.QuantityOnHand -= dto.Quantity;
        _stockRepository.Update(sourceStock);

        // Add to destination
        destStock.QuantityOnHand += dto.Quantity;
        _stockRepository.Update(destStock);

        var product = await _productRepository.GetByIdAsync(dto.ProductId, organizationId, cancellationToken);

        var movement = new StockMovement
        {
            OrganizationId = organizationId,
            ProductId = dto.ProductId,
            SourceWarehouseId = dto.SourceWarehouseId,
            DestinationWarehouseId = dto.DestinationWarehouseId,
            MovementType = MovementType.Transfer,
            Quantity = dto.Quantity,
            ReferenceType = "StockTransfer",
            UnitCost = product?.CostPrice ?? 0,
            BatchNumber = dto.BatchNumber,
            Reason = dto.Reason ?? "Inter-warehouse transfer",
            MovementDate = DateTime.UtcNow,
            CreatedBy = userId
        };

        await _movementRepository.AddAsync(movement, cancellationToken);
        await _stockRepository.SaveChangesAsync(cancellationToken);
        await _movementRepository.SaveChangesAsync(cancellationToken);

        var createdMovement = await _movementRepository.GetByIdAsync(movement.Id, organizationId, cancellationToken);
        return _mapper.Map<StockMovementResponseDto>(createdMovement!);
    }

    public async Task<StockAdjustmentResponseDto> CreateAdjustmentAsync(Guid organizationId, CreateStockAdjustmentDto dto, string userId, CancellationToken cancellationToken = default)
    {
        var adjustmentNumber = $"ADJ-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

        var adjustment = new StockAdjustment
        {
            OrganizationId = organizationId,
            AdjustmentNumber = adjustmentNumber,
            WarehouseId = dto.WarehouseId,
            AdjustmentType = dto.AdjustmentType,
            Status = AdjustmentStatus.Draft,
            Reason = dto.Reason,
            AdjustedBy = userId,
            Items = dto.Items.Select(item => new StockAdjustmentItem
            {
                OrganizationId = organizationId,
                ProductId = item.ProductId,
                SystemQuantity = item.SystemQuantity,
                ActualQuantity = item.ActualQuantity,
                UnitCost = item.UnitCost,
                Reason = item.Reason
            }).ToList()
        };

        await _adjustmentRepository.AddAsync(adjustment, cancellationToken);
        await _adjustmentRepository.SaveChangesAsync(cancellationToken);

        var created = await _adjustmentRepository.GetByIdAsync(adjustment.Id, organizationId, cancellationToken);
        return _mapper.Map<StockAdjustmentResponseDto>(created!);
    }

    public async Task<StockAdjustmentResponseDto> ApproveAdjustmentAsync(Guid id, Guid organizationId, string userId, CancellationToken cancellationToken = default)
    {
        var adjustment = await _adjustmentRepository.GetByIdAsync(id, organizationId, cancellationToken);
        if (adjustment == null) throw new KeyNotFoundException($"Stock Adjustment {id} not found.");
        if (adjustment.Status != AdjustmentStatus.Draft) throw new InvalidOperationException("Only draft stock adjustments can be approved.");

        adjustment.Status = AdjustmentStatus.Approved;
        adjustment.ApprovedBy = userId;
        adjustment.ApprovedAt = DateTime.UtcNow;

        foreach (var item in adjustment.Items)
        {
            var stock = await _stockRepository.GetByProductAndWarehouseAsync(item.ProductId, adjustment.WarehouseId, organizationId, cancellationToken);
            if (stock == null)
            {
                stock = new InventoryStock
                {
                    OrganizationId = organizationId,
                    ProductId = item.ProductId,
                    WarehouseId = adjustment.WarehouseId,
                    QuantityOnHand = 0,
                    QuantityReserved = 0
                };
                await _stockRepository.AddAsync(stock, cancellationToken);
            }

            var variance = item.VarianceQuantity;
            stock.QuantityOnHand += variance;
            _stockRepository.Update(stock);

            // Record stock movement for variance
            var movement = new StockMovement
            {
                OrganizationId = organizationId,
                ProductId = item.ProductId,
                SourceWarehouseId = variance < 0 ? adjustment.WarehouseId : null,
                DestinationWarehouseId = variance > 0 ? adjustment.WarehouseId : null,
                MovementType = adjustment.AdjustmentType switch
                {
                    AdjustmentType.Damage => MovementType.Damage,
                    AdjustmentType.Loss => MovementType.Loss,
                    AdjustmentType.Spoilage => MovementType.Spoilage,
                    _ => MovementType.Adjustment
                },
                Quantity = Math.Abs(variance),
                ReferenceType = "StockAdjustment",
                ReferenceId = adjustment.AdjustmentNumber,
                UnitCost = item.UnitCost,
                Reason = item.Reason ?? adjustment.Reason,
                MovementDate = DateTime.UtcNow,
                CreatedBy = userId
            };
            await _movementRepository.AddAsync(movement, cancellationToken);
        }

        _adjustmentRepository.Update(adjustment);
        await _adjustmentRepository.SaveChangesAsync(cancellationToken);
        await _stockRepository.SaveChangesAsync(cancellationToken);
        await _movementRepository.SaveChangesAsync(cancellationToken);

        var updated = await _adjustmentRepository.GetByIdAsync(id, organizationId, cancellationToken);
        return _mapper.Map<StockAdjustmentResponseDto>(updated!);
    }

    public async Task<bool> ReconcileStockAsync(Guid organizationId, StockReconciliationRequestDto dto, string userId, CancellationToken cancellationToken = default)
    {
        var adjustmentDto = new CreateStockAdjustmentDto(
            dto.WarehouseId,
            AdjustmentType.PhysicalCount,
            dto.Reason,
            new List<CreateStockAdjustmentItemDto>()
        );

        foreach (var item in dto.Items)
        {
            var stock = await _stockRepository.GetByProductAndWarehouseAsync(item.ProductId, dto.WarehouseId, organizationId, cancellationToken);
            var product = await _productRepository.GetByIdAsync(item.ProductId, organizationId, cancellationToken);
            
            var sysQty = stock?.QuantityOnHand ?? 0;
            var unitCost = product?.CostPrice ?? 0;

            adjustmentDto.Items.Add(new CreateStockAdjustmentItemDto(
                item.ProductId,
                sysQty,
                item.ActualQuantity,
                unitCost,
                "Stock Reconciliation Count"
            ));
        }

        var createdAdj = await CreateAdjustmentAsync(organizationId, adjustmentDto, userId, cancellationToken);
        await ApproveAdjustmentAsync(createdAdj.Id, organizationId, userId, cancellationToken);
        return true;
    }

    public async Task<IEnumerable<InventoryStockResponseDto>> GetProductStockAsync(Guid productId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var stocks = await _stockRepository.GetStockByProductAsync(productId, organizationId, cancellationToken);
        return _mapper.Map<IEnumerable<InventoryStockResponseDto>>(stocks);
    }

    public async Task<PagedResult<StockMovementResponseDto>> GetStockMovementsAsync(Guid organizationId, StockMovementFilterDto filter, CancellationToken cancellationToken = default)
    {
        var pagedMovements = await _movementRepository.GetPagedAsync(organizationId, filter, cancellationToken);
        return new PagedResult<StockMovementResponseDto>(
            _mapper.Map<List<StockMovementResponseDto>>(pagedMovements.Items),
            pagedMovements.TotalCount,
            pagedMovements.PageNumber,
            pagedMovements.PageSize
        );
    }
}
