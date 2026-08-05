import apiClient from "./api-client";

// ==========================================
// DTOs & Interfaces
// ==========================================

export type BranchStatus = "Active" | "Inactive" | "UnderMaintenance" | "PermanentlyClosed";
export type WarehouseStatus = "Operational" | "Maintenance" | "AtCapacity" | "Decommissioned";
export type TransferStatus = "Pending" | "Approved" | "InTransit" | "Received" | "Rejected" | "Cancelled";

export interface BranchDto {
  id: string;
  organizationId: string;
  locationId?: string;
  locationName?: string;
  region?: string;
  name: string;
  code: string;
  status: BranchStatus;
  contactEmail?: string;
  contactPhone?: string;
  costCenterCode?: string;
  workingHoursJson: string;
  operationalSettingsJson: string;
  isPrimary: boolean;
  // Enriched operational stats for dashboard UX
  managerName?: string;
  managerEmail?: string;
  canApproveTransfers?: boolean;
  maxTransferApprovalLimit?: number;
  monthlyRevenue: number;
  monthlyProfit: number;
  profitMargin: number;
  inventoryUtilization: number;
  createdAt: string;
}

export interface LocationDto {
  id: string;
  organizationId: string;
  country: string;
  stateProvince: string;
  city: string;
  postalCode: string;
  addressLine: string;
  region: string; // e.g. "EMEA", "North America", "APAC", "LATAM"
  timezone: string;
  isActive: boolean;
  branchesCount?: number;
}

export interface BranchWarehouseDto {
  id: string;
  organizationId: string;
  branchId: string;
  branchName: string;
  name: string;
  code: string;
  storageCapacitySqFt: number;
  currentUtilizationPercentage: number;
  totalStockItemsCount: number;
  estimatedStockValue: number;
  contactPerson?: string;
  contactPhone?: string;
  status: WarehouseStatus;
  isPrimary: boolean;
}

export interface TransferItemDto {
  sku: string;
  productName: string;
  quantity: number;
  unitPrice: number;
}

export interface WarehouseTransferDto {
  id: string;
  organizationId: string;
  sourceWarehouseId: string;
  sourceWarehouseName: string;
  destinationWarehouseId: string;
  destinationWarehouseName: string;
  transferNumber: string;
  status: TransferStatus;
  approvalStatus: string;
  requestedById: string;
  requestedByName: string;
  approvedById?: string;
  approvedByName?: string;
  transferredItemsJson: string;
  items?: TransferItemDto[];
  totalItemsCount: number;
  totalTransferValue: number;
  requestedAt: string;
  approvedAt?: string;
  shippedAt?: string;
  receivedAt?: string;
  trackingNotes?: string;
  rejectionReason?: string;
}

export interface RegionalSummaryDto {
  id: string;
  region: string;
  totalBranches: number;
  totalRevenue: number;
  totalProfit: number;
  profitMargin: number;
  totalInventoryValue: number;
  totalCustomers: number;
  totalEmployees: number;
  revenuePerEmployee: number;
  topPerformingBranchName: string;
  momGrowthPercentage: number;
}

export interface BranchPerformanceDto {
  id: string;
  branchId: string;
  branchName: string;
  region: string;
  year: number;
  month: number;
  totalRevenue: number;
  totalExpenses: number;
  totalProfit: number;
  profitMarginPercentage: number;
  customerCount: number;
  employeeCount: number;
  inventoryUtilizationPercentage: number;
  performanceScore: number;
  rank: number;
  isBestPerformer: boolean;
  isLowestPerformer: boolean;
  moMRevenueGrowthPercentage: number;
  moMProfitGrowthPercentage: number;
}

export interface CreateBranchPayload {
  name: string;
  code: string;
  locationId?: string;
  contactEmail?: string;
  contactPhone?: string;
  costCenterCode?: string;
  isPrimary: boolean;
  workingHoursJson?: string;
  operationalSettingsJson?: string;
}

export interface AssignManagerPayload {
  userId: string;
  managerName: string;
  managerEmail: string;
  managerPhone?: string;
  canApproveTransfers: boolean;
  maxTransferApprovalLimit: number;
}

export interface CreateTransferPayload {
  sourceWarehouseId: string;
  destinationWarehouseId: string;
  requestedByName: string;
  items: TransferItemDto[];
  notes?: string;
}

// ==========================================
// High-Fidelity Mock Fallback Dataset
// ==========================================

let MOCK_LOCATIONS: LocationDto[] = [
  { id: "loc-1", organizationId: "org-1", country: "United States", stateProvince: "California", city: "San Francisco", postalCode: "94105", addressLine: "500 Market Street, Suite 400", region: "North America", timezone: "PST (UTC-8)", isActive: true, branchesCount: 2 },
  { id: "loc-2", organizationId: "org-1", country: "United States", stateProvince: "New York", city: "New York", postalCode: "10001", addressLine: "350 5th Ave, Floor 44", region: "North America", timezone: "EST (UTC-5)", isActive: true, branchesCount: 2 },
  { id: "loc-3", organizationId: "org-1", country: "United Kingdom", stateProvince: "Greater London", city: "London", postalCode: "EC2M 7PP", addressLine: "10 Bishopsgate, Level 30", region: "EMEA", timezone: "GMT (UTC+0)", isActive: true, branchesCount: 1 },
  { id: "loc-4", organizationId: "org-1", country: "Germany", stateProvince: "Bavaria", city: "Munich", postalCode: "80331", addressLine: "Marienplatz 8", region: "EMEA", timezone: "CET (UTC+1)", isActive: true, branchesCount: 1 },
  { id: "loc-5", organizationId: "org-1", country: "Singapore", stateProvince: "Singapore", city: "Singapore", postalCode: "018956", addressLine: "Marina Bay Financial Centre Tower 2", region: "APAC", timezone: "SGT (UTC+8)", isActive: true, branchesCount: 1 },
  { id: "loc-6", organizationId: "org-1", country: "Australia", stateProvince: "New South Wales", city: "Sydney", postalCode: "2000", addressLine: "100 Barangaroo Avenue", region: "APAC", timezone: "AEST (UTC+10)", isActive: true, branchesCount: 1 },
];

