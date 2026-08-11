import { useQuery, useMutation } from "@tanstack/react-query";
import apiClient from "./api-client";

// --- DTOs ---

export interface CommandMetricDto {
  id: string;
  name: string;
  value: number;
  Formattedvalue: string;
  Previousvalue: number;
  trendPercentage: number;
  trendDirection: "Up" | "Down" | "Flat";
  category: string;
}

export interface ProactiveAlertDto {
  id: string;
  title: string;
  description: string;
  category: string; // Finance, Inventory, CRM, System
  priority: string; // High, Medium, Low
  recommendedAction: string;
  SourceEntityId?: string;
  createdAt: string;
  isRead: boolean;
}

export interface CommandOpportunityDto {
  id: string;
  title: string;
  description: string;
  Potentialvalue: number;
  category: string; // Upsell, Recovery, Optimization
  recommendedAction: string;
}

export interface AiActionDto {
  id: string;
  title: string;
  description: string;
  priority: "High" | "Medium" | "Low";
  actionType: string;
  SourceEntityid: string;
  createdAt: string;
}

export interface CommandAutomationSummaryDto {
  activeWorkflowsCount: number;
  actionsExecutedToday: number;
  failedExecutionsToday: number;
  timeSavedHours: number;
}

export interface CommandActivityDto {
  id: string;
  activityType: string;
  description: string;
  timestamp: string;
  sourceModule: string;
}

export interface CommandTrendDto {
  id: string;
  Metricname: string;
  dataPoints: { date: string; value: number }[];
  trendAnalysis: string;
}

export interface BusinessBriefingDto {
  id: string;
  Organizationid: string;
  date: string;
  executiveSummary: string;
  keyHighlights: string[];
  risksAndBlockers: string[];
  recommendedActions: string[];
  generatedAt: string;
}

export interface BusinessHealthDto {
  healthScore: number;
  status: "Healthy" | "AtRisk" | "Critical";
  trend: "Improving" | "Declining" | "Stable";
  explanation: string;
}

export interface CommandCenterSummaryDto {
  businessHealth: BusinessHealthDto;
  executiveSummary: BusinessBriefingDto;
  metrics: CommandMetricDto[];
  priorityAlerts: ProactiveAlertDto[];
  opportunities: CommandOpportunityDto[];
  recommendedActions: AiActionDto[];
  automationSummary: CommandAutomationSummaryDto;
  recentActivity: CommandActivityDto[];
}

export interface SuggestedActionDto {
  description: string;
  actionType: string;
  ActionCenterActionId?: string;
}

export interface CommandCenterAskRequestDto {
  question: string;
  OptionalContext?: string;
}

export interface CommandCenterAskResponseDto {
  answer: string;
  confidence: string;
  suggestedActions: SuggestedActionDto[];
  relatedDataAvailable: boolean;
}

// --- API Service Calls ---

export const commandCenterService = {
  async getSummary(): Promise<CommandCenterSummaryDto> {
    const response = await apiClient.get<CommandCenterSummaryDto>("/api/command-center");
    return response.data;
  },

  async askAi(request: CommandCenterAskRequestDto): Promise<CommandCenterAskResponseDto> {
    const response = await apiClient.post<CommandCenterAskResponseDto>("/api/command-center/ask", request);
    return response.data;
  },
};

// --- React Query Hooks ---

export function useCommandCenterSummary() {
  return useQuery({
    queryKey: ["commandCenterSummary"],
    queryFn: () => commandCenterService.getSummary(),
    refetchInterval: 5 * 60 * 1000, // Refresh every 5 minutes automatically
  });
}

export function useAskBusinessOS() {
  return useMutation({
    mutationFn: (request: CommandCenterAskRequestDto) => commandCenterService.askAi(request),
  });
}
