import apiClient from "./api-client";

// ==========================================
// DTOs & Interfaces
// ==========================================

export interface BusinessHealthSummary {
  healthScore: number;
  revenueGrowthRate: number;
  profitGrowthRate: number;
  customerGrowthRate: number;
  mrr: number;
  arr: number;
  grossMarginPercentage: number;
  netRevenueRetention: number; // NRR %
  ltvCacRatio: number;
  marketingRoi: number;
  activeCustomersCount: number;
  churnRate: number;
  lastCalculatedAt: string;
}

export interface RevenueTrendItem {
  period: string;
  revenue: number;
  profit: number;
  expenses: number;
  newMrr: number;
  expansionMrr: number;
  contractionMrr: number;
  churnedMrr: number;
  targetRevenue?: number;
}

export interface RevenueByRegion {
  region: string;
  revenue: number;
  percentage: number;
  growthYoY: number;
  activeCustomers: number;
}

export interface ProductPerformanceItem {
  id: string;
  sku: string;
  name: string;
  category: string;
  revenue: number;
  unitsSold: number;
  unitCost: number;
  unitPrice: number;
  grossMarginPercentage: number;
  stockLevel: number;
  status: "High Performing" | "Steady" | "Slow Moving" | "Loss Center";
  aiRecommendedAction: string;
  estimatedImpact: number;
}

export interface CustomerLtvCacItem {
  customerId: string;
  customerName: string;
  companyName: string;
  tier: string;
  mrr: number;
  ltv: number;
  cac: number;
  ltvCacRatio: number;
  repeatPurchaseRate: number;
  churnRiskScore: "Low" | "Medium" | "High" | "Critical";
  lastActiveDaysAgo: number;
  upsellOpportunity: string;
  estimatedUpsellValue: number;
}

export interface MarketingRoiChannel {
  id: string;
  channelName: string;
  spend: number;
  revenueGenerated: number;
  roas: number; // Return on Ad Spend
  cac: number;
  conversions: number;
  conversionRate: number;
  trend: "Trending Up" | "Stable" | "Underperforming";
  suggestedBudgetShift: number; // + or - $
  estimatedArrImpact: number;
}

export interface GrowthRecommendationDto {
  id: string;
  title: string;
  category: "Campaign Optimization" | "Inventory Reorder" | "VIP Retention" | "Upsell Expansion" | "Loss Elimination";
  description: string;
  suggestedAction: string;
  estimatedRevenueImpact: number;
  costReductionImpact: number;
  priority: "High" | "Medium" | "Low";
  confidenceScore: number; // 0-100%
  targetEntityId?: string;
  targetEntityName?: string;
  status: "Open" | "In-Progress" | "Actioned" | "Dismissed";
}

export interface IndustryBenchmarkItem {
  metricName: string;
  companyValue: number;
  industryMedian: number;
  topQuartileValue: number;
  unit: string;
  percentileRank: number;
  status: "Above Top Quartile" | "Above Median" | "Below Median" | "Critical Action Needed";
  aiAdvice: string;
}

// ==========================================
// Realistic Enterprise Mock Datasets (High ROI)
// ==========================================

let MOCK_HEALTH_SUMMARY: BusinessHealthSummary = {
  healthScore: 86,
  revenueGrowthRate: 28.4,
  profitGrowthRate: 34.2,
  customerGrowthRate: 18.9,
  mrr: 104500,
  arr: 1254000,
  grossMarginPercentage: 82.5,
  netRevenueRetention: 124.8,
  ltvCacRatio: 4.6,
  marketingRoi: 3.4,
  activeCustomersCount: 428,
  churnRate: 1.4,
  lastCalculatedAt: new Date().toISOString(),
};