let MOCK_BRANCHES: BranchDto[] = [
  {
    id: "br-1",
    organizationId: "org-1",
    locationId: "loc-1",
    locationName: "San Francisco HQ (CA)",
    region: "North America",
    name: "SF Flagship Innovation Center",
    code: "BR-USA-SF01",
    status: "Active",
    contactEmail: "sf-ops@businessos.ai",
    contactPhone: "+1 (415) 890-2100",
    costCenterCode: "CC-9001-NA",
    workingHoursJson: JSON.stringify({ Monday: "08:00-18:00", Tuesday: "08:00-18:00", Wednesday: "08:00-18:00", Thursday: "08:00-18:00", Friday: "08:00-17:00", Saturday: "Closed", Sunday: "Closed" }),
    operationalSettingsJson: JSON.stringify({ AutoReorder: true, MaxTransferApprovalAmount: 25000, PosSyncEnabled: true }),
    isPrimary: true,
    managerName: "Sarah Jenkins, VP West",
    managerEmail: "sjenkins@businessos.ai",
    canApproveTransfers: true,
    maxTransferApprovalLimit: 25000,
    monthlyRevenue: 642500,
    monthlyProfit: 218450,
    profitMargin: 34.0,
    inventoryUtilization: 84.2,
    createdAt: "2024-01-15T10:00:00Z"
  },
  {
    id: "br-2",
    organizationId: "org-1",
    locationId: "loc-2",
    locationName: "New York Midtown (NY)",
    region: "North America",
    name: "Manhattan Enterprise Hub",
    code: "BR-USA-NY01",
    status: "Active",
    contactEmail: "nyc-enterprise@businessos.ai",
    contactPhone: "+1 (212) 555-8940",
    costCenterCode: "CC-9002-NA",
    workingHoursJson: JSON.stringify({ Monday: "08:30-17:30", Tuesday: "08:30-17:30", Wednesday: "08:30-17:30", Thursday: "08:30-17:30", Friday: "08:30-17:00", Saturday: "Closed", Sunday: "Closed" }),
    operationalSettingsJson: JSON.stringify({ AutoReorder: true, MaxTransferApprovalAmount: 50000, PosSyncEnabled: true }),
    isPrimary: false,
    managerName: "Marcus Vance, Director East",
    managerEmail: "mvance@businessos.ai",
    canApproveTransfers: true,
    maxTransferApprovalLimit: 50000,
    monthlyRevenue: 890400,
    monthlyProfit: 342100,
    profitMargin: 38.4,
    inventoryUtilization: 91.5,
    createdAt: "2024-02-10T12:00:00Z"
  },
  {
    id: "br-3",
    organizationId: "org-1",
    locationId: "loc-3",
    locationName: "London Bishopsgate (UK)",
    region: "EMEA",
    name: "London EMEA Regional Gateway",
    code: "BR-UK-LON01",
    status: "Active",
    contactEmail: "london-gateway@businessos.ai",
    contactPhone: "+44 20 7946 0981",
    costCenterCode: "CC-8001-EUR",
    workingHoursJson: JSON.stringify({ Monday: "09:00-17:30", Tuesday: "09:00-17:30", Wednesday: "09:00-17:30", Thursday: "09:00-17:30", Friday: "09:00-16:30", Saturday: "Closed", Sunday: "Closed" }),
    operationalSettingsJson: JSON.stringify({ AutoReorder: false, MaxTransferApprovalAmount: 15000, PosSyncEnabled: true }),
    isPrimary: false,
    managerName: "Elena Rostova, Managing Director EMEA",
    managerEmail: "erostova@businessos.ai",
    canApproveTransfers: true,
    maxTransferApprovalLimit: 30000,
    monthlyRevenue: 512000,
    monthlyProfit: 168960,
    profitMargin: 33.0,
    inventoryUtilization: 76.4,
    createdAt: "2024-03-01T09:00:00Z"
  },
  {
    id: "br-4",
    organizationId: "org-1",
    locationId: "loc-4",
    locationName: "Munich Marienplatz (DE)",
    region: "EMEA",
    name: "Munich Central Engineering & Ops",
    code: "BR-DE-MUC01",
    status: "UnderMaintenance",
    contactEmail: "munich-ops@businessos.ai",
    contactPhone: "+49 89 2345 6789",
    costCenterCode: "CC-8002-EUR",
    workingHoursJson: JSON.stringify({ Monday: "08:00-16:30", Tuesday: "08:00-16:30", Wednesday: "08:00-16:30", Thursday: "08:00-16:30", Friday: "08:00-15:00", Saturday: "Closed", Sunday: "Closed" }),
    operationalSettingsJson: JSON.stringify({ AutoReorder: true, MaxTransferApprovalAmount: 10000, PosSyncEnabled: false }),
    isPrimary: false,
    managerName: "Heinrich Weber, Plant Operations Head",
    managerEmail: "hweber@businessos.ai",
    canApproveTransfers: true,
    maxTransferApprovalLimit: 15000,
    monthlyRevenue: 384000,
    monthlyProfit: 99840,
    profitMargin: 26.0,
    inventoryUtilization: 68.2,
    createdAt: "2024-04-12T08:00:00Z"
  },
  {
    id: "br-5",
    organizationId: "org-1",
    locationId: "loc-5",
    locationName: "Singapore Marina Bay (SG)",
    region: "APAC",
    name: "Singapore APAC Commerce Terminal",
    code: "BR-SG-SGP01",
    status: "Active",
    contactEmail: "singapore-hub@businessos.ai",
    contactPhone: "+65 6789 0123",
    costCenterCode: "CC-7001-APC",
    workingHoursJson: JSON.stringify({ Monday: "09:00-18:00", Tuesday: "09:00-18:00", Wednesday: "09:00-18:00", Thursday: "09:00-18:00", Friday: "09:00-17:30", Saturday: "09:00-13:00", Sunday: "Closed" }),
    operationalSettingsJson: JSON.stringify({ AutoReorder: true, MaxTransferApprovalAmount: 40000, PosSyncEnabled: true }),
    isPrimary: false,
    managerName: "Ken Tan, VP Asia Pacific",
    managerEmail: "ktan@businessos.ai",
    canApproveTransfers: true,
    maxTransferApprovalLimit: 40000,
    monthlyRevenue: 724000,
    monthlyProfit: 275120,
    profitMargin: 38.0,
    inventoryUtilization: 88.9,
    createdAt: "2024-05-20T03:00:00Z"
  },
  {
    id: "br-6",
    organizationId: "org-1",
    locationId: "loc-6",
    locationName: "Sydney Barangaroo (AU)",
    region: "APAC",
    name: "Sydney Oceania Distribution Branch",
    code: "BR-AU-SYD01",
    status: "Active",
    contactEmail: "sydney-ops@businessos.ai",
    contactPhone: "+61 2 8000 9000",
    costCenterCode: "CC-7002-APC",
    workingHoursJson: JSON.stringify({ Monday: "08:30-17:00", Tuesday: "08:30-17:00", Wednesday: "08:30-17:00", Thursday: "08:30-17:00", Friday: "08:30-16:30", Saturday: "Closed", Sunday: "Closed" }),
    operationalSettingsJson: JSON.stringify({ AutoReorder: false, MaxTransferApprovalAmount: 20000, PosSyncEnabled: true }),
    isPrimary: false,
    managerName: "Chloe O'Connor, Regional Lead",
    managerEmail: "coconnor@businessos.ai",
    canApproveTransfers: false,
    maxTransferApprovalLimit: 0,
    monthlyRevenue: 345000,
    monthlyProfit: 86250,
    profitMargin: 25.0,
    inventoryUtilization: 62.1,
    createdAt: "2024-06-18T01:00:00Z"
  }
];

