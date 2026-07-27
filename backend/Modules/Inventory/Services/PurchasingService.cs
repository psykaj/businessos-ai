using AutoMapper;
using backend.Common;
using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Enums;
using backend.Modules.Inventory.Interfaces;

namespace backend.Modules.Inventory.Services;

public class PurchasingService : IPurchasingService
{
    private readonly IPurchaseOrderRepository _poRepository;
    private readonly IGoodsReceiptRepository _grRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IStockMovementRepository _movementRepository;
    private readonly IMapper _mapper;

    public PurchasingService(
        IPurchaseOrderRepository poRepository,
        IGoodsReceiptRepository grRepository,
        ISupplierRepository supplierRepository,
        IProductRepository productRepository,
        IStockRepository stockRepository,
        IStockMovementRepository movementRepository,
        IMapper mapper)
    {
        _poRepository = poRepository;
        _grRepository = grRepository;
        _supplierRepository = supplierRepository;
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _movementRepository = movementRepository;
        _mapper = mapper;
    }

    public async Task<PurchaseOrderResponseDto> CreatePurchaseOrderAsync(Guid organizationId, CreatePurchaseOrderDto dto, string userId, CancellationToken cancellationToken = default)
    {
        var poNumber = $"PO-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

        decimal subTotal = 0;
        var items = new List<PurchaseOrderItem>();

        foreach (var itemDto in dto.Items)
        {
            var itemTotal = itemDto.QuantityOrdered * itemDto.UnitPrice;
            subTotal += itemTotal;

            items.Add(new PurchaseOrderItem
            {
                OrganizationId = organizationId,
                ProductId = itemDto.ProductId,
                QuantityOrdered = itemDto.QuantityOrdered,
                QuantityReceived = 0,
                UnitPrice = itemDto.UnitPrice,
                TotalPrice = itemTotal,
                Notes = itemDto.Notes
            });
        }

        var taxAmount = subTotal * 0.18m; // Default 18% tax assumption (can be overridden/configured)
        var totalAmount = subTotal + taxAmount + dto.ShippingCost;

        var po = new PurchaseOrder
        {
            OrganizationId = organizationId,
            PONumber = poNumber,
            SupplierId = dto.SupplierId,
            WarehouseId = dto.WarehouseId,
            Status = PurchaseOrderStatus.Draft,
            OrderDate = DateTime.UtcNow,
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
            SubTotal = subTotal,
            TaxAmount = taxAmount,
            ShippingCost = dto.ShippingCost,
            TotalAmount = totalAmount,
            Notes = dto.Notes,
            TermsAndConditions = dto.TermsAndConditions,
            CreatedBy = userId,
            Items = items
        };

        await _poRepository.AddAsync(po, cancellationToken);
        await _poRepository.SaveChangesAsync(cancellationToken);

        var created = await _poRepository.GetByIdAsync(po.Id, organizationId, cancellationToken);
        return _mapper.Map<PurchaseOrderResponseDto>(created!);
    }

    public async Task<PurchaseOrderResponseDto> UpdatePurchaseOrderAsync(Guid id, Guid organizationId, UpdatePurchaseOrderDto dto, CancellationToken cancellationToken = default)
    {
        var po = await _poRepository.GetByIdAsync(id, organizationId, cancellationToken);
        if (po == null) throw new KeyNotFoundException($"Purchase Order {id} not found.");
        if (po.Status != PurchaseOrderStatus.Draft) throw new InvalidOperationException("Only draft purchase orders can be updated.");

        po.SupplierId = dto.SupplierId;
        po.WarehouseId = dto.WarehouseId;
        po.ExpectedDeliveryDate = dto.ExpectedDeliveryDate;
        po.ShippingCost = dto.ShippingCost;
        po.Notes = dto.Notes;
        po.TermsAndConditions = dto.TermsAndConditions;

        // Replace items
        po.Items.Clear();

        decimal subTotal = 0;
        foreach (var itemDto in dto.Items)
        {
            var itemTotal = itemDto.QuantityOrdered * itemDto.UnitPrice;
            subTotal += itemTotal;

            po.Items.Add(new PurchaseOrderItem
            {
                OrganizationId = organizationId,
                PurchaseOrderId = po.Id,
                ProductId = itemDto.ProductId,
                QuantityOrdered = itemDto.QuantityOrdered,
                QuantityReceived = 0,
                UnitPrice = itemDto.UnitPrice,
                TotalPrice = itemTotal,
                Notes = itemDto.Notes
            });
        }

        po.SubTotal = subTotal;
        po.TaxAmount = subTotal * 0.18m;
        po.TotalAmount = subTotal + po.TaxAmount + dto.ShippingCost;

        _poRepository.Update(po);
        await _poRepository.SaveChangesAsync(cancellationToken);

        var updated = await _poRepository.GetByIdAsync(id, organizationId, cancellationToken);
        return _mapper.Map<PurchaseOrderResponseDto>(updated!);
    }