let MOCK_REVENUE_TRENDS: RevenueTrendItem[] = [
  { period: "Feb", revenue: 82400, profit: 64200, expenses: 18200, newMrr: 12000, expansionMrr: 4500, contractionMrr: 1200, churnedMrr: 1500, targetRevenue: 80000 },
  { period: "Mar", revenue: 86900, profit: 68100, expenses: 18800, newMrr: 13200, expansionMrr: 5200, contractionMrr: 800, churnedMrr: 1400, targetRevenue: 85000 },
  { period: "Apr", revenue: 91400, profit: 72300, expenses: 19100, newMrr: 14000, expansionMrr: 6100, contractionMrr: 1000, churnedMrr: 1200, targetRevenue: 90000 },
  { period: "May", revenue: 95200, profit: 75800, expenses: 19400, newMrr: 12500, expansionMrr: 7200, contractionMrr: 900, churnedMrr: 1500, targetRevenue: 95000 },
  { period: "Jun", revenue: 99800, profit: 80400, expenses: 19400, newMrr: 15200, expansionMrr: 6800, contractionMrr: 1100, churnedMrr: 1000, targetRevenue: 98000 },
  { period: "Jul", revenue: 104500, profit: 85100, expenses: 19400, newMrr: 16800, expansionMrr: 8400, contractionMrr: 600, churnedMrr: 800, targetRevenue: 104000 },
];

let MOCK_REVENUE_BY_REGION: RevenueByRegion[] = [
  { region: "North America", revenue: 647900, percentage: 51.7, growthYoY: 32.4, activeCustomers: 224 },
  { region: "Europe (EMEA)", revenue: 351120, percentage: 28.0, growthYoY: 26.8, activeCustomers: 121 },
  { region: "Asia-Pacific (APAC)", revenue: 175560, percentage: 14.0, growthYoY: 42.1, activeCustomers: 64 },
  { region: "Latin America (LATAM)", revenue: 79420, percentage: 6.3, growthYoY: 19.5, activeCustomers: 19 },
];

let MOCK_PRODUCT_PERFORMANCE: ProductPerformanceItem[] = [
  {
    id: "prod-1",
    sku: "BOS-ENT-AI",
    name: "BusinessOS Enterprise Copilot Suite",
    category: "SaaS Subscription",
    revenue: 584000,
    unitsSold: 142,
    unitCost: 280,
    unitPrice: 4112,
    grossMarginPercentage: 93.2,
    stockLevel: 999,
    status: "High Performing",
    aiRecommendedAction: "Raise enterprise new user tier contract pricing by 15% to capture excess LTV value.",
    estimatedImpact: 87600,
  },
  {
    id: "prod-2",
    sku: "BOS-PRO-WF",
    name: "Visual Automation & Workflow Engine",
    category: "SaaS Subscription",
    revenue: 342000,
    unitsSold: 218,
    unitCost: 150,
    unitPrice: 1568,
    grossMarginPercentage: 90.4,
    stockLevel: 999,
    status: "High Performing",
    aiRecommendedAction: "Trigger automated cross-sell sequence for CRM accounts nearing workflow usage limits.",
    estimatedImpact: 51300,
  },
  {
    id: "prod-3",
    sku: "BOS-API-DED",
    name: "Dedicated API Gateway & Webhook Cluster",
    category: "Infrastructure",
    revenue: 168000,
    unitsSold: 48,
    unitCost: 850,
    unitPrice: 3500,
    grossMarginPercentage: 75.7,
    stockLevel: 12,
    status: "Steady",
    aiRecommendedAction: "Reorder infrastructure reserve capacity before seasonal enterprise billing spike in Q3.",
    estimatedImpact: 24000,
  },
  {
    id: "prod-4",
    sku: "BOS-LEGACY-SVC",
    name: "Legacy Custom Scripting Service & Support",
    category: "Professional Services",
    revenue: 32000,
    unitsSold: 8,
    unitCost: 4800,
    unitPrice: 4000,
    grossMarginPercentage: -20.0,
    stockLevel: 0,
    status: "Loss Center",
    aiRecommendedAction: "Sunset negative-margin custom legacy scripting tier immediately and transition accounts to AI Copilot.",
    estimatedImpact: 19200, // savings
  },
  {
    id: "prod-5",
    sku: "BOS-HW-BEACON",
    name: "Physical Bluetooth Visitor Tracking Beacons",
    category: "Hardware Accessory",
    revenue: 14500,
    unitsSold: 29,
    unitCost: 380,
    unitPrice: 500,
    grossMarginPercentage: 24.0,
    stockLevel: 310,
    status: "Slow Moving",
    aiRecommendedAction: "Reduce excess physical warehouse stock with a discounted inventory clearing bundle.",
    estimatedImpact: 11500,
  },
];