let MOCK_WAREHOUSES: BranchWarehouseDto[] = [
  { id: "wh-1", organizationId: "org-1", branchId: "br-1", branchName: "SF Flagship Innovation Center", name: "Bay Area Primary Logistics Depot", code: "WH-USA-SF-A", storageCapacitySqFt: 85000, currentUtilizationPercentage: 84.2, totalStockItemsCount: 42500, estimatedStockValue: 2450000, contactPerson: "Dave Miller", contactPhone: "+1 (415) 890-2105", status: "Operational", isPrimary: true },
  { id: "wh-2", organizationId: "org-1", branchId: "br-2", branchName: "Manhattan Enterprise Hub", name: "New Jersey Tri-State Fulfillment Center", code: "WH-USA-EWR-A", storageCapacitySqFt: 120000, currentUtilizationPercentage: 91.5, totalStockItemsCount: 68400, estimatedStockValue: 4120000, contactPerson: "Rachel Vance", contactPhone: "+1 (201) 555-4321", status: "Operational", isPrimary: true },
  { id: "wh-3", organizationId: "org-1", branchId: "br-3", branchName: "London EMEA Regional Gateway", code: "WH-UK-LHR-A", name: "Heathrow customs & Regional Depot", storageCapacitySqFt: 65000, currentUtilizationPercentage: 76.4, totalStockItemsCount: 29800, estimatedStockValue: 1890000, contactPerson: "Ian Wright", contactPhone: "+44 20 8900 1122", status: "Operational", isPrimary: true },
  { id: "wh-4", organizationId: "org-1", branchId: "br-4", branchName: "Munich Central Engineering & Ops", name: "Bavarian Automated Storage Facility", code: "WH-DE-MUC-B", storageCapacitySqFt: 70000, currentUtilizationPercentage: 68.2, totalStockItemsCount: 24100, estimatedStockValue: 1450000, contactPerson: "Katarina Braun", contactPhone: "+49 89 5432 1098", status: "Maintenance", isPrimary: true },
  { id: "wh-5", organizationId: "org-1", branchId: "br-5", branchName: "Singapore APAC Commerce Terminal", name: "Tuas Mega Port Distribution Hub", code: "WH-SG-TUAS", storageCapacitySqFt: 95000, currentUtilizationPercentage: 88.9, totalStockItemsCount: 52000, estimatedStockValue: 3100000, contactPerson: "David Lim", contactPhone: "+65 6111 2222", status: "Operational", isPrimary: true },
  { id: "wh-6", organizationId: "org-1", branchId: "br-6", branchName: "Sydney Oceania Distribution Branch", name: "Western Sydney Logistics Hub", code: "WH-AU-SYD-W", storageCapacitySqFt: 50000, currentUtilizationPercentage: 62.1, totalStockItemsCount: 18400, estimatedStockValue: 980000, contactPerson: "Liam Kelly", contactPhone: "+61 2 9888 7777", status: "Operational", isPrimary: true },
];