    public async Task<PurchaseOrderResponseDto> ApprovePurchaseOrderAsync(Guid id, Guid organizationId, string userId, CancellationToken cancellationToken = default)
    {
        var po = await _poRepository.GetByIdAsync(id, organizationId, cancellationToken);
        if (po == null) throw new KeyNotFoundException($"Purchase Order {id} not found.");
        if (po.Status != PurchaseOrderStatus.Draft && po.Status != PurchaseOrderStatus.Submitted)
        {
            throw new InvalidOperationException($"Purchase order cannot be approved in state '{po.Status}'.");
        }

        po.Status = PurchaseOrderStatus.Approved;
        po.ApprovedBy = userId;
        po.ApprovedAt = DateTime.UtcNow;

        _poRepository.Update(po);
        await _poRepository.SaveChangesAsync(cancellationToken);

        var updated = await _poRepository.GetByIdAsync(id, organizationId, cancellationToken);
        return _mapper.Map<PurchaseOrderResponseDto>(updated!);
    }

    public async Task<PurchaseOrderResponseDto?> GetPurchaseOrderByIdAsync(Guid id, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var po = await _poRepository.GetByIdAsync(id, organizationId, cancellationToken);
        return po != null ? _mapper.Map<PurchaseOrderResponseDto>(po) : null;
    }

    public async Task<PagedResult<PurchaseOrderResponseDto>> GetPurchaseOrdersAsync(Guid organizationId, PurchaseOrderFilterDto filter, CancellationToken cancellationToken = default)
    {
        var pagedPOs = await _poRepository.GetPagedAsync(organizationId, filter, cancellationToken);
        return new PagedResult<PurchaseOrderResponseDto>(
            _mapper.Map<List<PurchaseOrderResponseDto>>(pagedPOs.Items),
            pagedPOs.TotalCount,
            pagedPOs.PageNumber,
            pagedPOs.PageSize
        );
    }