let MOCK_CUSTOMER_LTV_CAC: CustomerLtvCacItem[] = [
  {
    customerId: "cust-1",
    customerName: "Elena Vance (COO)",
    companyName: "VentureScale AI",
    tier: "Enterprise Pro",
    mrr: 4800,
    ltv: 184000,
    cac: 12500,
    ltvCacRatio: 14.7,
    repeatPurchaseRate: 98.4,
    churnRiskScore: "Low",
    lastActiveDaysAgo: 1,
    upsellOpportunity: "Add dedicated AI Agent Tool Registry license package.",
    estimatedUpsellValue: 18000,
  },
  {
    customerId: "cust-2",
    customerName: "Marcus Sterling (CEO)",
    companyName: "Sterling Logistics Corp",
    tier: "Enterprise Pro",
    mrr: 3600,
    ltv: 142000,
    cac: 14000,
    ltvCacRatio: 10.1,
    repeatPurchaseRate: 95.0,
    churnRiskScore: "Low",
    lastActiveDaysAgo: 0,
    upsellOpportunity: "Expand Multi-Warehouse Purchasing Automation module.",
    estimatedUpsellValue: 14400,
  },
  {
    customerId: "cust-3",
    customerName: "Dr. Rachel Thorne",
    companyName: "BioNexus Diagnostics",
    tier: "Professional",
    mrr: 1850,
    ltv: 64000,
    cac: 18000,
    ltvCacRatio: 3.5,
    repeatPurchaseRate: 88.0,
    churnRiskScore: "Medium",
    lastActiveDaysAgo: 18,
    upsellOpportunity: "Upgrade to HIPAA-Compliant Dedicated Cluster.",
    estimatedUpsellValue: 9600,
  },
  {
    customerId: "cust-4",
    customerName: "Julian Vance (VP Ops)",
    companyName: "Apex Retail Solutions",
    tier: "Professional",
    mrr: 2100,
    ltv: 48000,
    cac: 22000,
    ltvCacRatio: 2.1,
    repeatPurchaseRate: 64.0,
    churnRiskScore: "High",
    lastActiveDaysAgo: 52,
    upsellOpportunity: "Schedule executive intervention to resolve webhook delays before contract renewal.",
    estimatedUpsellValue: 25200, // saved ARR
  },
  {
    customerId: "cust-5",
    customerName: "Samuel Drake",
    companyName: "Odyssey Fintech",
    tier: "Starter",
    mrr: 950,
    ltv: 18000,
    cac: 24000,
    ltvCacRatio: 0.75,
    repeatPurchaseRate: 35.0,
    churnRiskScore: "Critical",
    lastActiveDaysAgo: 68,
    upsellOpportunity: "Trigger automated VIP Customer Win-Back sequence immediately.",
    estimatedUpsellValue: 11400,
  },
];