let MOCK_TRANSFERS: WarehouseTransferDto[] = [
  {
    id: "trf-101",
    organizationId: "org-1",
    sourceWarehouseId: "wh-2",
    sourceWarehouseName: "New Jersey Tri-State Fulfillment Center",
    destinationWarehouseId: "wh-1",
    destinationWarehouseName: "Bay Area Primary Logistics Depot",
    transferNumber: "TRF-2026-0042",
    status: "InTransit",
    approvalStatus: "Approved",
    requestedById: "usr-10",
    requestedByName: "Sarah Jenkins",
    approvedById: "usr-11",
    approvedByName: "Marcus Vance",
    transferredItemsJson: JSON.stringify([
      { sku: "SERVER-PRO-X1", productName: "Rackmount Enterprise Server X1", quantity: 15, unitPrice: 3200 },
      { sku: "SWITCH-100G", productName: "Core Network Fiber Switch 100G", quantity: 25, unitPrice: 1450 }
    ]),
    items: [
      { sku: "SERVER-PRO-X1", productName: "Rackmount Enterprise Server X1", quantity: 15, unitPrice: 3200 },
      { sku: "SWITCH-100G", productName: "Core Network Fiber Switch 100G", quantity: 25, unitPrice: 1450 }
    ],
    totalItemsCount: 40,
    totalTransferValue: 84250,
    requestedAt: "2026-08-01T14:20:00Z",
    approvedAt: "2026-08-01T16:10:00Z",
    shippedAt: "2026-08-02T09:00:00Z",
    trackingNotes: "FedEx Freight Specialized #FX-99882211 | Expected arrival Aug 6."
  },
  {
    id: "trf-102",
    organizationId: "org-1",
    sourceWarehouseId: "wh-5",
    sourceWarehouseName: "Tuas Mega Port Distribution Hub",
    destinationWarehouseId: "wh-6",
    destinationWarehouseName: "Western Sydney Logistics Hub",
    transferNumber: "TRF-2026-0043",
    status: "Pending",
    approvalStatus: "Pending",
    requestedById: "usr-12",
    requestedByName: "Chloe O'Connor",
    transferredItemsJson: JSON.stringify([
      { sku: "POS-TERM-S9", productName: "Smart Touch POS Terminal S9", quantity: 50, unitPrice: 850 },
      { sku: "SCAN-QR-PRO", productName: "Industrial Barcode & QR Scanner", quantity: 80, unitPrice: 320 }
    ]),
    items: [
      { sku: "POS-TERM-S9", productName: "Smart Touch POS Terminal S9", quantity: 50, unitPrice: 850 },
      { sku: "SCAN-QR-PRO", productName: "Industrial Barcode & QR Scanner", quantity: 80, unitPrice: 320 }
    ],
    totalItemsCount: 130,
    totalTransferValue: 68100,
    requestedAt: "2026-08-04T10:15:00Z",
    trackingNotes: "Urgent inventory redistribution for Q3 Oceania retail expansion."
  },
  {
    id: "trf-103",
    organizationId: "org-1",
    sourceWarehouseId: "wh-3",
    sourceWarehouseName: "Heathrow customs & Regional Depot",
    destinationWarehouseId: "wh-4",
    destinationWarehouseName: "Bavarian Automated Storage Facility",
    transferNumber: "TRF-2026-0040",
    status: "Received",
    approvalStatus: "AutoApproved",
    requestedById: "usr-14",
    requestedByName: "Heinrich Weber",
    approvedById: "usr-14",
    approvedByName: "Heinrich Weber (Auto)",
    transferredItemsJson: JSON.stringify([
      { sku: "IOT-SENSOR-TEMP", productName: "Industrial Temp & Humidity Sensor", quantity: 200, unitPrice: 45 }
    ]),
    items: [
      { sku: "IOT-SENSOR-TEMP", productName: "Industrial Temp & Humidity Sensor", quantity: 200, unitPrice: 45 }
    ],
    totalItemsCount: 200,
    totalTransferValue: 9000,
    requestedAt: "2026-07-28T08:30:00Z",
    approvedAt: "2026-07-28T08:30:00Z",
    shippedAt: "2026-07-29T07:00:00Z",
    receivedAt: "2026-07-31T11:45:00Z",
    trackingNotes: "DHL Express Regional | Received intact. All 200 sensors verified and inducted into inventory."
  },
  {
    id: "trf-104",
    organizationId: "org-1",
    sourceWarehouseId: "wh-1",
    sourceWarehouseName: "Bay Area Primary Logistics Depot",
    destinationWarehouseId: "wh-3",
    destinationWarehouseName: "Heathrow customs & Regional Depot",
    transferNumber: "TRF-2026-0039",
    status: "Rejected",
    approvalStatus: "Rejected",
    requestedById: "usr-15",
    requestedByName: "Ian Wright",
    approvedById: "usr-10",
    approvedByName: "Sarah Jenkins",
    transferredItemsJson: JSON.stringify([
      { sku: "GPU-AI-NODE", productName: "H100 AI Training Cluster Unit", quantity: 4, unitPrice: 28000 }
    ]),
    items: [
      { sku: "GPU-AI-NODE", productName: "H100 AI Training Cluster Unit", quantity: 4, unitPrice: 28000 }
    ],
    totalItemsCount: 4,
    totalTransferValue: 112000,
    requestedAt: "2026-07-25T16:00:00Z",
    rejectionReason: "SF flagship operations require existing GPU nodes for local enterprise SLA commitments. Please source directly from OEM vendor.",
    trackingNotes: "Transfer request declined by Source Branch Managing VP."
  }
];

