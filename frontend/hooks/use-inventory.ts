import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { inventoryService } from "@/lib/inventory-service";
import {
  ProductSearchFilter,
  CreateProductDto,
  UpdateProductDto,
  CreateCategoryDto,
  UpdateCategoryDto,
  CreateWarehouseDto,
  UpdateWarehouseDto,
  StockMovementFilter,
  StockOperationRequest,
  StockTransferRequest,
  CreateSupplierDto,
  UpdateSupplierDto,
  PurchaseOrderFilter,
  CreatePurchaseOrderDto,
  UpdatePurchaseOrderDto,
  CreateGoodsReceiptDto,
  CreateStockAdjustmentDto,
} from "@/types/inventory";

// ── Product Hooks ─────────────────────────────────────────────────────────────
export function useProducts(filter: ProductSearchFilter = {}) {
  return useQuery({
    queryKey: ["products", filter],
    queryFn: () => inventoryService.searchProducts(filter),
  });
}

export function useProduct(id: string) {
  return useQuery({
    queryKey: ["product", id],
    queryFn: () => inventoryService.getProductById(id),
    enabled: !!id,
  });
}

export function useProductBySku(sku: string) {
  return useQuery({
    queryKey: ["product-sku", sku],
    queryFn: () => inventoryService.getProductBySKU(sku),
    enabled: !!sku,
  });
}

export function useCreateProduct() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateProductDto) => inventoryService.createProduct(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
      queryClient.invalidateQueries({ queryKey: ["inventory-dashboard-summary"] });
    },
  });
}

export function useUpdateProduct() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: UpdateProductDto }) =>
      inventoryService.updateProduct(id, dto),
    onSuccess: (_, { id }) => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
      queryClient.invalidateQueries({ queryKey: ["product", id] });
    },
  });
}

export function useArchiveProduct() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => inventoryService.archiveProduct(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
    },
  });
}

// ── Category Hooks ────────────────────────────────────────────────────────────
export function useCategories() {
  return useQuery({
    queryKey: ["categories"],
    queryFn: () => inventoryService.getAllCategories(),
  });
}

export function useCreateCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateCategoryDto) => inventoryService.createCategory(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["categories"] });
    },
  });
}

export function useUpdateCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: UpdateCategoryDto }) =>
      inventoryService.updateCategory(id, dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["categories"] });
    },
  });
}

// ── Warehouse Hooks ───────────────────────────────────────────────────────────
export function useWarehouses() {
  return useQuery({
    queryKey: ["warehouses"],
    queryFn: () => inventoryService.getAllWarehouses(),
  });
}

export function useCreateWarehouse() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateWarehouseDto) => inventoryService.createWarehouse(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["warehouses"] });
    },
  });
}

export function useUpdateWarehouse() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: UpdateWarehouseDto }) =>
      inventoryService.updateWarehouse(id, dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["warehouses"] });
    },
  });
}

// ── Stock Movement & Operation Hooks ──────────────────────────────────────────
export function useProductStock(productId: string) {
  return useQuery({
    queryKey: ["product-stock", productId],
    queryFn: () => inventoryService.getProductStock(productId),
    enabled: !!productId,
  });
}

export function useStockMovements(filter: StockMovementFilter = {}) {
  return useQuery({
    queryKey: ["stock-movements", filter],
    queryFn: () => inventoryService.getStockMovements(filter),
  });
}

export function useRecordStockIn() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: StockOperationRequest) => inventoryService.recordStockIn(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
      queryClient.invalidateQueries({ queryKey: ["product-stock"] });
      queryClient.invalidateQueries({ queryKey: ["stock-movements"] });
      queryClient.invalidateQueries({ queryKey: ["inventory-dashboard-summary"] });
    },
  });
}

export function useRecordStockOut() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: StockOperationRequest) => inventoryService.recordStockOut(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
      queryClient.invalidateQueries({ queryKey: ["product-stock"] });
      queryClient.invalidateQueries({ queryKey: ["stock-movements"] });
      queryClient.invalidateQueries({ queryKey: ["inventory-dashboard-summary"] });
    },
  });
}

export function useTransferStock() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: StockTransferRequest) => inventoryService.transferStock(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
      queryClient.invalidateQueries({ queryKey: ["product-stock"] });
      queryClient.invalidateQueries({ queryKey: ["stock-movements"] });
    },
  });
}

// ── Supplier Hooks ────────────────────────────────────────────────────────────
export function useSuppliers(query?: string, pageNumber = 1, pageSize = 20) {
  return useQuery({
    queryKey: ["suppliers", query, pageNumber, pageSize],
    queryFn: () => inventoryService.getSuppliers(query, pageNumber, pageSize),
  });
}