let MOCK_MARKETING_ROI: MarketingRoiChannel[] = [
  {
    id: "chan-1",
    channelName: "LinkedIn B2B Executive Targeting",
    spend: 24500,
    revenueGenerated: 128400,
    roas: 5.24,
    cac: 4200,
    conversions: 28,
    conversionRate: 4.8,
    trend: "Trending Up",
    suggestedBudgetShift: 8500,
    estimatedArrImpact: 44540,
  },
  {
    id: "chan-2",
    channelName: "Google Search (SaaS Keywords)",
    spend: 32000,
    revenueGenerated: 112000,
    roas: 3.50,
    cac: 5100,
    conversions: 36,
    conversionRate: 3.2,
    trend: "Stable",
    suggestedBudgetShift: 2000,
    estimatedArrImpact: 7000,
  },
  {
    id: "chan-3",
    channelName: "Tech Webinars & Partner Syndication",
    spend: 15000,
    revenueGenerated: 63000,
    roas: 4.20,
    cac: 3800,
    conversions: 16,
    conversionRate: 6.5,
    trend: "Trending Up",
    suggestedBudgetShift: 5000,
    estimatedArrImpact: 21000,
  },
  {
    id: "chan-4",
    channelName: "Broad Display Retargeting & Twitter Ads",
    spend: 18500,
    revenueGenerated: 22200,
    roas: 1.20,
    cac: 18500,
    conversions: 4,
    conversionRate: 0.4,
    trend: "Underperforming",
    suggestedBudgetShift: -15500,
    estimatedArrImpact: 15500, // saved waste
  },
];

let MOCK_RECOMMENDATIONS: GrowthRecommendationDto[] = [
  {
    id: "rec-1",
    title: "Increase Budget for Campaign A (LinkedIn Executive B2B)",
    category: "Campaign Optimization",
    description: "LinkedIn B2B Executive Targeting is achieving a 5.24x ROAS with an enterprise CAC of only $4,200. Shift $8,500 from underperforming broad ads into this channel.",
    suggestedAction: "Reallocate $8,500/mo ad budget from Display Ads directly into LinkedIn B2B Executive Targeting.",
    estimatedRevenueImpact: 44540,
    costReductionImpact: 15500,
    priority: "High",
    confidenceScore: 96.4,
    targetEntityId: "chan-1",
    targetEntityName: "LinkedIn B2B Executive Targeting",
    status: "Open",
  },
  {
    id: "rec-2",
    title: "Reorder Product X (Dedicated API Gateway Reserve Capacity)",
    category: "Inventory Reorder",
    description: "Dedicated API Gateway Cluster reserve units have dropped below 15 units. Historical Q3 billing data forecasts an upcoming enterprise usage surge.",
    suggestedAction: "Authorize instant purchase order for 50 reserve cluster licenses from cloud vendor to avoid fulfillment bottlenecks.",
    estimatedRevenueImpact: 24000,
    costReductionImpact: 4500,
    priority: "High",
    confidenceScore: 92.1,
    targetEntityId: "prod-3",
    targetEntityName: "Dedicated API Gateway & Webhook Cluster",
    status: "Open",
  },
  {
    id: "rec-3",
    title: "Contact Inactive VIP Customers (Apex Retail & Odyssey)",
    category: "VIP Retention",
    description: "2 high-value VIP accounts ($3,050 combined MRR) demonstrate login dormancy >50 days and declining API calls, indicating severe churn risk.",
    suggestedAction: "Trigger automated AI executive check-in and schedule priority success intervention call.",
    estimatedRevenueImpact: 36600, // annual saved ARR
    costReductionImpact: 0,
    priority: "High",
    confidenceScore: 94.8,
    targetEntityId: "cust-4",
    targetEntityName: "Apex Retail Solutions & Odyssey Fintech",
    status: "Open",
  },
  {
    id: "rec-4",
    title: "Upsell Premium Plan to Customer Y (VentureScale AI)",
    category: "Upsell Expansion",
    description: "VentureScale AI has reached 94% of their active workflow limit and has expanded their engineering seats by 40% this quarter.",
    suggestedAction: "Propose seamless upgrade to Dedicated AI Agent Tool Registry package (+$,1500/mo MRR) with automated one-click Stripe discount acceptance.",
    estimatedRevenueImpact: 18000,
    costReductionImpact: 0,
    priority: "Medium",
    confidenceScore: 89.5,
    targetEntityId: "cust-1",
    targetEntityName: "VentureScale AI",
    status: "Open",
  },
  {
    id: "rec-5",
    title: "Reduce Stock of Slow-Moving Products & Eliminate Loss Center",
    category: "Loss Elimination",
    description: "Legacy Custom Scripting Service runs at a negative gross margin (-20%) due to intense manual engineer maintenance, while Bluetooth beacons consume dead shelf space.",
    suggestedAction: "Sunset legacy custom scripting tier instantly (transitioning users to AI Copilot) and launch a clearing sale for Bluetooth beacons.",
    estimatedRevenueImpact: 11500,
    costReductionImpact: 19200,
    priority: "High",
    confidenceScore: 98.2,
    targetEntityId: "prod-4",
    targetEntityName: "Legacy Custom Scripting Service",
    status: "Open",
  },
];

