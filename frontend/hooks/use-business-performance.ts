import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  BusinessPerformanceService,
} from "@/lib/business-performance-service";
import { toast } from "sonner";

export const businessPerformanceKeys = {
  all: ["business-performance"] as const,
  health: () => [...businessPerformanceKeys.all, "health"] as const,
  revenue: () => [...businessPerformanceKeys.all, "revenue"] as const,
  products: () => [...businessPerformanceKeys.all, "products"] as const,
  customers: () => [...businessPerformanceKeys.all, "customers"] as const,
  marketing: () => [...businessPerformanceKeys.all, "marketing"] as const,
  recommendations: () => [...businessPerformanceKeys.all, "recommendations"] as const,
  benchmarks: () => [...businessPerformanceKeys.all, "benchmarks"] as const,
};

// ==========================================
// Queries
// ==========================================

export function useBusinessHealthSummary() {
  return useQuery({
    queryKey: businessPerformanceKeys.health(),
    queryFn: () => BusinessPerformanceService.getHealthSummary(),
    staleTime: 30_000,
  });
}

export function useRevenueAnalytics() {
  return useQuery({
    queryKey: businessPerformanceKeys.revenue(),
    queryFn: () => BusinessPerformanceService.getRevenueAnalytics(),
    staleTime: 30_000,
  });
}

export function useProductAnalytics() {
  return useQuery({
    queryKey: businessPerformanceKeys.products(),
    queryFn: () => BusinessPerformanceService.getProductAnalytics(),
    staleTime: 30_000,
  });
}

export function useCustomerAnalytics() {
  return useQuery({
    queryKey: businessPerformanceKeys.customers(),
    queryFn: () => BusinessPerformanceService.getCustomerAnalytics(),
    staleTime: 30_000,
  });
}

export function useMarketingRoi() {
  return useQuery({
    queryKey: businessPerformanceKeys.marketing(),
    queryFn: () => BusinessPerformanceService.getMarketingRoi(),
    staleTime: 30_000,
  });
}

export function useGrowthRecommendations() {
  return useQuery({
    queryKey: businessPerformanceKeys.recommendations(),
    queryFn: () => BusinessPerformanceService.getGrowthRecommendations(),
    staleTime: 15_000,
  });
}

export function useIndustryBenchmarks() {
  return useQuery({
    queryKey: businessPerformanceKeys.benchmarks(),
    queryFn: () => BusinessPerformanceService.getBenchmarks(),
    staleTime: 60_000,
  });
}

// ==========================================
// Action Mutations (Business Value Rule Engine)
// ==========================================

export function useExecuteGrowthAction() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, actionType, estimatedImpact }: { id: string; actionType: string; estimatedImpact?: number }) =>
      BusinessPerformanceService.executeGrowthAction(id, actionType),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: businessPerformanceKeys.all });
      const formattedImpact = variables.estimatedImpact ? ` (+$${variables.estimatedImpact.toLocaleString()}/yr ARR impact captured!)` : "";
      toast.success(`Action executed successfully!${formattedImpact}`, {
        description: `Automated workflow triggered for: ${variables.actionType}.`,
        duration: 5000,
      });
    },
    onError: () => {
      toast.error("Failed to execute growth action. Please verify network or credentials.");
    },
  });
}

export function useReallocateMarketingBudget() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ channelId, channelName, shiftAmount, estimatedLift }: { channelId: string; channelName: string; shiftAmount: number; estimatedLift?: number }) =>
      BusinessPerformanceService.reallocateMarketingBudget(channelId, shiftAmount),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: businessPerformanceKeys.marketing() });
      queryClient.invalidateQueries({ queryKey: businessPerformanceKeys.health() });
      const actionDesc = variables.shiftAmount > 0 ? `+$${variables.shiftAmount.toLocaleString()} added to` : `$${Math.abs(variables.shiftAmount).toLocaleString()} removed from`;
      toast.success(`Budget optimized for ${variables.channelName}!`, {
        description: `${actionDesc} channel budget. Projected ARR Lift: +$${(variables.estimatedLift ?? 15000).toLocaleString()}.`,
        duration: 5000,
      });
    },
    onError: () => {
      toast.error("Failed to reallocate campaign budget.");
    },
  });
}

export function useOptimizeStockOrPricing() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ productId, productName, actionName, impact }: { productId: string; productName: string; actionName: string; impact?: number }) =>
      BusinessPerformanceService.optimizeStockOrPricing(productId, actionName),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: businessPerformanceKeys.products() });
      queryClient.invalidateQueries({ queryKey: businessPerformanceKeys.health() });
      toast.success(`Product strategy updated for ${variables.productName}!`, {
        description: `${variables.actionName}. Estimated business value: +$${(variables.impact ?? 20000).toLocaleString()}/yr.`,
        duration: 5000,
      });
    },
    onError: () => {
      toast.error("Failed to apply product optimization strategy.");
    },
  });
}
