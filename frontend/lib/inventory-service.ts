import apiClient from "./api-client";
import {
  ProductDto,
  CreateProductDto,
  UpdateProductDto,
  ProductSearchFilter,
  BarcodeLookupDto,
  CategoryDto,
  CreateCategoryDto,
  UpdateCategoryDto,
  WarehouseDto,
  CreateWarehouseDto,
  UpdateWarehouseDto,
  InventoryStockDto,
  StockMovementDto,
  StockMovementFilter,
  StockOperationRequest,
  StockTransferRequest,
  SupplierDto,
  CreateSupplierDto,
  UpdateSupplierDto,
  SupplierPerformanceDto,
  PurchaseOrderDto,
  CreatePurchaseOrderDto,
  UpdatePurchaseOrderDto,
  PurchaseOrderFilter,
  GoodsReceiptDto,
  CreateGoodsReceiptDto,
  StockAdjustmentDto,
  CreateStockAdjustmentDto,
  StockAlertDto,
  ReorderSuggestionDto,
  ProductVelocityReportDto,
  DashboardInventorySummaryDto,
  PagedResult,
} from "@/types/inventory";

export const inventoryService = {
  // ── Products ─────────────────────────────────────────────────────────────
  async searchProducts(filter: ProductSearchFilter = {}): Promise<PagedResult<ProductDto>> {
    const res = await apiClient.get<PagedResult<ProductDto>>("/api/inventory/products", {
      params: filter,
    });
    return res.data;
  },

  async getProductById(id: string): Promise<ProductDto> {
    const res = await apiClient.get<ProductDto>(`/api/inventory/products/${id}`);
    return res.data;
  },

  async getProductBySKU(sku: string): Promise<ProductDto> {
    const res = await apiClient.get<ProductDto>(`/api/inventory/products/sku/${sku}`);
    return res.data;
  },

  async lookupBarcode(code: string): Promise<BarcodeLookupDto> {
    const res = await apiClient.get<BarcodeLookupDto>(`/api/inventory/products/barcode/${code}`);
    return res.data;
  },

  async createProduct(dto: CreateProductDto): Promise<ProductDto> {
    const res = await apiClient.post<ProductDto>("/api/inventory/products", dto);
    return res.data;
  },

  async updateProduct(id: string, dto: UpdateProductDto): Promise<ProductDto> {
    const res = await apiClient.put<ProductDto>(`/api/inventory/products/${id}`, dto);
    return res.data;
  },

  async archiveProduct(id: string): Promise<void> {
    await apiClient.post(`/api/inventory/products/${id}/archive`);
  },

  // ── Categories ───────────────────────────────────────────────────────────
  async getAllCategories(): Promise<CategoryDto[]> {
    const res = await apiClient.get<CategoryDto[]>("/api/inventory/categories");
    return res.data;
  },

  async getCategoryById(id: string): Promise<CategoryDto> {
    const res = await apiClient.get<CategoryDto>(`/api/inventory/categories/${id}`);
    return res.data;
  },

  async createCategory(dto: CreateCategoryDto): Promise<CategoryDto> {
    const res = await apiClient.post<CategoryDto>("/api/inventory/categories", dto);
    return res.data;
  },

  async updateCategory(id: string, dto: UpdateCategoryDto): Promise<CategoryDto> {
    const res = await apiClient.put<CategoryDto>(`/api/inventory/categories/${id}`, dto);
    return res.data;
  },

  // ── Warehouses ───────────────────────────────────────────────────────────
  async getAllWarehouses(): Promise<WarehouseDto[]> {
    const res = await apiClient.get<WarehouseDto[]>("/api/inventory/warehouses");
    return res.data;
  },

  async getWarehouseById(id: string): Promise<WarehouseDto> {
    const res = await apiClient.get<WarehouseDto>(`/api/inventory/warehouses/${id}`);
    return res.data;
  },

  async createWarehouse(dto: CreateWarehouseDto): Promise<WarehouseDto> {
    const res = await apiClient.post<WarehouseDto>("/api/inventory/warehouses", dto);
    return res.data;
  },

  async updateWarehouse(id: string, dto: UpdateWarehouseDto): Promise<WarehouseDto> {
    const res = await apiClient.put<WarehouseDto>(`/api/inventory/warehouses/${id}`, dto);
    return res.data;
  },

  // ── Stock Operations & Audit Trail ──────────────────────────────────────
  async getProductStock(productId: string): Promise<InventoryStockDto[]> {
    const res = await apiClient.get<InventoryStockDto[]>(`/api/inventory/stock/product/${productId}`);
    return res.data;
  },

  async getStockMovements(filter: StockMovementFilter = {}): Promise<PagedResult<StockMovementDto>> {
    const res = await apiClient.get<PagedResult<StockMovementDto>>("/api/inventory/stock/movements", {
      params: filter,
    });
    return res.data;
  },

  async recordStockIn(dto: StockOperationRequest): Promise<StockMovementDto> {
    const res = await apiClient.post<StockMovementDto>("/api/inventory/stock/in", dto);
    return res.data;
  },

  async recordStockOut(dto: StockOperationRequest): Promise<StockMovementDto> {
    const res = await apiClient.post<StockMovementDto>("/api/inventory/stock/out", dto);
    return res.data;
  },

  async transferStock(dto: StockTransferRequest): Promise<StockMovementDto> {
    const res = await apiClient.post<StockMovementDto>("/api/inventory/stock/transfer", dto);
    return res.data;
  },

  // ── Suppliers ────────────────────────────────────────────────────────────
  async getSuppliers(query?: string, pageNumber = 1, pageSize = 20): Promise<PagedResult<SupplierDto>> {
    const res = await apiClient.get<PagedResult<SupplierDto>>("/api/suppliers", {
      params: { query, pageNumber, pageSize },
    });
    return res.data;
  },

  async getAllSuppliers(): Promise<SupplierDto[]> {
    const res = await apiClient.get<SupplierDto[]>("/api/suppliers/all");
    return res.data;
  },

  async getSupplierById(id: string): Promise<SupplierDto> {
    const res = await apiClient.get<SupplierDto>(`/api/suppliers/${id}`);
    return res.data;
  },

  async getSupplierPerformance(id: string): Promise<SupplierPerformanceDto> {
    const res = await apiClient.get<SupplierPerformanceDto>(`/api/suppliers/${id}/performance`);
    return res.data;
  },

  async createSupplier(dto: CreateSupplierDto): Promise<SupplierDto> {
    const res = await apiClient.post<SupplierDto>("/api/suppliers", dto);
    return res.data;
  },

  async updateSupplier(id: string, dto: UpdateSupplierDto): Promise<SupplierDto> {
    const res = await apiClient.put<SupplierDto>(`/api/suppliers/${id}`, dto);
    return res.data;
  },

  // ── Purchase Orders ──────────────────────────────────────────────────────
  async getPurchaseOrders(filter: PurchaseOrderFilter = {}): Promise<PagedResult<PurchaseOrderDto>> {
    const res = await apiClient.get<PagedResult<PurchaseOrderDto>>("/api/purchasing/orders", {
      params: filter,
    });
    return res.data;
  },

  async getPurchaseOrderById(id: string): Promise<PurchaseOrderDto> {
    const res = await apiClient.get<PurchaseOrderDto>(`/api/purchasing/orders/${id}`);
    return res.data;
  },

  async createPurchaseOrder(dto: CreatePurchaseOrderDto): Promise<PurchaseOrderDto> {
    const res = await apiClient.post<PurchaseOrderDto>("/api/purchasing/orders", dto);
    return res.data;
  },

  async updatePurchaseOrder(id: string, dto: UpdatePurchaseOrderDto): Promise<PurchaseOrderDto> {
    const res = await apiClient.put<PurchaseOrderDto>(`/api/purchasing/orders/${id}`, dto);
    return res.data;
  },

  async approvePurchaseOrder(id: string): Promise<PurchaseOrderDto> {
    const res = await apiClient.post<PurchaseOrderDto>(`/api/purchasing/orders/${id}/approve`);
    return res.data;
  },

  // ── Goods Receipts ───────────────────────────────────────────────────────
  async getGoodsReceipts(purchaseOrderId?: string, pageNumber = 1, pageSize = 20): Promise<PagedResult<GoodsReceiptDto>> {
    const res = await apiClient.get<PagedResult<GoodsReceiptDto>>("/api/purchasing/receipts", {
      params: { purchaseOrderId, pageNumber, pageSize },
    });
    return res.data;
  },

  async receiveGoods(dto: CreateGoodsReceiptDto): Promise<GoodsReceiptDto> {
    const res = await apiClient.post<GoodsReceiptDto>("/api/purchasing/receipts", dto);
    return res.data;
  },

  // ── Stock Adjustments ───────────────────────────────────────────────────
  async getStockAdjustments(warehouseId?: string, pageNumber = 1, pageSize = 20): Promise<PagedResult<StockAdjustmentDto>> {
    const res = await apiClient.get<PagedResult<StockAdjustmentDto>>("/api/inventory/adjustments", {
      params: { warehouseId, pageNumber, pageSize },
    });
    return res.data;
  },

  async createAdjustment(dto: CreateStockAdjustmentDto): Promise<StockAdjustmentDto> {
    const res = await apiClient.post<StockAdjustmentDto>("/api/inventory/adjustments", dto);
    return res.data;
  },

  async approveAdjustment(id: string): Promise<StockAdjustmentDto> {
    const res = await apiClient.post<StockAdjustmentDto>(`/api/inventory/adjustments/${id}/approve`);
    return res.data;
  },

  // ── Smart Inventory & BI Dashboard ──────────────────────────────────────
  async getStockAlerts(): Promise<StockAlertDto[]> {
    const res = await apiClient.get<StockAlertDto[]>("/api/inventory/smart/alerts");
    return res.data;
  },

  async getReorderSuggestions(): Promise<ReorderSuggestionDto[]> {
    const res = await apiClient.get<ReorderSuggestionDto[]>("/api/inventory/smart/reorder-suggestions");
    return res.data;
  },

  async getVelocityReport(days = 30): Promise<ProductVelocityReportDto[]> {
    const res = await apiClient.get<ProductVelocityReportDto[]>("/api/inventory/smart/velocity-report", {
      params: { days },
    });
    return res.data;
  },

  async getDashboardSummary(): Promise<DashboardInventorySummaryDto> {
    const res = await apiClient.get<DashboardInventorySummaryDto>("/api/inventory/smart/dashboard-summary");
    return res.data;
  },
};
