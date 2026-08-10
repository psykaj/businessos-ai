import { useQuery, useMutation } from "@tanstack/react-query";
import apiClient from "./api-client";

// --- DTOs ---

export interface CommandMetricDto {
  Id: string;
  Name: string;
  Value: number;
  FormattedValue: string;
  PreviousValue: number;
  TrendPercentage: number;
  TrendDirection: "Up" | "Down" | "Flat";
  Category: string;
}

export interface ProactiveAlertDto {
  Id: string;
  Title: string;
  Description: string;
  Category: string; // Finance, Inventory, CRM, System
  Priority: string; // High, Medium, Low
  RecommendedAction: string;
  SourceEntityId?: string;
  CreatedAt: string;
  IsRead: boolean;
}

export interface CommandOpportunityDto {
  Id: string;
  Title: string;
  Description: string;
  PotentialValue: number;
  Category: string; // Upsell, Recovery, Optimization
  RecommendedAction: string;
}

export interface AiActionDto {
  Id: string;
  Title: string;
  Description: string;
  Priority: "High" | "Medium" | "Low";
  ActionType: string;
  SourceEntityId: string;
  CreatedAt: string;
}

export interface CommandAutomationSummaryDto {
  ActiveWorkflowsCount: number;
  ActionsExecutedToday: number;
  FailedExecutionsToday: number;
  TimeSavedHours: number;
}

export interface CommandActivityDto {
  Id: string;
  ActivityType: string;
  Description: string;
  Timestamp: string;
  SourceModule: string;
}

export interface CommandTrendDto {
  Id: string;
  MetricName: string;
  DataPoints: { Date: string; Value: number }[];
  TrendAnalysis: string;
}

export interface BusinessBriefingDto {
  Id: string;
  OrganizationId: string;
  Date: string;
  ExecutiveSummary: string;
  KeyHighlights: string[];
  RisksAndBlockers: string[];
  RecommendedActions: string[];
  GeneratedAt: string;
}

export interface BusinessHealthDto {
  HealthScore: number;
  Status: "Healthy" | "AtRisk" | "Critical";
  Trend: "Improving" | "Declining" | "Stable";
  Explanation: string;
}

export interface CommandCenterSummaryDto {
  BusinessHealth: BusinessHealthDto;
  ExecutiveSummary: BusinessBriefingDto;
  Metrics: CommandMetricDto[];
  PriorityAlerts: ProactiveAlertDto[];
  Opportunities: CommandOpportunityDto[];
  RecommendedActions: AiActionDto[];
  AutomationSummary: CommandAutomationSummaryDto;
  RecentActivity: CommandActivityDto[];
}

export interface SuggestedActionDto {
  Description: string;
  ActionType: string;
  ActionCenterActionId?: string;
}

export interface CommandCenterAskRequestDto {
  Question: string;
  OptionalContext?: string;
}

export interface CommandCenterAskResponseDto {
  Answer: string;
  Confidence: string;
  SuggestedActions: SuggestedActionDto[];
  RelatedDataAvailable: boolean;
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