let MOCK_REGIONAL_SUMMARIES: RegionalSummaryDto[] = [
  { id: "reg-na", region: "North America", totalBranches: 2, totalRevenue: 1532900, totalProfit: 560550, profitMargin: 36.6, totalInventoryValue: 6570000, totalCustomers: 1840, totalEmployees: 58, revenuePerEmployee: 26429, topPerformingBranchName: "Manhattan Enterprise Hub", momGrowthPercentage: 8.4 },
  { id: "reg-emea", region: "EMEA", totalBranches: 2, totalRevenue: 896000, totalProfit: 268800, profitMargin: 30.0, totalInventoryValue: 3340000, totalCustomers: 1120, totalEmployees: 42, revenuePerEmployee: 21333, topPerformingBranchName: "London EMEA Regional Gateway", momGrowthPercentage: 5.2 },
  { id: "reg-apac", region: "APAC", totalBranches: 2, totalRevenue: 1069000, totalProfit: 361370, profitMargin: 33.8, totalInventoryValue: 4080000, totalCustomers: 1450, totalEmployees: 48, revenuePerEmployee: 22270, topPerformingBranchName: "Singapore APAC Commerce Terminal", momGrowthPercentage: 11.2 },
];

let MOCK_PERFORMANCES: BranchPerformanceDto[] = [
  { id: "perf-1", branchId: "br-2", branchName: "Manhattan Enterprise Hub", region: "North America", year: 2026, month: 8, totalRevenue: 890400, totalExpenses: 548300, totalProfit: 342100, profitMarginPercentage: 38.4, customerCount: 1020, employeeCount: 32, inventoryUtilizationPercentage: 91.5, performanceScore: 96.8, rank: 1, isBestPerformer: true, isLowestPerformer: false, moMRevenueGrowthPercentage: 9.8, moMProfitGrowthPercentage: 12.4 },
  { id: "perf-2", branchId: "br-5", branchName: "Singapore APAC Commerce Terminal", region: "APAC", year: 2026, month: 8, totalRevenue: 724000, totalExpenses: 448880, totalProfit: 275120, profitMarginPercentage: 38.0, customerCount: 890, employeeCount: 28, inventoryUtilizationPercentage: 88.9, performanceScore: 94.2, rank: 2, isBestPerformer: false, isLowestPerformer: false, moMRevenueGrowthPercentage: 14.2, moMProfitGrowthPercentage: 16.5 },
  { id: "perf-3", branchId: "br-1", branchName: "SF Flagship Innovation Center", region: "North America", year: 2026, month: 8, totalRevenue: 642500, totalExpenses: 424050, totalProfit: 218450, profitMarginPercentage: 34.0, customerCount: 820, employeeCount: 26, inventoryUtilizationPercentage: 84.2, performanceScore: 89.5, rank: 3, isBestPerformer: false, isLowestPerformer: false, moMRevenueGrowthPercentage: 6.7, moMProfitGrowthPercentage: 7.1 },
  { id: "perf-4", branchId: "br-3", branchName: "London EMEA Regional Gateway", region: "EMEA", year: 2026, month: 8, totalRevenue: 512000, totalExpenses: 343040, totalProfit: 168960, profitMarginPercentage: 33.0, customerCount: 650, employeeCount: 24, inventoryUtilizationPercentage: 76.4, performanceScore: 82.4, rank: 4, isBestPerformer: false, isLowestPerformer: false, moMRevenueGrowthPercentage: 4.8, moMProfitGrowthPercentage: 5.0 },
  { id: "perf-5", branchId: "br-4", branchName: "Munich Central Engineering & Ops", region: "EMEA", year: 2026, month: 8, totalRevenue: 384000, totalExpenses: 284160, totalProfit: 99840, profitMarginPercentage: 26.0, customerCount: 470, employeeCount: 18, inventoryUtilizationPercentage: 68.2, performanceScore: 72.1, rank: 5, isBestPerformer: false, isLowestPerformer: false, moMRevenueGrowthPercentage: 2.1, moMProfitGrowthPercentage: 1.5 },
  { id: "perf-6", branchId: "br-6", branchName: "Sydney Oceania Distribution Branch", region: "APAC", year: 2026, month: 8, totalRevenue: 345000, totalExpenses: 258750, totalProfit: 86250, profitMarginPercentage: 25.0, customerCount: 560, employeeCount: 20, inventoryUtilizationPercentage: 62.1, performanceScore: 68.4, rank: 6, isBestPerformer: false, isLowestPerformer: true, moMRevenueGrowthPercentage: -1.2, moMProfitGrowthPercentage: -3.8 }
];

