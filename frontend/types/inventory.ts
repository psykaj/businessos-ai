export interface ProductDto {
  id: string;
  organizationId: string;
  sku: string;
  name: string;
  description?: string;
  barcode?: string;
  qrCode?: string;
  categoryId: string;
  categoryName: string;
  unitOfMeasure: string;
  costPrice: number;
  sellingPrice: number;
  reorderPoint: number;
  safetyStock: number;
  reorderQuantity: number;
  minimumStockLevel: number;
  maxStockLevel: number;
  totalQuantityOnHand: number;
  totalQuantityReserved: number;
  totalQuantityAvailable: number;
  isArchived: boolean;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateProductDto {
  sku: string;
  name: string;
  description?: string;
  barcode?: string;
  qrCode?: string;
  categoryId: string;
  unitOfMeasure: string;
  costPrice: number;
  sellingPrice: number;
  reorderPoint: number;
  safetyStock: number;
  reorderQuantity: number;
  minimumStockLevel: number;
  maxStockLevel: number;
}

export interface UpdateProductDto {
  name: string;
  description?: string;
  barcode?: string;
  qrCode?: string;
  categoryId: string;
  unitOfMeasure: string;
  costPrice: number;
  sellingPrice: number;
  reorderPoint: number;
  safetyStock: number;
  reorderQuantity: number;
  minimumStockLevel: number;
  maxStockLevel: number;
  isActive: boolean;
}

export interface ProductSearchFilter {
  query?: string;
  categoryId?: string;
  warehouseId?: string;
  isArchived?: boolean;
  lowStockOnly?: boolean;
  pageNumber?: number;
  pageSize?: number;
  sortBy?: string;
  sortDescending?: boolean;
}

export interface BarcodeLookupDto {
  code: string;
  product?: ProductDto | null;
}

export interface CategoryDto {
  id: string;
  organizationId: string;
  name: string;
  code: string;
  description?: string;
  parentCategoryId?: string;
  parentCategoryName?: string;
  productsCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateCategoryDto {
  name: string;
  code: string;
  description?: string;
  parentCategoryId?: string;
}

export interface UpdateCategoryDto {
  name: string;
  code: string;
  description?: string;
  parentCategoryId?: string;
}

export interface WarehouseDto {
  id: string;
  organizationId: string;
  name: string;
  code: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  managerName?: string;
  managerPhone?: string;
  isActive: boolean;
  isPrimary: boolean;
  totalItemsCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateWarehouseDto {
  name: string;
  code: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  managerName?: string;
  managerPhone?: string;
  isPrimary?: boolean;
}

export interface UpdateWarehouseDto {
  name: string;
  code: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  managerName?: string;
  managerPhone?: string;
  isActive: boolean;
  isPrimary: boolean;
}

export interface InventoryStockDto {
  id: string;
  organizationId: string;
  productId: string;
  productName: string;
  productSKU: string;
  warehouseId: string;
  warehouseName: string;
  quantityOnHand: number;
  quantityReserved: number;
  quantityAvailable: number;
  locationBin?: string;
  reorderLevel: number;
  updatedAt: string;
}

export type MovementType =
  | "StockIn"
  | "StockOut"
  | "Transfer"
  | "Adjustment"
  | "Damage"
  | "PurchaseReceipt"
  | "SalesReturn"
  | "Loss"
  | "Spoilage";

export interface StockMovementDto {
  id: string;
  organizationId: string;
  productId: string;
  productName: string;
  productSKU: string;
  sourceWarehouseId?: string;
  sourceWarehouseName?: string;
  destinationWarehouseId?: string;
  destinationWarehouseName?: string;
  movementType: MovementType;
  quantity: number;
  referenceType?: string;
  referenceId?: string;
  unitCost: number;
  batchNumber?: string;
  expiryDate?: string;
  reason?: string;
  movementDate: string;
}

export interface StockMovementFilter {
  productId?: string;
  warehouseId?: string;
  movementType?: MovementType;
  startDate?: string;
  endDate?: string;
  pageNumber?: number;
  pageSize?: number;
}

export interface StockOperationRequest {
  productId: string;
  warehouseId: string;
  quantity: number;
  reason: string;
  referenceType?: string;
  referenceId?: string;
  unitCost: number;
  batchNumber?: string;
  expiryDate?: string;
}

export interface StockTransferRequest {
  productId: string;
  sourceWarehouseId: string;
  destinationWarehouseId: string;
  quantity: number;
  reason?: string;
  batchNumber?: string;
}

export interface SupplierDto {
  id: string;
  organizationId: string;
  name: string;
  code: string;
  contactPerson: string;
  email: string;
  phone: string;
  address?: string;
  city?: string;
  country?: string;
  taxId?: string;
  paymentTerms: string;
  performanceScore: number;
  totalOrdersCount: number;
  onTimeDeliveriesCount: number;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateSupplierDto {
  name: string;
  code: string;
  contactPerson: string;
  email: string;
  phone: string;
  address?: string;
  city?: string;
  country?: string;
  taxId?: string;
  paymentTerms?: string;
}

export interface UpdateSupplierDto {
  name: string;
  code: string;
  contactPerson: string;
  email: string;
  phone: string;
  address?: string;
  city?: string;
  country?: string;
  taxId?: string;
  paymentTerms: string;
  isActive: boolean;
}

export interface SupplierPerformanceDto {
  supplierId: string;
  supplierName: string;
  performanceScore: number;
  totalOrdersCount: number;
  onTimeDeliveriesCount: number;
  onTimeDeliveryRate: number;
  fulfillmentRate: number;
}

export type PurchaseOrderStatus =
  | "Draft"
  | "Submitted"
  | "Approved"
  | "PartiallyReceived"
  | "FullyReceived"
  | "Cancelled";

export interface PurchaseOrderItemDto {
  id: string;
  productId: string;
  productName: string;
  productSKU: string;
  quantityOrdered: number;
  quantityReceived: number;
  unitPrice: number;
  totalPrice: number;
  notes?: string;
}

export interface PurchaseOrderDto {
  id: string;
  organizationId: string;
  poNumber: string;
  supplierId: string;
  supplierName: string;
  warehouseId: string;
  warehouseName: string;
  status: PurchaseOrderStatus;
  orderDate: string;
  expectedDeliveryDate: string;
  subTotal: number;
  taxAmount: number;
  shippingCost: number;
  totalAmount: number;
  notes?: string;
  termsAndConditions?: string;
  approvedBy?: string;
  approvedAt?: string;
  items: PurchaseOrderItemDto[];
  createdAt: string;
  updatedAt: string;
}

export interface CreatePurchaseOrderItemDto {
  productId: string;
  quantityOrdered: number;
  unitPrice: number;
  notes?: string;
}

export interface CreatePurchaseOrderDto {
  supplierId: string;
  warehouseId: string;
  expectedDeliveryDate: string;
  shippingCost: number;
  notes?: string;
  termsAndConditions?: string;
  items: CreatePurchaseOrderItemDto[];
}

export type UpdatePurchaseOrderDto = CreatePurchaseOrderDto;

export interface PurchaseOrderFilter {
  supplierId?: string;
  warehouseId?: string;
  status?: PurchaseOrderStatus;
  startDate?: string;
  endDate?: string;
  pageNumber?: number;
  pageSize?: number;
}

export interface GoodsReceiptItemDto {
  id: string;
  purchaseOrderItemId: string;
  productId: string;
  productName: string;
  productSKU: string;
  quantityReceived: number;
  quantityRejected: number;
  rejectionReason?: string;
  unitCost: number;
  batchNumber?: string;
  expiryDate?: string;
}

export interface GoodsReceiptDto {
  id: string;
  organizationId: string;
  receiptNumber: string;
  purchaseOrderId: string;
  poNumber: string;
  supplierId: string;
  supplierName: string;
  warehouseId: string;
  warehouseName: string;
  receiptDate: string;
  receivedBy: string;
  notes?: string;
  items: GoodsReceiptItemDto[];
  createdAt: string;
}

export interface CreateGoodsReceiptItemDto {
  purchaseOrderItemId: string;
  productId: string;
  quantityReceived: number;
  quantityRejected?: number;
  rejectionReason?: string;
  unitCost?: number;
  batchNumber?: string;
  expiryDate?: string;
}

export interface CreateGoodsReceiptDto {
  purchaseOrderId: string;
  notes?: string;
  items: CreateGoodsReceiptItemDto[];
}

export type AdjustmentType =
  | "PhysicalCount"
  | "Damage"
  | "Loss"
  | "Spoilage"
  | "Correction";

export type AdjustmentStatus = "Draft" | "Approved" | "Rejected";

export interface StockAdjustmentItemDto {
  id: string;
  productId: string;
  productName: string;
  productSKU: string;
  systemQuantity: number;
  actualQuantity: number;
  varianceQuantity: number;
  unitCost: number;
  reason?: string;
}

export interface StockAdjustmentDto {
  id: string;
  organizationId: string;
  adjustmentNumber: string;
  warehouseId: string;
  warehouseName: string;
  adjustmentType: AdjustmentType;
  status: AdjustmentStatus;
  reason: string;
  adjustedBy: string;
  approvedBy?: string;
  approvedAt?: string;
  items: StockAdjustmentItemDto[];
  createdAt: string;
}

export interface CreateStockAdjustmentItemDto {
  productId: string;
  systemQuantity: number;
  actualQuantity: number;
  unitCost: number;
  reason?: string;
}

export interface CreateStockAdjustmentDto {
  warehouseId: string;
  adjustmentType: AdjustmentType;
  reason: string;
  items: CreateStockAdjustmentItemDto[];
}

export interface StockAlertDto {
  productId: string;
  productName: string;
  sku: string;
  alertType: "LowStock" | "OutOfStock" | "Overstock";
  currentStock: number;
  threshold: number;
  recommendation: string;
  detectedAt: string;
}

export interface ReorderSuggestionDto {
  productId: string;
  productName: string;
  sku: string;
  currentAvailableStock: number;
  reorderPoint: number;
  safetyStock: number;
  suggestedReorderQuantity: number;
  estimatedCost: number;
  preferredSupplierId: string;
  preferredSupplierName: string;
}

export interface ProductVelocityReportDto {
  productId: string;
  productName: string;
  sku: string;
  totalMovementQuantity: number;
  totalValue: number;
  classification: "FastMoving" | "ModerateMoving" | "SlowMoving";
  daysWithoutMovement: number;
}

export interface DashboardInventorySummaryDto {
  totalProductsCount: number;
  lowStockCount: number;
  outOfStockCount: number;
  overstockCount: number;
  pendingPurchaseOrdersCount: number;
  totalInventoryValue: number;
  recentAlerts: StockAlertDto[];
  topReorderSuggestions: ReorderSuggestionDto[];
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}
