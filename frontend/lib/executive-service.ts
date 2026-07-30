import apiClient from "./api-client";

// DTOs
export interface KpiDto {
  id: string;
  organizationId: string;
  category: string;
  name: string;
  description: string;
  currentValue: number;
  targetValue: number;
  unit: string;
  trend: string;
  lastCalculatedAt: string;
}

export interface KpiHistoryDto {
  id: string;
  kpiId: string;
  value: number;
  recordedAt: string;
}

export interface ForecastDto {
  id: string;
  organizationId: string;
  metricName: string;
  predictedValue: number;
  lowerBound: number;
  upperBound: number;
  targetDate: string;
  modelUsed: string;
  confidenceScore: number;
  generatedAt: string;
}

export interface ExecutiveInsightDto {
  id: string;
  organizationId: string;
  title: string;
  description: string;
  category: string;
  priority: string;
  businessImpact: number;
  confidenceLevel: number;
  suggestedAction: string;
  createdAt: string;
}

export interface BusinessGoalDto {
  id: string;
  organizationId: string;
  title: string;
  description: string;
  category: string;
  targetValue: number;
  currentValue: number;
  unit: string;
  deadline: string;
  status: string;
}

export interface ScorecardDto {
  id: string;
  organizationId: string;
  title: string;
  targetId: string;
  ownerId: string;
  score: number;
  evaluationPeriod: string;
  strengths: string;
  areasForImprovement: string;
  createdAt: string;
}

export interface BusinessHealthScoreDto {
  id: string;
  organizationId: string;
  overallScore: number;
  financialHealth: number;
  operationalHealth: number;
  customerHealth: number;
  calculatedAt: string;
  status: string;
}

export interface BenchmarkDto {
  id: string;
  organizationId: string;
  metricName: string;
  industryAverage: number;
  topQuartile: number;
  industry: string;
  validFrom: string;
  validTo: string;
  source: string;
}

export interface AiRecommendationDto {
  id: string;
  organizationId: string;
  title: string;
  description: string;
  category: string;
  estimatedImpact: number;
  confidenceLevel: number;
  suggestedAction: string;
  priority: string;
  isApplied: boolean;
  appliedAt: string | null;
  createdAt: string;
}

export interface DecisionLogDto {
  id: string;
  organizationId: string;
  decisionTitle: string;
  description: string;
  relatedInsightId?: string;
  decisionMakerId?: string;
  expectedOutcome: string;
  status: string;
  decisionDate: string;
  createdAt: string;
}

export const ExecutiveService = {
  // KPIs
  getKpis: async () => {
    const res = await apiClient.get<KpiDto[]>("/api/v1/kpi");
    return res.data;
  },
  getKpiHistory: async (kpiId: string, days = 30) => {
    const res = await apiClient.get<KpiHistoryDto[]>(`/api/v1/kpi/${kpiId}/history`, { params: { days } });
    return res.data;
  },

  // Forecasting
  getLatestForecast: async (metricName: string) => {
    const res = await apiClient.get<ForecastDto>(`/api/v1/forecasting/latest`, { params: { metricName } });
    return res.data;
  },
  generateForecast: async (metricName: string) => {
    const res = await apiClient.post<ForecastDto>(`/api/v1/forecasting/generate`, { metricName });
    return res.data;
  },

  // Executive Insights
  getInsights: async (limit = 10) => {
    const res = await apiClient.get<ExecutiveInsightDto[]>("/api/v1/executive-insights", { params: { limit } });
    return res.data;
  },

  // Business Goals
  getGoals: async () => {
    const res = await apiClient.get<BusinessGoalDto[]>("/api/v1/business-goals");
    return res.data;
  },
  
  // Scorecards
  getScorecards: async () => {
    const res = await apiClient.get<ScorecardDto[]>("/api/v1/scorecards");
    return res.data;
  },

  // Business Health
  getLatestHealth: async () => {
    const res = await apiClient.get<BusinessHealthScoreDto>("/api/v1/business-health/latest");
    return res.data;
  },
  getHealthHistory: async (limit = 12) => {
    const res = await apiClient.get<BusinessHealthScoreDto[]>("/api/v1/business-health/history", { params: { limit } });
    return res.data;
  },
  calculateHealth: async () => {
    const res = await apiClient.post<BusinessHealthScoreDto>("/api/v1/business-health/calculate");
    return res.data;
  },

  // Benchmarks
  getBenchmarks: async () => {
    const res = await apiClient.get<BenchmarkDto[]>("/api/v1/benchmarks");
    return res.data;
  },

  // AI Recommendations
  getRecommendations: async () => {
    const res = await apiClient.get<AiRecommendationDto[]>("/api/v1/ai-recommendations");
    return res.data;
  },
  getPendingRecommendations: async () => {
    const res = await apiClient.get<AiRecommendationDto[]>("/api/v1/ai-recommendations/pending");
    return res.data;
  },
  applyRecommendation: async (id: string) => {
    const res = await apiClient.post(`/api/v1/ai-recommendations/${id}/apply`);
    return res.data;
  },

  // Decision Center
  getDecisionLogs: async () => {
    const res = await apiClient.get<DecisionLogDto[]>("/api/v1/decision-center");
    return res.data;
  },
  createDecisionLog: async (data: Partial<DecisionLogDto>) => {
    const res = await apiClient.post<DecisionLogDto>("/api/v1/decision-center", data);
    return res.data;
  },
};
