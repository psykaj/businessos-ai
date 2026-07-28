# Day 18 Implementation Plan: Inventory Management, Purchasing & Supplier Platform Backend

Build a complete, enterprise-grade Inventory & Purchasing Backend for BusinessOS AI adhering to Clean Architecture, multi-tenant isolation, CQRS/Repository pattern, Redis caching integration, and real-time audit capabilities.

## User Review Required

> [!IMPORTANT]
> - All inventory stock operations (Stock In, Stock Out, Stock Transfer, Goods Receipt, Inventory Adjustment) dynamically recalculate inventory stock balances (`QuantityOnHand`, `QuantityReserved`, `QuantityAvailable`) with atomic database transactions and audit logging.
> - Multi-tenant isolation is strictly enforced via `OrganizationId` on all entities, repositories, and controllers.
> - Barcode and QR code lookup foundations are integrated into product models and search APIs.

## Proposed Changes

### Module: `backend/Modules/Inventory`

#### [NEW] Entities (`backend/Modules/Inventory/Entities/`)
- `Product.cs`: Core product catalog with SKU, Barcode, QR Code, Unit of Measure, Pricing, Safety Stock, Reorder Point, Max/Min stock levels.
- `ProductCategory.cs`: Hierarchical product categories.
- `Warehouse.cs`: Multi-warehouse support with location metadata, code, primary status.
- `InventoryStock.cs`: Stock levels per product per warehouse (`QuantityOnHand`, `QuantityReserved`, `QuantityAvailable`, LocationBin).
- `StockMovement.cs`: Immutable audit trail for all stock transitions (In, Out, Transfer, Receipt, Adjustment, Loss).
- `Supplier.cs`: Supplier directory with contact details, payment terms, tax ID, and performance ratings.
- `PurchaseOrder.cs`: PO document with status workflow (`Draft`, `Submitted`, `Approved`, `PartiallyReceived`, `FullyReceived`, `Cancelled`), financial summary, line items.
- `PurchaseOrderItem.cs`: Line items for POs with quantity ordered, received, unit price.
- `GoodsReceipt.cs`: Receipt record documenting physical delivery against POs.
- `GoodsReceiptItem.cs`: Received line items with accepted/rejected breakdown and rejection reasons.
- `StockAdjustment.cs`: Count/damage/spoilage reconciliation header.
- `StockAdjustmentItem.cs`: Stock variance per product (System vs Actual quantity).

#### [NEW] Enums (`backend/Modules/Inventory/Enums/`)
- `MovementType.cs`: `StockIn`, `StockOut`, `Transfer`, `Adjustment`, `Damage`, `PurchaseReceipt`, `SalesReturn`.
- `PurchaseOrderStatus.cs`: `Draft`, `Submitted`, `Approved`, `PartiallyReceived`, `FullyReceived`, `Cancelled`.
- `AdjustmentType.cs`: `PhysicalCount`, `Damage`, `Loss`, `Spoilage`, `Correction`.
- `AdjustmentStatus.cs`: `Draft`, `Approved`, `Rejected`.

#### [NEW] DTOs (`backend/Modules/Inventory/DTOs/`)
- `ProductDtos.cs`: Create/Update/Response DTOs, ProductSearchFilter, BarcodeLookupDto.
- `CategoryDtos.cs`: Category Create/Update/Response DTOs.
- `WarehouseDtos.cs`: Warehouse Create/Update/Response DTOs.
- `InventoryStockDtos.cs`: StockLevelDto, StockTransferDto, StockAdjustmentRequestDto.
- `StockMovementDtos.cs`: StockMovementResponseDto, StockMovementFilterDto.
- `SupplierDtos.cs`: Supplier Create/Update/Response DTOs, PerformanceScoreDto.
- `PurchaseOrderDtos.cs`: CreatePurchaseOrderDto, UpdatePurchaseOrderDto, PurchaseOrderResponseDto, ApprovePurchaseOrderDto.
- `GoodsReceiptDtos.cs`: CreateGoodsReceiptDto, GoodsReceiptResponseDto.
- `StockAdjustmentDtos.cs`: CreateStockAdjustmentDto, StockAdjustmentResponseDto.
- `SmartInventoryDtos.cs`: ReorderSuggestionDto, StockAlertDto, ProductVelocityReportDto, DashboardInventorySummaryDto.