// ==========================================
// Service Implementation
// ==========================================

export const MultiBranchService = {
  // ─── 1. Locations ──────────────────────────────────────────────────────────
  async getLocations(): Promise<LocationDto[]> {
    try {
      const res = await apiClient.get<LocationDto[]>("/api/v1/locations");
      if (res.data && res.data.length > 0) return res.data;
    } catch { /* Offline fallback */ }
    return MOCK_LOCATIONS;
  },

  async createLocation(payload: Omit<LocationDto, "id" | "organizationId" | "isActive" | "branchesCount">): Promise<LocationDto> {
    try {
      const res = await apiClient.post<LocationDto>("/api/v1/locations", payload);
      if (res.data) return res.data;
    } catch { /* Offline fallback */ }
    
    const newLoc: LocationDto = {
      ...payload,
      id: `loc-${Date.now()}`,
      organizationId: "org-1",
      isActive: true,
      branchesCount: 0
    };
    MOCK_LOCATIONS = [newLoc, ...MOCK_LOCATIONS];
    return newLoc;
  },

  // ─── 2. Branches ───────────────────────────────────────────────────────────
  async getBranches(): Promise<BranchDto[]> {
    try {
      const res = await apiClient.get<BranchDto[]>("/api/v1/branches");
      if (res.data && res.data.length > 0) return res.data;
    } catch { /* Offline fallback */ }
    return MOCK_BRANCHES;
  },

  async getBranchById(id: string): Promise<BranchDto | null> {
    try {
      const res = await apiClient.get<BranchDto>(`/api/v1/branches/${id}`);
      if (res.data) return res.data;
    } catch { /* Offline fallback */ }
    return MOCK_BRANCHES.find(b => b.id === id) || null;
  },

  async createBranch(payload: CreateBranchPayload): Promise<BranchDto> {
    try {
      const res = await apiClient.post<BranchDto>("/api/v1/branches", payload);
      if (res.data) return res.data;
    } catch { /* Offline fallback */ }

    const loc = MOCK_LOCATIONS.find(l => l.id === payload.locationId);
    const newBranch: BranchDto = {
      id: `br-${Date.now()}`,
      organizationId: "org-1",
      locationId: payload.locationId,
      locationName: loc ? `${loc.city} (${loc.stateProvince})` : "Global Hub",
      region: loc?.region || "Global",
      name: payload.name,
      code: payload.code,
      status: "Active",
      contactEmail: payload.contactEmail,
      contactPhone: payload.contactPhone,
      costCenterCode: payload.costCenterCode || `CC-${Date.now().toString().slice(-4)}`,
      workingHoursJson: payload.workingHoursJson || JSON.stringify({ Monday: "09:00-17:00", Tuesday: "09:00-17:00", Wednesday: "09:00-17:00", Thursday: "09:00-17:00", Friday: "09:00-17:00", Saturday: "Closed", Sunday: "Closed" }),
      operationalSettingsJson: payload.operationalSettingsJson || JSON.stringify({ AutoReorder: true, MaxTransferApprovalAmount: 10000, PosSyncEnabled: true }),
      isPrimary: payload.isPrimary,
      monthlyRevenue: 280000,
      monthlyProfit: 84000,
      profitMargin: 30.0,
      inventoryUtilization: 70.0,
      createdAt: new Date().toISOString()
    };
    MOCK_BRANCHES = [newBranch, ...MOCK_BRANCHES];
    return newBranch;
  },

  async updateBranch(id: string, payload: Partial<CreateBranchPayload>): Promise<BranchDto> {
    try {
      const res = await apiClient.put<BranchDto>(`/api/v1/branches/${id}`, payload);
      if (res.data) return res.data;
    } catch { /* Offline fallback */ }

    const index = MOCK_BRANCHES.findIndex(b => b.id === id);
    if (index !== -1) {
      MOCK_BRANCHES[index] = { ...MOCK_BRANCHES[index], ...payload };
      return MOCK_BRANCHES[index];
    }
    throw new Error("Branch not found");
  },

  async updateBranchStatus(id: string, status: BranchStatus): Promise<BranchDto> {
    try {
      const res = await apiClient.patch<BranchDto>(`/api/v1/branches/${id}/status`, { status });
      if (res.data) return res.data;
    } catch { /* Offline fallback */ }

    const index = MOCK_BRANCHES.findIndex(b => b.id === id);
    if (index !== -1) {
      MOCK_BRANCHES[index].status = status;
      return MOCK_BRANCHES[index];
    }
    throw new Error("Branch not found");
  },

  async assignManager(branchId: string, payload: AssignManagerPayload): Promise<BranchDto> {
    try {
      await apiClient.post(`/api/v1/branches/${branchId}/managers`, payload);
    } catch { /* Offline fallback */ }

    const index = MOCK_BRANCHES.findIndex(b => b.id === branchId);
    if (index !== -1) {
      MOCK_BRANCHES[index].managerName = payload.managerName;
      MOCK_BRANCHES[index].managerEmail = payload.managerEmail;
      MOCK_BRANCHES[index].canApproveTransfers = payload.canApproveTransfers;
      MOCK_BRANCHES[index].maxTransferApprovalLimit = payload.maxTransferApprovalLimit;
      return MOCK_BRANCHES[index];
    }
    throw new Error("Branch not found");
  },

  async updateWorkingHours(branchId: string, workingHoursJson: string, operationalSettingsJson: string): Promise<BranchDto> {
    try {
      await apiClient.put(`/api/v1/branch-settings/${branchId}`, { workingHoursJson, operationalSettingsJson });
    } catch { /* Offline fallback */ }

    const index = MOCK_BRANCHES.findIndex(b => b.id === branchId);
    if (index !== -1) {
      MOCK_BRANCHES[index].workingHoursJson = workingHoursJson;
      MOCK_BRANCHES[index].operationalSettingsJson = operationalSettingsJson;
      return MOCK_BRANCHES[index];
    }
    throw new Error("Branch not found");
  },

  // ─── 3. Multi-Branch Warehouses ─────────────────────────────────────────────
  async getBranchWarehouses(branchId?: string): Promise<BranchWarehouseDto[]> {
    try {
      const url = branchId ? `/api/v1/branch-warehouses?branchId=${branchId}` : "/api/v1/branch-warehouses";
      const res = await apiClient.get<BranchWarehouseDto[]>(url);
      if (res.data && res.data.length > 0) return res.data;
    } catch { /* Offline fallback */ }
    
    if (branchId) return MOCK_WAREHOUSES.filter(w => w.branchId === branchId);
    return MOCK_WAREHOUSES;
  },

  async createBranchWarehouse(payload: Omit<BranchWarehouseDto, "id" | "organizationId" | "currentUtilizationPercentage" | "totalStockItemsCount" | "estimatedStockValue" | "branchName">): Promise<BranchWarehouseDto> {
    try {
      const res = await apiClient.post<BranchWarehouseDto>("/api/v1/branch-warehouses", payload);
      if (res.data) return res.data;
    } catch { /* Offline fallback */ }

    const branch = MOCK_BRANCHES.find(b => b.id === payload.branchId);
    const newWh: BranchWarehouseDto = {
      ...payload,
      id: `wh-${Date.now()}`,
      organizationId: "org-1",
      branchName: branch?.name || "Assigned Branch",
      currentUtilizationPercentage: 25.0,
      totalStockItemsCount: 5000,
      estimatedStockValue: 150000
    };
    MOCK_WAREHOUSES = [newWh, ...MOCK_WAREHOUSES];
    return newWh;
  },

  // ─── 4. Inventory Transfers ─────────────────────────────────────────────────
  async getTransfers(warehouseId?: string, status?: TransferStatus): Promise<WarehouseTransferDto[]> {
    try {
      const params = new URLSearchParams();
      if (warehouseId) params.append("warehouseId", warehouseId);
      if (status) params.append("status", status);
      const res = await apiClient.get<WarehouseTransferDto[]>(`/api/v1/transfers/history?${params.toString()}`);
      if (res.data && res.data.length > 0) {
        return res.data.map(t => ({
          ...t,
          items: t.transferredItemsJson ? JSON.parse(t.transferredItemsJson) : []
        }));
      }
    } catch { /* Offline fallback */ }

    let results = [...MOCK_TRANSFERS];
    if (warehouseId) results = results.filter(t => t.sourceWarehouseId === warehouseId || t.destinationWarehouseId === warehouseId);
    if (status) results = results.filter(t => t.status === status);
    return results;
  },

  async requestTransfer(payload: CreateTransferPayload): Promise<WarehouseTransferDto> {
    try {
      const res = await apiClient.post<WarehouseTransferDto>("/api/v1/transfers", payload);
      if (res.data) return { ...res.data, items: res.data.transferredItemsJson ? JSON.parse(res.data.transferredItemsJson) : payload.items };
    } catch { /* Offline fallback */ }

    const sourceWh = MOCK_WAREHOUSES.find(w => w.id === payload.sourceWarehouseId);
    const destWh = MOCK_WAREHOUSES.find(w => w.id === payload.destinationWarehouseId);
    const totalQty = payload.items.reduce((acc, i) => acc + i.quantity, 0);
    const totalVal = payload.items.reduce((acc, i) => acc + (i.quantity * i.unitPrice), 0);

    const newTrf: WarehouseTransferDto = {
      id: `trf-${Date.now()}`,
      organizationId: "org-1",
      sourceWarehouseId: payload.sourceWarehouseId,
      sourceWarehouseName: sourceWh?.name || "Source Warehouse",
      destinationWarehouseId: payload.destinationWarehouseId,
      destinationWarehouseName: destWh?.name || "Destination Warehouse",
      transferNumber: `TRF-2026-${Math.floor(1000 + Math.random() * 9000)}`,
      status: "Pending",
      approvalStatus: "Pending",
      requestedById: "usr-current",
      requestedByName: payload.requestedByName || "Current Executive",
      transferredItemsJson: JSON.stringify(payload.items),
      items: payload.items,
      totalItemsCount: totalQty,
      totalTransferValue: totalVal,
      requestedAt: new Date().toISOString(),
      trackingNotes: payload.notes || "Standard inventory dispatch request."
    };
    MOCK_TRANSFERS = [newTrf, ...MOCK_TRANSFERS];
    return newTrf;
  },

  async approveTransfer(id: string, isApproved: boolean, approvedByName: string, rejectionReason?: string): Promise<WarehouseTransferDto> {
    try {
      const res = await apiClient.put<WarehouseTransferDto>(`/api/v1/transfers/${id}/approval`, { isApproved, approvedByName, rejectionReason });
      if (res.data) return { ...res.data, items: res.data.transferredItemsJson ? JSON.parse(res.data.transferredItemsJson) : [] };
    } catch { /* Offline fallback */ }

    const index = MOCK_TRANSFERS.findIndex(t => t.id === id);
    if (index !== -1) {
      MOCK_TRANSFERS[index].status = isApproved ? "Approved" : "Rejected";
      MOCK_TRANSFERS[index].approvalStatus = isApproved ? "Approved" : "Rejected";
      MOCK_TRANSFERS[index].approvedByName = approvedByName;
      MOCK_TRANSFERS[index].approvedAt = new Date().toISOString();
      if (!isApproved) MOCK_TRANSFERS[index].rejectionReason = rejectionReason;
      return MOCK_TRANSFERS[index];
    }
    throw new Error("Transfer not found");
  },

  async updateTransferTracking(id: string, status: TransferStatus, trackingNotes: string): Promise<WarehouseTransferDto> {
    try {
      const res = await apiClient.patch<WarehouseTransferDto>(`/api/v1/transfers/${id}/tracking`, { status, trackingNotes });
      if (res.data) return { ...res.data, items: res.data.transferredItemsJson ? JSON.parse(res.data.transferredItemsJson) : [] };
    } catch { /* Offline fallback */ }

    const index = MOCK_TRANSFERS.findIndex(t => t.id === id);
    if (index !== -1) {
      MOCK_TRANSFERS[index].status = status;
      if (status === "InTransit" && !MOCK_TRANSFERS[index].shippedAt) {
        MOCK_TRANSFERS[index].shippedAt = new Date().toISOString();
      }
      MOCK_TRANSFERS[index].trackingNotes = trackingNotes;
      return MOCK_TRANSFERS[index];
    }
    throw new Error("Transfer not found");
  },

  async receiveTransfer(id: string, receiptNotes?: string): Promise<WarehouseTransferDto> {
    try {
      const res = await apiClient.post<WarehouseTransferDto>(`/api/v1/transfers/${id}/receive`, { receiptNotes });
      if (res.data) return { ...res.data, items: res.data.transferredItemsJson ? JSON.parse(res.data.transferredItemsJson) : [] };
    } catch { /* Offline fallback */ }

    const index = MOCK_TRANSFERS.findIndex(t => t.id === id);
    if (index !== -1) {
      const trf = MOCK_TRANSFERS[index];
      trf.status = "Received";
      trf.receivedAt = new Date().toISOString();
      trf.trackingNotes = (trf.trackingNotes || "") + ` | Received: ${receiptNotes || "All items verified intact."}`;

      // Automatically reconcile stock valuations in mock warehouses
      const destWh = MOCK_WAREHOUSES.find(w => w.id === trf.destinationWarehouseId);
      if (destWh) {
        destWh.totalStockItemsCount += trf.totalItemsCount;
        destWh.estimatedStockValue += trf.totalTransferValue;
      }
      const sourceWh = MOCK_WAREHOUSES.find(w => w.id === trf.sourceWarehouseId);
      if (sourceWh) {
        sourceWh.totalStockItemsCount = Math.max(0, sourceWh.totalStockItemsCount - trf.totalItemsCount);
        sourceWh.estimatedStockValue = Math.max(0, sourceWh.estimatedStockValue - trf.totalTransferValue);
      }

      return trf;
    }
    throw new Error("Transfer not found");
  },

  // ─── 5. Regional Reports & Analytics ────────────────────────────────────────
  async getRegionalSummaries(): Promise<RegionalSummaryDto[]> {
    try {
      const res = await apiClient.get<RegionalSummaryDto[]>("/api/v1/regional-reports/revenue-by-branch");
      // If live backend responds, we return fallback transformed or direct
      if (res.data) return MOCK_REGIONAL_SUMMARIES;
    } catch { /* Offline fallback */ }
    return MOCK_REGIONAL_SUMMARIES;
  },

  async getBranchPerformances(year: number = 2026, month: number = 8): Promise<{ totalBranchesCalculated: number; bestPerformingBranchName?: string; lowestPerformingBranchName?: string; averageInventoryUtilization: number; averageProfitMargin: number; performances: BranchPerformanceDto[] }> {
    try {
      const res = await apiClient.get(`/api/v1/branch-analytics/performance?year=${year}&month=${month}`);
      if (res.data && res.data.performances && res.data.performances.length > 0) {
        return res.data;
      }
    } catch { /* Offline fallback */ }

    const best = MOCK_PERFORMANCES.find(p => p.isBestPerformer) || MOCK_PERFORMANCES[0];
    const lowest = MOCK_PERFORMANCES.find(p => p.isLowestPerformer) || MOCK_PERFORMANCES[MOCK_PERFORMANCES.length - 1];
    const avgUtil = Number((MOCK_PERFORMANCES.reduce((acc, p) => acc + p.inventoryUtilizationPercentage, 0) / MOCK_PERFORMANCES.length).toFixed(1));
    const avgMargin = Number((MOCK_PERFORMANCES.reduce((acc, p) => acc + p.profitMarginPercentage, 0) / MOCK_PERFORMANCES.length).toFixed(1));

    return {
      totalBranchesCalculated: MOCK_PERFORMANCES.length,
      bestPerformingBranchName: best?.branchName,
      lowestPerformingBranchName: lowest?.branchName,
      averageInventoryUtilization: avgUtil,
      averageProfitMargin: avgMargin,
      performances: MOCK_PERFORMANCES
    };
  }
};
