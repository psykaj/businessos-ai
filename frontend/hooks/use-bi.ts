import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { biService, DashboardFilter, CreateGoalPayload, UpdateGoalPayload, GenerateReportPayload } from "@/lib/bi-service";
import { toast } from "sonner";

export const BI_QUERY_KEYS = {
  kpis: ["bi", "kpis"],
  kpiByName: (name: string) => ["bi", "kpis", name],
  ceoDashboard: (filter?: DashboardFilter) => ["bi", "dashboard", "ceo", filter],
  salesDashboard: (filter?: DashboardFilter) => ["bi", "dashboard", "sales", filter],
  marketingDashboard: (filter?: DashboardFilter) => ["bi", "dashboard", "marketing", filter],
  financeDashboard: (filter?: DashboardFilter) => ["bi", "dashboard", "finance", filter],
  operationsDashboard: (filter?: DashboardFilter) => ["bi", "dashboard", "operations", filter],
  teamDashboard: (filter?: DashboardFilter) => ["bi", "dashboard", "team", filter],
  insights: (category?: string, priority?: string) => ["bi", "insights", category, priority],
  forecast: (type: string) => ["bi", "forecast", type],
  reports: (reportType?: string) => ["bi", "reports", reportType],
  reportById: (id: string) => ["bi", "reports", id],
  goals: (status?: string) => ["bi", "goals", status],
};

export function useKPIs(category?: string) {
  return useQuery({
    queryKey: [...BI_QUERY_KEYS.kpis, category],
    queryFn: () => biService.getKPIs(category),
    staleTime: 1000 * 60 * 5, // 5 mins
  });
}

export function useRecalculateKPIs() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: () => biService.recalculateKPIs(),
    onSuccess: (data) => {
      toast.success(`Successfully recalculated ${data.updatedCount} KPIs`);
      queryClient.invalidateQueries({ queryKey: BI_QUERY_KEYS.kpis });
    },
    onError: (err: any) => {
      toast.error("Failed to recalculate KPIs", { description: err.message });
    }
  });
}

export function useCEODashboard(filter?: DashboardFilter) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.ceoDashboard(filter),
    queryFn: () => biService.getCEODashboard(filter),
    staleTime: 1000 * 60 * 2,
  });
}

export function useSalesDashboard(filter?: DashboardFilter) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.salesDashboard(filter),
    queryFn: () => biService.getSalesDashboard(filter),
    staleTime: 1000 * 60 * 2,
  });
}

export function useMarketingDashboard(filter?: DashboardFilter) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.marketingDashboard(filter),
    queryFn: () => biService.getMarketingDashboard(filter),
    staleTime: 1000 * 60 * 2,
  });
}

export function useFinanceDashboard(filter?: DashboardFilter) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.financeDashboard(filter),
    queryFn: () => biService.getFinanceDashboard(filter),
    staleTime: 1000 * 60 * 2,
  });
}

export function useOperationsDashboard(filter?: DashboardFilter) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.operationsDashboard(filter),
    queryFn: () => biService.getOperationsDashboard(filter),
    staleTime: 1000 * 60 * 2,
  });
}

export function useTeamDashboard(filter?: DashboardFilter) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.teamDashboard(filter),
    queryFn: () => biService.getTeamDashboard(filter),
    staleTime: 1000 * 60 * 2,
  });
}

export function useInsights(category?: string, priority?: string) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.insights(category, priority),
    queryFn: () => biService.getInsights(category, priority),
    staleTime: 1000 * 60 * 5,
  });
}

export function useGenerateInsights() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: () => biService.generateInsights(),
    onSuccess: (data) => {
      toast.success(`Generated ${data.generatedCount} AI business insights`);
      queryClient.invalidateQueries({ queryKey: ["bi", "insights"] });
    },
    onError: (err: any) => {
      toast.error("Failed to generate AI insights", { description: err.message });
    }
  });
}

export function useForecast(type: string) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.forecast(type),
    queryFn: () => biService.getForecast(type),
    staleTime: 1000 * 60 * 10,
  });
}

export function useGenerateForecast() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ forecastType, horizonDays }: { forecastType: string; horizonDays?: number }) =>
      biService.generateForecast(forecastType, horizonDays),
    onSuccess: (data) => {
      toast.success(`Generated ${data.forecastType} forecast`);
      queryClient.invalidateQueries({ queryKey: BI_QUERY_KEYS.forecast(data.forecastType) });
    },
    onError: (err: any) => {
      toast.error("Failed to generate forecast", { description: err.message });
    }
  });
}

export function useReports(reportType?: string) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.reports(reportType),
    queryFn: () => biService.getReports(reportType),
    staleTime: 1000 * 60 * 2,
  });
}

export function useGenerateReport() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: GenerateReportPayload) => biService.generateReport(payload),
    onSuccess: (data) => {
      toast.success(`Report "${data.name}" generated successfully`);
      queryClient.invalidateQueries({ queryKey: ["bi", "reports"] });
    },
    onError: (err: any) => {
      toast.error("Failed to generate report", { description: err.message });
    }
  });
}

export function useGoals(status?: string) {
  return useQuery({
    queryKey: BI_QUERY_KEYS.goals(status),
    queryFn: () => biService.getGoals(status),
    staleTime: 1000 * 60 * 2,
  });
}

export function useCreateGoal() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: CreateGoalPayload) => biService.createGoal(payload),
    onSuccess: (data) => {
      toast.success(`Goal "${data.name}" created successfully`);
      queryClient.invalidateQueries({ queryKey: ["bi", "goals"] });
    },
    onError: (err: any) => {
      toast.error("Failed to create goal", { description: err.message });
    }
  });
}

export function useUpdateGoal() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, payload }: { id: string; payload: UpdateGoalPayload }) => biService.updateGoal(id, payload),
    onSuccess: (data) => {
      toast.success(`Goal "${data.name}" updated successfully`);
      queryClient.invalidateQueries({ queryKey: ["bi", "goals"] });
    },
    onError: (err: any) => {
      toast.error("Failed to update goal", { description: err.message });
    }
  });
}

export function useDeleteGoal() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => biService.deleteGoal(id),
    onSuccess: () => {
      toast.success("Goal deleted successfully");
      queryClient.invalidateQueries({ queryKey: ["bi", "goals"] });
    },
    onError: (err: any) => {
      toast.error("Failed to delete goal", { description: err.message });
    }
  });
}

export function useSyncGoals() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: () => biService.syncGoals(),
    onSuccess: () => {
      toast.success("Synchronized goals with live KPI metrics");
      queryClient.invalidateQueries({ queryKey: ["bi", "goals"] });
    },
    onError: (err: any) => {
      toast.error("Failed to sync goals", { description: err.message });
    }
  });
}
