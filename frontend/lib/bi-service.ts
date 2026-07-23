import apiClient from "./api-client";

export interface KPI {
  id: string;
  organizationId: string;
  name: string;
  category: string;
  currentValue: number;
  targetValue?: number;
  unit: string;
  trend: string;
  lastCalculatedAt: string;
}

export interface RecalculateKPIsResponse {
  updatedCount: number;
  kpIs: KPI[];
  recalculatedAt: string;
}

export interface DashboardMetricItem {
  key: string;
  label: string;
  currentValue: number;
  previousValue: number;
  percentageChange: number;
  unit: string;
  trend: string;
}

export interface ChartDataPoint {
  dateLabel: string;
  date: string;
  value: number;
  comparisonValue?: number;
}

export interface Insight {
  id: string;
  organizationId: string;
  category: string;
  title: string;
  description: string;
  priority: "Low" | "Medium" | "High" | "Critical";
  recommendation: string;
  businessImpact: string;
  suggestedAction: string;
  createdAt: string;
}

export interface Goal {
  id: string;
  organizationId: string;
  name: string;
  targetValue: number;
  currentValue: number;
  progressPercentage: number;
  startDate: string;
  endDate: string;
  status: "NotStarted" | "InProgress" | "Achieved" | "Behind" | "Failed";
  createdAt: string;
}

export interface CreateGoalPayload {
  name: string;
  targetValue: number;
  initialValue?: number;
  startDate?: string;
  endDate?: string;
}

export interface UpdateGoalPayload {
  name?: string;
  targetValue?: number;
  currentValue?: number;
  startDate?: string;
  endDate?: string;
  status?: string;
}

export interface ForecastPoint {
  id: string;
  organizationId: string;
  forecastType: string;
  predictedValue: number;
  confidenceScore: number;
  forecastDate: string;
  createdAt: string;
}

export interface ForecastSummary {
  forecastType: string;
  totalPredicted: number;
  growthRate: number;
  averageConfidence: number;
  dataPoints: ForecastPoint[];
}

export interface Report {
  id: string;
  organizationId: string;
  name: string;
  reportType: string;
  filters: string;
  generatedAt: string;
  generatedBy: string;
  format: string;
  fileUrl?: string;
  dataPayload?: any;
}

export interface GenerateReportPayload {
  name?: string;
  reportType: string;
  format?: string;
  startDate?: string;
  endDate?: string;
  filterParams?: Record<string, string>;
}

export interface DashboardFilter {
  period?: string;
  startDate?: string;
  endDate?: string;
  categoryFilter?: string;
}

export interface CEODashboard {
  generatedAt: string;
  periodLabel: string;
  totalRevenue: number;
  totalLeads: number;
  activeCustomers: number;
  overallHealthScore: number;
  keyMetrics: DashboardMetricItem[];
  primaryRevenueTrend: ChartDataPoint[];
  growthVelocity: ChartDataPoint[];
  topInsights: Insight[];
  activeGoals: Goal[];
}

export interface SalesDashboard {
  generatedAt: string;
  periodLabel: string;
  totalPipelineValue: number;
  wonDealsValue: number;
  totalDealsCount: number;
  winRate: number;
  averageDealSize: number;
  averageSalesCycleDays: number;
  keyMetrics: DashboardMetricItem[];
  repRankings: Array<{
    userId: string;
    memberName: string;
    role: string;
    activitiesCount: number;
    dealsWonCount: number;
    revenueGenerated: number;
    conversionRate: number;
  }>;
  topInsights: Insight[];
  activeGoals: Goal[];
}

export interface MarketingDashboard {
  generatedAt: string;
  periodLabel: string;
  totalLeadsGenerated: number;
  leadConversionRate: number;
  totalCampaigns: number;
  averageCampaignROI: number;
  totalQRScans: number;
  keyMetrics: DashboardMetricItem[];
  campaignPerformance: Array<{
    campaignId: string;
    name: string;
    budget: number;
    leadsGenerated: number;
    revenueGenerated: number;
    roi: number;
  }>;
  topInsights: Insight[];
  activeGoals: Goal[];
}

export interface FinanceDashboard {
  generatedAt: string;
  periodLabel: string;
  totalRevenue: number;
  monthlyRecurringRevenue: number;
  annualRecurringRevenue: number;
  totalOutstandingInvoices: number;
  churnRate: number;
  keyMetrics: DashboardMetricItem[];
  topInsights: Insight[];
  activeGoals: Goal[];
}

export interface OperationsDashboard {
  generatedAt: string;
  periodLabel: string;
  activeWorkflows: number;
  totalWorkflowExecutions: number;
  executionSuccessRate: number;
  totalAIRequests: number;
  connectedIntegrations: number;
  keyMetrics: DashboardMetricItem[];
  topInsights: Insight[];
  activeGoals: Goal[];
}

