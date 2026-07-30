import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { ExecutiveService } from "@/lib/executive-service";

export const executiveKeys = {
  all: ["executive"] as const,
  kpis: () => [...executiveKeys.all, "kpis"] as const,
  kpiHistory: (kpiId: string) => [...executiveKeys.kpis(), kpiId, "history"] as const,
  forecasts: () => [...executiveKeys.all, "forecasts"] as const,
  insights: () => [...executiveKeys.all, "insights"] as const,
  goals: () => [...executiveKeys.all, "goals"] as const,
  scorecards: () => [...executiveKeys.all, "scorecards"] as const,
  health: () => [...executiveKeys.all, "health"] as const,
  healthHistory: () => [...executiveKeys.health(), "history"] as const,
  benchmarks: () => [...executiveKeys.all, "benchmarks"] as const,
  recommendations: () => [...executiveKeys.all, "recommendations"] as const,
  decisionLogs: () => [...executiveKeys.all, "decisionLogs"] as const,
};

export function useKpis() {
  return useQuery({
    queryKey: executiveKeys.kpis(),
    queryFn: ExecutiveService.getKpis,
  });
}

export function useKpiHistory(kpiId: string, days = 30) {
  return useQuery({
    queryKey: executiveKeys.kpiHistory(kpiId),
    queryFn: () => ExecutiveService.getKpiHistory(kpiId, days),
    enabled: !!kpiId,
  });
}

export function useLatestForecast(metricName: string) {
  return useQuery({
    queryKey: [...executiveKeys.forecasts(), metricName],
    queryFn: () => ExecutiveService.getLatestForecast(metricName),
    enabled: !!metricName,
  });
}

export function useGenerateForecast() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ExecutiveService.generateForecast,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: executiveKeys.forecasts() });
    },
  });
}

export function useExecutiveInsights(limit = 10) {
  return useQuery({
    queryKey: [...executiveKeys.insights(), limit],
    queryFn: () => ExecutiveService.getInsights(limit),
  });
}

export function useBusinessGoals() {
  return useQuery({
    queryKey: executiveKeys.goals(),
    queryFn: ExecutiveService.getGoals,
  });
}

export function useScorecards() {
  return useQuery({
    queryKey: executiveKeys.scorecards(),
    queryFn: ExecutiveService.getScorecards,
  });
}

export function useBusinessHealth() {
  return useQuery({
    queryKey: executiveKeys.health(),
    queryFn: ExecutiveService.getLatestHealth,
  });
}

export function useBusinessHealthHistory(limit = 12) {
  return useQuery({
    queryKey: executiveKeys.healthHistory(),
    queryFn: () => ExecutiveService.getHealthHistory(limit),
  });
}

export function useCalculateBusinessHealth() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ExecutiveService.calculateHealth,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: executiveKeys.health() });
      queryClient.invalidateQueries({ queryKey: executiveKeys.healthHistory() });
    },
  });
}

export function useBenchmarks() {
  return useQuery({
    queryKey: executiveKeys.benchmarks(),
    queryFn: ExecutiveService.getBenchmarks,
  });
}

export function useAiRecommendations(pendingOnly = false) {
  return useQuery({
    queryKey: [...executiveKeys.recommendations(), { pendingOnly }],
    queryFn: pendingOnly ? ExecutiveService.getPendingRecommendations : ExecutiveService.getRecommendations,
  });
}

export function useApplyRecommendation() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ExecutiveService.applyRecommendation,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: executiveKeys.recommendations() });
    },
  });
}

export function useDecisionLogs() {
  return useQuery({
    queryKey: executiveKeys.decisionLogs(),
    queryFn: ExecutiveService.getDecisionLogs,
  });
}

export function useCreateDecisionLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ExecutiveService.createDecisionLog,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: executiveKeys.decisionLogs() });
    },
  });
}