#### [NEW] Repositories (`backend/Modules/Inventory/Repositories/`)
- Interfaces in `backend/Modules/Inventory/Interfaces/`: `IProductRepository`, `ICategoryRepository`, `IWarehouseRepository`, `IStockRepository`, `IStockMovementRepository`, `ISupplierRepository`, `IPurchaseOrderRepository`, `IGoodsReceiptRepository`, `IStockAdjustmentRepository`.
- Implementations: Concrete repositories handling async EF Core queries, tenant filtering, inclusion of child entities, and pagination.

#### [NEW] Services (`backend/Modules/Inventory/Services/`)
- `ProductService.cs`: Manages CRUD, archiving, SKU/barcode lookup, and stock totals.
- `CategoryService.cs`: Category hierarchy management.
- `WarehouseService.cs`: Warehouse management.
- `StockMovementService.cs`: Stock movements, stock transfers, stock-in/out logic.
- `PurchasingService.cs`: PO creation, approval workflow, goods receipt processing, stock ledger updating.
- `SupplierService.cs`: Supplier directory & automated performance rating calculations.
- `SmartInventoryService.cs`: Reorder point algorithms, stock alerts (Low stock, Out of stock, Overstock), fast/slow moving reports, executive dashboard metrics.
- `InventoryCacheService.cs`: High-performance Redis caching helper for frequent product/stock queries.

#### [NEW] Helpers & Validators (`backend/Modules/Inventory/Helpers/`, `backend/Modules/Inventory/Validators/`)
- `BarcodeQrHelper.cs`: Helper for barcode/QR code payload generation & validation.
- `InventoryValidators.cs`: FluentValidation rules for Products, Warehouses, Suppliers, POs, Goods Receipts, and Stock Adjustments.

#### [NEW] Controllers (`backend/Modules/Inventory/Controllers/`)
- `BaseInventoryController.cs`: Base controller deriving from `ControllerBase` with tenant context helpers.
- `ProductsController.cs`
- `CategoriesController.cs`
- `WarehousesController.cs`
- `StockMovementsController.cs`
- `PurchaseOrdersController.cs`
- `SuppliersController.cs`
- `GoodsReceiptsController.cs`
- `StockAdjustmentsController.cs`
- `SmartInventoryController.cs`

#### [NEW] Extension (`backend/Modules/Inventory/Extensions/`)
- `InventoryServiceCollectionExtensions.cs`: Dependency Injection registration for all repositories, services, and validators.

---

### Integration & Persistence

#### [MODIFY] [ApplicationDbContext.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Persistence/ApplicationDbContext.cs)
- Register DbSets: `Products`, `ProductCategories`, `Warehouses`, `InventoryStocks`, `StockMovements`, `Suppliers`, `PurchaseOrders`, `PurchaseOrderItems`, `GoodsReceipts`, `GoodsReceiptItems`, `StockAdjustments`, `StockAdjustmentItems`.
- Configure EF Core Fluent API indexes (`OrganizationId`, `SKU`, `Barcode`, `WarehouseId`, `SupplierId`, `PONumber`, `ReceiptNumber`), relationships, and soft-delete query filters.

#### [MODIFY] [Program.cs](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/backend/Program.cs)
- Register `builder.Services.AddInventoryModule();`.

---

### Documentation & Verification

#### [NEW] Documentation Files (`docs/`)
- `docs/inventory.md`: Complete overview of Product catalog, Warehouse management, Stock calculation engine, and Barcode/QR workflows.
- `docs/purchasing.md`: Purchase Order lifecycles, approval workflow, goods receipt, partial fulfillment, and stock integration.
- `docs/suppliers.md`: Supplier management, evaluation metrics, performance scoring, and supplier portal architectural hooks.
- `docs/stock-management.md`: Stock In, Stock Out, Stock Transfer, Inventory Adjustments, Damage reporting, and audit history.

#### [MODIFY] [roadmap.md](file:///Users/pankajanilyadav/Documents/BusinessOS-AI/roadmap.md)
- Update inventory, purchasing, supplier, stock management, and smart inventory alerts checklist items to completed (✅).

## Verification Plan

### Automated Tests & Build Verification
1. Run `dotnet build` from `/Users/pankajanilyadav/Documents/BusinessOS-AI/backend` to verify 0 compile errors.
2. Run `dotnet ef migrations add AddInventoryPurchasingModule` to create and validate EF Core database migrations.
3. Validate DB model configuration against DbContext.

### Manual Verification
1. Test Stock balance calculations: `QuantityOnHand`, `QuantityReserved`, `QuantityAvailable`.
2. Test PO flow: Create PO -> Approve PO -> Goods Receipt (partial & full) -> Verify Stock Movements & InventoryStock balance update.
3. Test Smart Alerts: Trigger reorder suggestions & low stock alerts based on reorder points.
