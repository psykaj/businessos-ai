using backend.Modules.Inventory.DTOs;
using backend.Modules.Inventory.Entities;
using backend.Modules.Inventory.Enums;
using backend.Modules.Inventory.Interfaces;

namespace backend.Modules.Inventory.Services;

public class SmartInventoryService : ISmartInventoryService
{
    private readonly IProductRepository _productRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IPurchaseOrderRepository _poRepository;
    private readonly IStockMovementRepository _movementRepository;

    public SmartInventoryService(
        IProductRepository productRepository,
        ISupplierRepository supplierRepository,
        IPurchaseOrderRepository poRepository,
        IStockMovementRepository movementRepository)
    {
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
        _poRepository = poRepository;
        _movementRepository = movementRepository;
    }

    public async Task<IEnumerable<StockAlertDto>> GetStockAlertsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var alerts = new List<StockAlertDto>();
        var now = DateTime.UtcNow;

        var lowStockProds = await _productRepository.GetLowStockProductsAsync(organizationId, cancellationToken);
        foreach (var p in lowStockProds)
        {
            var avail = p.StockLevels.Sum(s => s.QuantityOnHand - s.QuantityReserved);
            if (avail == 0)
            {
                alerts.Add(new StockAlertDto(
                    p.Id, p.Name, p.SKU, "OutOfStock", avail, 0,
                    $"Product is completely out of stock. Immediate reorder recommended (Reorder Quantity: {p.ReorderQuantity}).",
                    now
                ));
            }
            else
            {
                alerts.Add(new StockAlertDto(
                    p.Id, p.Name, p.SKU, "LowStock", avail, p.ReorderPoint,
                    $"Stock ({avail}) is below reorder threshold ({p.ReorderPoint}). Suggested reorder qty: {p.ReorderQuantity}.",
                    now
                ));
            }
        }

        var overstockProds = await _productRepository.GetOverstockProductsAsync(organizationId, cancellationToken);
        foreach (var p in overstockProds)
        {
            var totalStock = p.StockLevels.Sum(s => s.QuantityOnHand);
            alerts.Add(new StockAlertDto(
                p.Id, p.Name, p.SKU, "Overstock", totalStock, p.MaxStockLevel,
                $"Current stock ({totalStock}) exceeds maximum stock level ({p.MaxStockLevel}). Pause purchasing to avoid holding costs.",
                now
            ));
        }

        return alerts;
    }

    public async Task<IEnumerable<ReorderSuggestionDto>> GetReorderSuggestionsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var suggestions = new List<ReorderSuggestionDto>();
        var lowStockProds = await _productRepository.GetLowStockProductsAsync(organizationId, cancellationToken);
        var suppliers = (await _supplierRepository.GetAllAsync(organizationId, cancellationToken)).ToList();
        var defaultSupplier = suppliers.FirstOrDefault();

        foreach (var p in lowStockProds)
        {
            var avail = p.StockLevels.Sum(s => s.QuantityOnHand - s.QuantityReserved);
            var targetReorderQty = Math.Max(p.ReorderQuantity, p.MaxStockLevel - avail);
            var estimatedCost = targetReorderQty * p.CostPrice;

            suggestions.Add(new ReorderSuggestionDto(
                p.Id,
                p.Name,
                p.SKU,
                avail,
                p.ReorderPoint,
                p.SafetyStock,
                targetReorderQty,
                estimatedCost,
                defaultSupplier?.Id ?? Guid.Empty,
                defaultSupplier?.Name ?? "Default Vendor"
            ));
        }

        return suggestions;
    }

    public async Task<IEnumerable<ProductVelocityReportDto>> GetProductVelocityReportAsync(Guid organizationId, int days = 30, CancellationToken cancellationToken = default)
    {
        var filter = new StockMovementFilterDto(
            null, null, MovementType.StockOut,
            DateTime.UtcNow.AddDays(-days), DateTime.UtcNow,
            1, 1000
        );

        var movementsResult = await _movementRepository.GetPagedAsync(organizationId, filter, cancellationToken);
        var grouped = movementsResult.Items.GroupBy(m => m.ProductId);

        var report = new List<ProductVelocityReportDto>();

        foreach (var group in grouped)
        {
            var productId = group.Key;
            var product = await _productRepository.GetByIdAsync(productId, organizationId, cancellationToken);
            if (product == null) continue;

            var totalQty = group.Sum(m => m.Quantity);
            var totalValue = totalQty * product.CostPrice;
            var lastMovementDate = group.Max(m => m.MovementDate);
            var daysWithoutMovement = (int)(DateTime.UtcNow - lastMovementDate).TotalDays;

            string classification = totalQty switch
            {
                >= 100 => "FastMoving",
                >= 20 => "ModerateMoving",
                _ => "SlowMoving"
            };

            report.Add(new ProductVelocityReportDto(
                product.Id,
                product.Name,
                product.SKU,
                totalQty,
                totalValue,
                classification,
                daysWithoutMovement
            ));
        }

        return report.OrderByDescending(r => r.TotalMovementQuantity);
    }

    public async Task<DashboardInventorySummaryDto> GetDashboardSummaryAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var allProducts = await _productRepository.GetPagedAsync(organizationId, new ProductSearchFilterDto(null, null, null, false, false, 1, 10000), cancellationToken);
        var lowStock = await _productRepository.GetLowStockProductsAsync(organizationId, cancellationToken);
        var outOfStock = await _productRepository.GetOutOfStockProductsAsync(organizationId, cancellationToken);
        var overstock = await _productRepository.GetOverstockProductsAsync(organizationId, cancellationToken);

        var pendingPOs = await _poRepository.GetCountByStatusAsync(organizationId, PurchaseOrderStatus.Approved, cancellationToken);

        decimal totalValue = allProducts.Items.Sum(p => p.StockLevels.Sum(s => s.QuantityOnHand) * p.CostPrice);

        var alerts = (await GetStockAlertsAsync(organizationId, cancellationToken)).Take(10).ToList();
        var reorderSuggestions = (await GetReorderSuggestionsAsync(organizationId, cancellationToken)).Take(10).ToList();

        return new DashboardInventorySummaryDto(
            allProducts.TotalCount,
            lowStock.Count(),
            outOfStock.Count(),
            overstock.Count(),
            pendingPOs,
            totalValue,
            alerts,
            reorderSuggestions
        );
    }
}