let MOCK_BENCHMARKS: IndustryBenchmarkItem[] = [
  { metricName: "Net Revenue Retention (NRR)", companyValue: 124.8, industryMedian: 104.0, topQuartileValue: 120.0, unit: "%", percentileRank: 88, status: "Above Top Quartile", aiAdvice: "World-class expansion model! Continue investing in automated product cross-sells." },
  { metricName: "LTV : CAC Ratio", companyValue: 4.6, industryMedian: 2.8, topQuartileValue: 4.0, unit: "x", percentileRank: 82, status: "Above Top Quartile", aiAdvice: "Unit economics are prime. Accelerate acquisition spending in top converting channels." },
  { metricName: "Gross Profit Margin", companyValue: 82.5, industryMedian: 74.0, topQuartileValue: 80.0, unit: "%", percentileRank: 79, status: "Above Top Quartile", aiAdvice: "SaaS gross margin is extremely healthy. Eliminating legacy scripting loss center will push margin to 86%." },
  { metricName: "Annual Customer Churn Rate", companyValue: 1.4, industryMedian: 8.5, topQuartileValue: 4.0, unit: "%", percentileRank: 94, status: "Above Top Quartile", aiAdvice: "Exceptional stickiness! Proactive win-back sequences prevent late-stage drop-off." },
  { metricName: "Marketing ROAS", companyValue: 3.4, industryMedian: 2.5, topQuartileValue: 3.8, unit: "x", percentileRank: 68, status: "Above Median", aiAdvice: "Shift spend from low-performing display networks into LinkedIn ABM to reach the top quartile." },
];

// ==========================================
// Service Methods
// ==========================================