export function useAllSuppliers() {
  return useQuery({
    queryKey: ["all-suppliers"],
    queryFn: () => inventoryService.getAllSuppliers(),
  });
}

export function useSupplierPerformance(id: string) {
  return useQuery({
    queryKey: ["supplier-performance", id],
    queryFn: () => inventoryService.getSupplierPerformance(id),
    enabled: !!id,
  });
}

export function useCreateSupplier() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateSupplierDto) => inventoryService.createSupplier(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["suppliers"] });
      queryClient.invalidateQueries({ queryKey: ["all-suppliers"] });
    },
  });
}

export function useUpdateSupplier() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: UpdateSupplierDto }) =>
      inventoryService.updateSupplier(id, dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["suppliers"] });
      queryClient.invalidateQueries({ queryKey: ["all-suppliers"] });
    },
  });
}

// ── Purchase Order Hooks ──────────────────────────────────────────────────────
export function usePurchaseOrders(filter: PurchaseOrderFilter = {}) {
  return useQuery({
    queryKey: ["purchase-orders", filter],
    queryFn: () => inventoryService.getPurchaseOrders(filter),
  });
}

export function usePurchaseOrder(id: string) {
  return useQuery({
    queryKey: ["purchase-order", id],
    queryFn: () => inventoryService.getPurchaseOrderById(id),
    enabled: !!id,
  });
}

export function useCreatePurchaseOrder() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreatePurchaseOrderDto) => inventoryService.createPurchaseOrder(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["purchase-orders"] });
      queryClient.invalidateQueries({ queryKey: ["inventory-dashboard-summary"] });
    },
  });
}

export function useUpdatePurchaseOrder() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: UpdatePurchaseOrderDto }) =>
      inventoryService.updatePurchaseOrder(id, dto),
    onSuccess: (_, { id }) => {
      queryClient.invalidateQueries({ queryKey: ["purchase-orders"] });
      queryClient.invalidateQueries({ queryKey: ["purchase-order", id] });
    },
  });
}

export function useApprovePurchaseOrder() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => inventoryService.approvePurchaseOrder(id),
    onSuccess: (_, id) => {
      queryClient.invalidateQueries({ queryKey: ["purchase-orders"] });
      queryClient.invalidateQueries({ queryKey: ["purchase-order", id] });
    },
  });
}

// ── Goods Receipt Hooks ───────────────────────────────────────────────────────
export function useGoodsReceipts(purchaseOrderId?: string, pageNumber = 1, pageSize = 20) {
  return useQuery({
    queryKey: ["goods-receipts", purchaseOrderId, pageNumber, pageSize],
    queryFn: () => inventoryService.getGoodsReceipts(purchaseOrderId, pageNumber, pageSize),
  });
}

export function useReceiveGoods() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateGoodsReceiptDto) => inventoryService.receiveGoods(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["purchase-orders"] });
      queryClient.invalidateQueries({ queryKey: ["goods-receipts"] });
      queryClient.invalidateQueries({ queryKey: ["products"] });
      queryClient.invalidateQueries({ queryKey: ["stock-movements"] });
      queryClient.invalidateQueries({ queryKey: ["inventory-dashboard-summary"] });
    },
  });
}

// ── Stock Adjustments Hooks ───────────────────────────────────────────────────
export function useStockAdjustments(warehouseId?: string, pageNumber = 1, pageSize = 20) {
  return useQuery({
    queryKey: ["stock-adjustments", warehouseId, pageNumber, pageSize],
    queryFn: () => inventoryService.getStockAdjustments(warehouseId, pageNumber, pageSize),
  });
}

export function useCreateAdjustment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateStockAdjustmentDto) => inventoryService.createAdjustment(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["stock-adjustments"] });
    },
  });
}

export function useApproveAdjustment() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => inventoryService.approveAdjustment(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["stock-adjustments"] });
      queryClient.invalidateQueries({ queryKey: ["products"] });
      queryClient.invalidateQueries({ queryKey: ["stock-movements"] });
    },
  });
}

// ── Smart Analytics & Dashboard Summary Hooks ─────────────────────────────────
export function useStockAlerts() {
  return useQuery({
    queryKey: ["stock-alerts"],
    queryFn: () => inventoryService.getStockAlerts(),
  });
}

export function useReorderSuggestions() {
  return useQuery({
    queryKey: ["reorder-suggestions"],
    queryFn: () => inventoryService.getReorderSuggestions(),
  });
}

export function useProductVelocityReport(days = 30) {
  return useQuery({
    queryKey: ["velocity-report", days],
    queryFn: () => inventoryService.getVelocityReport(days),
  });
}

export function useDashboardSummary() {
  return useQuery({
    queryKey: ["inventory-dashboard-summary"],
    queryFn: () => inventoryService.getDashboardSummary(),
  });
}