export interface TeamDashboard {
  generatedAt: string;
  periodLabel: string;
  totalTeamMembers: number;
  totalActivitiesLogged: number;
  totalDealsClosed: number;
  totalRevenueGenerated: number;
  memberDetails: Array<{
    userId: string;
    memberName: string;
    role: string;
    activitiesCount: number;
    dealsWonCount: number;
    revenueGenerated: number;
    conversionRate: number;
  }>;
  keyMetrics: DashboardMetricItem[];
  topInsights: Insight[];
  activeGoals: Goal[];
}

export const biService = {
  // KPIs
  getKPIs: async (category?: string) => {
    const res = await apiClient.get<KPI[]>("/api/v1/bi/kpis", { params: { category } });
    return res.data;
  },
  getKPIByName: async (name: string) => {
    const res = await apiClient.get<KPI>(`/api/v1/bi/kpis/${encodeURIComponent(name)}`);
    return res.data;
  },
  recalculateKPIs: async () => {
    const res = await apiClient.post<RecalculateKPIsResponse>("/api/v1/bi/kpis/recalculate");
    return res.data;
  },

  // Dashboards
  getCEODashboard: async (filter?: DashboardFilter) => {
    const res = await apiClient.get<CEODashboard>("/api/v1/bi/dashboard/ceo", { params: filter });
    return res.data;
  },
  getSalesDashboard: async (filter?: DashboardFilter) => {
    const res = await apiClient.get<SalesDashboard>("/api/v1/bi/dashboard/sales", { params: filter });
    return res.data;
  },
  getMarketingDashboard: async (filter?: DashboardFilter) => {
    const res = await apiClient.get<MarketingDashboard>("/api/v1/bi/dashboard/marketing", { params: filter });
    return res.data;
  },
  getFinanceDashboard: async (filter?: DashboardFilter) => {
    const res = await apiClient.get<FinanceDashboard>("/api/v1/bi/dashboard/finance", { params: filter });
    return res.data;
  },
  getOperationsDashboard: async (filter?: DashboardFilter) => {
    const res = await apiClient.get<OperationsDashboard>("/api/v1/bi/dashboard/operations", { params: filter });
    return res.data;
  },
  getTeamDashboard: async (filter?: DashboardFilter) => {
    const res = await apiClient.get<TeamDashboard>("/api/v1/bi/dashboard/team", { params: filter });
    return res.data;
  },

  // Insights
  getInsights: async (category?: string, priority?: string) => {
    const res = await apiClient.get<Insight[]>("/api/v1/bi/insights", { params: { category, priority } });
    return res.data;
  },
  generateInsights: async () => {
    const res = await apiClient.post<{ generatedCount: number; insights: Insight[]; generatedAt: string }>("/api/v1/bi/insights/generate");
    return res.data;
  },

  // Forecasting
  getForecast: async (type: string) => {
    const res = await apiClient.get<ForecastSummary>(`/api/v1/bi/forecasting/${encodeURIComponent(type)}`);
    return res.data;
  },
  generateForecast: async (forecastType: string, horizonDays = 30) => {
    const res = await apiClient.post<ForecastSummary>("/api/v1/bi/forecasting/generate", { forecastType, horizonDays });
    return res.data;
  },

  // Reports
  getReports: async (reportType?: string) => {
    const res = await apiClient.get<Report[]>("/api/v1/bi/reports", { params: { reportType } });
    return res.data;
  },
  getReportById: async (id: string) => {
    const res = await apiClient.get<Report>(`/api/v1/bi/reports/${id}`);
    return res.data;
  },
  generateReport: async (payload: GenerateReportPayload) => {
    const res = await apiClient.post<Report>("/api/v1/bi/reports/generate", payload);
    return res.data;
  },
  exportReport: async (id: string, format = "csv") => {
    const res = await apiClient.get(`/api/v1/bi/reports/${id}/export`, {
      params: { format },
      responseType: "blob"
    });
    return res;
  },

  // Goals
  getGoals: async (status?: string) => {
    const res = await apiClient.get<Goal[]>("/api/v1/bi/goals", { params: { status } });
    return res.data;
  },
  getGoalById: async (id: string) => {
    const res = await apiClient.get<Goal>(`/api/v1/bi/goals/${id}`);
    return res.data;
  },
  createGoal: async (payload: CreateGoalPayload) => {
    const res = await apiClient.post<Goal>("/api/v1/bi/goals", payload);
    return res.data;
  },
  updateGoal: async (id: string, payload: UpdateGoalPayload) => {
    const res = await apiClient.put<Goal>(`/api/v1/bi/goals/${id}`, payload);
    return res.data;
  },
  deleteGoal: async (id: string) => {
    const res = await apiClient.delete(`/api/v1/bi/goals/${id}`);
    return res.data;
  },
  syncGoals: async () => {
    const res = await apiClient.post<Goal[]>("/api/v1/bi/goals/sync");
    return res.data;
  }
};