export const BusinessPerformanceService = {
  // 1. Get Summary & Health
  async getHealthSummary(): Promise<BusinessHealthSummary> {
    try {
      const res = await apiClient.get<BusinessHealthSummary>("/api/v1/business-performance/summary");
      if (res.data) return res.data;
    } catch {
      // Offline fallback
    }
    return MOCK_HEALTH_SUMMARY;
  },

  // 2. Get Revenue Trends & Regions
  async getRevenueAnalytics(): Promise<{ trends: RevenueTrendItem[]; regions: RevenueByRegion[] }> {
    try {
      const res = await apiClient.get<{ trends: RevenueTrendItem[]; regions: RevenueByRegion[] }>("/api/v1/revenue-analytics/trends");
      if (res.data?.trends) return res.data;
    } catch {
      // Offline fallback
    }
    return { trends: MOCK_REVENUE_TRENDS, regions: MOCK_REVENUE_BY_REGION };
  },

  // 3. Get Product Performance Matrix
  async getProductAnalytics(): Promise<ProductPerformanceItem[]> {
    try {
      const res = await apiClient.get<ProductPerformanceItem[]>("/api/v1/product-analytics/matrix");
      if (res.data) return res.data;
    } catch {
      // Offline fallback
    }
    return MOCK_PRODUCT_PERFORMANCE;
  },

  // 4. Get Customer LTV/CAC & Churn Risk
  async getCustomerAnalytics(): Promise<CustomerLtvCacItem[]> {
    try {
      const res = await apiClient.get<CustomerLtvCacItem[]>("/api/v1/customer-analytics/ltv-cac");
      if (res.data) return res.data;
    } catch {
      // Offline fallback
    }
    return MOCK_CUSTOMER_LTV_CAC;
  },

  // 5. Get Marketing ROI Channels
  async getMarketingRoi(): Promise<MarketingRoiChannel[]> {
    try {
      const res = await apiClient.get<MarketingRoiChannel[]>("/api/v1/marketing-roi/channels");
      if (res.data) return res.data;
    } catch {
      // Offline fallback
    }
    return MOCK_MARKETING_ROI;
  },

  // 6. Get Growth Recommendations (AI Growth Center)
  async getGrowthRecommendations(): Promise<GrowthRecommendationDto[]> {
    try {
      const res = await apiClient.get<GrowthRecommendationDto[]>("/api/v1/growth-center/recommendations");
      if (res.data) return res.data;
    } catch {
      // Offline fallback
    }
    return MOCK_RECOMMENDATIONS;
  },

  // 7. Get Industry Benchmarks
  async getBenchmarks(): Promise<IndustryBenchmarkItem[]> {
    try {
      const res = await apiClient.get<IndustryBenchmarkItem[]>("/api/v1/benchmarks/comparison");
      if (res.data) return res.data;
    } catch {
      // Offline fallback
    }
    return MOCK_BENCHMARKS;
  },

  // ==========================================
  // Interactive Business Action Mutations
  // ==========================================
  async executeGrowthAction(id: string, actionType: string): Promise<GrowthRecommendationDto | undefined> {
    try {
      const res = await apiClient.post<GrowthRecommendationDto>(`/api/v1/growth-center/recommendations/${id}/execute`, { actionType });
      if (res.data) return res.data;
    } catch {
      // Offline simulation
    }
    const idx = MOCK_RECOMMENDATIONS.findIndex(r => r.id === id);
    if (idx !== -1) {
      MOCK_RECOMMENDATIONS[idx] = { ...MOCK_RECOMMENDATIONS[idx], status: "Actioned" };
      return MOCK_RECOMMENDATIONS[idx];
    }
    return undefined;
  },

  async reallocateMarketingBudget(channelId: string, shiftAmount: number): Promise<MarketingRoiChannel | undefined> {
    try {
      const res = await apiClient.post<MarketingRoiChannel>(`/api/v1/marketing-roi/reallocate`, { channelId, shiftAmount });
      if (res.data) return res.data;
    } catch {
      // Offline simulation
    }
    const idx = MOCK_MARKETING_ROI.findIndex(c => c.id === channelId);
    if (idx !== -1) {
      MOCK_MARKETING_ROI[idx] = { 
        ...MOCK_MARKETING_ROI[idx], 
        spend: MOCK_MARKETING_ROI[idx].spend + shiftAmount,
        trend: "Trending Up"
      };
      return MOCK_MARKETING_ROI[idx];
    }
    return undefined;
  },

  async optimizeStockOrPricing(productId: string, actionName: string): Promise<ProductPerformanceItem | undefined> {
    try {
      const res = await apiClient.post<ProductPerformanceItem>(`/api/v1/product-analytics/${productId}/optimize`, { actionName });
      if (res.data) return res.data;
    } catch {
      // Offline simulation
    }
    const idx = MOCK_PRODUCT_PERFORMANCE.findIndex(p => p.id === productId);
    if (idx !== -1) {
      MOCK_PRODUCT_PERFORMANCE[idx] = {
        ...MOCK_PRODUCT_PERFORMANCE[idx],
        status: MOCK_PRODUCT_PERFORMANCE[idx].status === "Loss Center" ? "Steady" : "High Performing",
        grossMarginPercentage: Math.min(95, MOCK_PRODUCT_PERFORMANCE[idx].grossMarginPercentage + 15)
      };
      return MOCK_PRODUCT_PERFORMANCE[idx];
    }
    return undefined;
  },
};
