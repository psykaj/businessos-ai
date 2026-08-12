import apiClient from "./api-client";

export interface BusinessOutcome {
  id: string;
  businessId: string;
  outcomeType: string;
  sourceType: string;
  sourceId: string;
  relatedEntityType?: string;
  relatedEntityId?: string;
  beforeValue?: number;
  afterValue?: number;
  changeValue?: number;
  changePercentage?: number;
  currency?: string;
  timeSavedMinutes?: number;
  revenueImpact?: number;
  costImpact?: number;
  customerImpact?: number;
  confidence: "High" | "Medium" | "Low" | "Unknown";
  attributionLevel: "Direct" | "Strong" | "Moderate" | "Weak" | "Unknown";
  measurementMethod?: string;
  occurredAt: string;
  recordedAt: string;
  status: "Active" | "Voided" | "Archived" | "Draft";
  metadata?: string;
  explanation?: string;
}

export interface RoiSummary {
  revenueGenerated: number;
  revenueRecovered: number;
  costSaved: number;
  timeSavedMinutes: number;
  estimatedTimeValue: number;
  customersRetained: number;
  customersConverted: number;
  leadsConverted: number;
  successfulActions: number;
  successfulAutomations: number;
  confidenceSummary: Record<string, number>;
  startDate: string;
  endDate: string;
}

export interface CreateOutcomeDto {
  outcomeType: string;
  sourceType: string;
  sourceId: string;
  relatedEntityType?: string;
  relatedEntityId?: string;
  beforeValue?: number;
  afterValue?: number;
  changeValue?: number;
  changePercentage?: number;
  currency?: string;
  timeSavedMinutes?: number;
  revenueImpact?: number;
  costImpact?: number;
  customerImpact?: number;
  confidence: string;
  measurementMethod?: string;
  metadata?: string;
  explanation?: string;
  occurredAt?: string;
}

export const outcomesService = {
  getOutcomes: async (params?: {
    sourceType?: string;
    sourceId?: string;
    page?: number;
    pageSize?: number;
  }): Promise<BusinessOutcome[]> => {
    const { data } = await apiClient.get("/api/outcomes", { params });
    return data;
  },

  getRoiSummary: async (params?: {
    startDate?: string;
    endDate?: string;
    estimatedHourlyCost?: number;
  }): Promise<RoiSummary> => {
    const { data } = await apiClient.get("/api/outcomes/roi-summary", { params });
    return data;
  },

  createOutcome: async (payload: CreateOutcomeDto): Promise<BusinessOutcome> => {
    const { data } = await apiClient.post("/api/outcomes", payload);
    return data;
  },
};