    public async Task<GoodsReceiptResponseDto> ReceiveGoodsAsync(Guid organizationId, CreateGoodsReceiptDto dto, string userId, CancellationToken cancellationToken = default)
    {
        var po = await _poRepository.GetByIdAsync(dto.PurchaseOrderId, organizationId, cancellationToken);
        if (po == null) throw new KeyNotFoundException($"Purchase Order {dto.PurchaseOrderId} not found.");
        if (po.Status != PurchaseOrderStatus.Approved && po.Status != PurchaseOrderStatus.PartiallyReceived)
        {
            throw new InvalidOperationException($"Cannot receive goods for Purchase Order in status '{po.Status}'. Order must be Approved or PartiallyReceived.");
        }

        var grNumber = $"GRN-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

        var goodsReceipt = new GoodsReceipt
        {
            OrganizationId = organizationId,
            ReceiptNumber = grNumber,
            PurchaseOrderId = po.Id,
            SupplierId = po.SupplierId,
            WarehouseId = po.WarehouseId,
            ReceiptDate = DateTime.UtcNow,
            ReceivedBy = userId,
            Notes = dto.Notes,
            Items = new List<GoodsReceiptItem>()
        };

        foreach (var itemDto in dto.Items)
        {
            var poItem = po.Items.FirstOrDefault(i => i.Id == itemDto.PurchaseOrderItemId);
            if (poItem == null) throw new InvalidOperationException($"Purchase Order Item {itemDto.PurchaseOrderItemId} not found on PO.");

            var acceptedQty = itemDto.QuantityReceived - itemDto.QuantityRejected;
            if (acceptedQty < 0) throw new ArgumentException("Quantity rejected cannot exceed total quantity received.");

            poItem.QuantityReceived += itemDto.QuantityReceived;

            goodsReceipt.Items.Add(new GoodsReceiptItem
            {
                OrganizationId = organizationId,
                PurchaseOrderItemId = poItem.Id,
                ProductId = itemDto.ProductId,
                QuantityReceived = itemDto.QuantityReceived,
                QuantityRejected = itemDto.QuantityRejected,
                RejectionReason = itemDto.RejectionReason,
                UnitCost = itemDto.UnitCost > 0 ? itemDto.UnitCost : poItem.UnitPrice,
                BatchNumber = itemDto.BatchNumber,
                ExpiryDate = itemDto.ExpiryDate
            });

            // Increase physical inventory stock for accepted quantity
            if (acceptedQty > 0)
            {
                var stock = await _stockRepository.GetByProductAndWarehouseAsync(itemDto.ProductId, po.WarehouseId, organizationId, cancellationToken);
                if (stock == null)
                {
                    stock = new InventoryStock
                    {
                        OrganizationId = organizationId,
                        ProductId = itemDto.ProductId,
                        WarehouseId = po.WarehouseId,
                        QuantityOnHand = 0,
                        QuantityReserved = 0
                    };
                    await _stockRepository.AddAsync(stock, cancellationToken);
                }

                stock.QuantityOnHand += acceptedQty;
                _stockRepository.Update(stock);

                // Record stock movement
                var movement = new StockMovement
                {
                    OrganizationId = organizationId,
                    ProductId = itemDto.ProductId,
                    DestinationWarehouseId = po.WarehouseId,
                    MovementType = MovementType.PurchaseReceipt,
                    Quantity = acceptedQty,
                    ReferenceType = "GoodsReceipt",
                    ReferenceId = grNumber,
                    UnitCost = itemDto.UnitCost > 0 ? itemDto.UnitCost : poItem.UnitPrice,
                    BatchNumber = itemDto.BatchNumber,
                    ExpiryDate = itemDto.ExpiryDate,
                    Reason = $"Received via PO #{po.PONumber}",
                    MovementDate = DateTime.UtcNow,
                    CreatedBy = userId
                };
                await _movementRepository.AddAsync(movement, cancellationToken);
            }
        }

        // Update PO Status (FullyReceived or PartiallyReceived)
        bool allFullyReceived = po.Items.All(i => i.QuantityReceived >= i.QuantityOrdered);
        po.Status = allFullyReceived ? PurchaseOrderStatus.FullyReceived : PurchaseOrderStatus.PartiallyReceived;

        await _grRepository.AddAsync(goodsReceipt, cancellationToken);
        _poRepository.Update(po);
        await _grRepository.SaveChangesAsync(cancellationToken);
        await _poRepository.SaveChangesAsync(cancellationToken);
        await _stockRepository.SaveChangesAsync(cancellationToken);
        await _movementRepository.SaveChangesAsync(cancellationToken);

        // Update supplier performance tracking
        var supplier = await _supplierRepository.GetByIdAsync(po.SupplierId, organizationId, cancellationToken);
        if (supplier != null)
        {
            supplier.TotalOrdersCount += 1;
            if (DateTime.UtcNow <= po.ExpectedDeliveryDate.AddDays(1))
            {
                supplier.OnTimeDeliveriesCount += 1;
            }
            supplier.PerformanceScore = (double)supplier.OnTimeDeliveriesCount / supplier.TotalOrdersCount * 100.0;
            _supplierRepository.Update(supplier);
            await _supplierRepository.SaveChangesAsync(cancellationToken);
        }

        var created = await _grRepository.GetByIdAsync(goodsReceipt.Id, organizationId, cancellationToken);
        return _mapper.Map<GoodsReceiptResponseDto>(created!);
    }

    public async Task<PagedResult<GoodsReceiptResponseDto>> GetGoodsReceiptsAsync(Guid organizationId, Guid? purchaseOrderId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var pagedGRs = await _grRepository.GetPagedAsync(organizationId, purchaseOrderId, pageNumber, pageSize, cancellationToken);
        return new PagedResult<GoodsReceiptResponseDto>(
            _mapper.Map<List<GoodsReceiptResponseDto>>(pagedGRs.Items),
            pagedGRs.TotalCount,
            pagedGRs.PageNumber,
            pagedGRs.PageSize
        );
    }
}
