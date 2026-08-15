import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import apiClient from "@/lib/api-client";
import { toast } from "sonner";

// Types
export interface DailyPriorityDto {
  id: string;
  organizationId: string;
  briefingId: string;
  priorityType: string;
  title: string;
  description: string;
  reason: string;
  severity: string;
  priorityScore: number;
  relatedEntityType: string;
  relatedEntityId: string;
  relatedGoalId?: string;
  relatedKpiId?: string;
  suggestedAction: string;
  expectedImpact: string;
  impactType: string;
  confidence: number;
  status: string;
  dueAt?: string;
  completedAt?: string;
}

export interface DailyBusinessBriefingDto {
  id: string;
  organizationId: string;
  briefingDate: string;
  summary: string;
  businessHealth: "Healthy" | "NeedsAttention" | "AtRisk" | "Critical" | string;
  priorityCount: number;
  opportunityCount: number;
  riskCount: number;
  completedPriorityCount: number;
  generatedAt: string;
  expiresAt: string;
  status: string;
  priorities: DailyPriorityDto[];
}

// API Functions
const getTodayBriefing = async (): Promise<DailyBusinessBriefingDto> => {
  const response = await apiClient.get<DailyBusinessBriefingDto>("/api/daily-briefing/today");
  return response.data;
};

const generateBriefing = async (force: boolean = false): Promise<DailyBusinessBriefingDto> => {
  const response = await apiClient.post<DailyBusinessBriefingDto>(`/api/daily-briefing/generate?forceRegeneration=${force}`);
  return response.data;
};

const refreshBriefing = async (id: string): Promise<DailyBusinessBriefingDto> => {
  const response = await apiClient.post<DailyBusinessBriefingDto>(`/api/daily-briefing/${id}/refresh`);
  return response.data;
};

const completePriority = async (id: string): Promise<void> => {
  await apiClient.post(`/api/daily-priorities/${id}/complete`);
};

const dismissPriority = async (id: string): Promise<void> => {
  await apiClient.post(`/api/daily-priorities/${id}/dismiss`);
};

const snoozePriority = async ({ id, snoozeUntil }: { id: string; snoozeUntil?: string }): Promise<void> => {
  await apiClient.post(`/api/daily-priorities/${id}/snooze`, { snoozeUntil });
};

// React Query Hooks
export const useTodayBriefing = () => {
  return useQuery({
    queryKey: ["daily-briefing", "today"],
    queryFn: getTodayBriefing,
    retry: false, // If 404, we want to know immediately to trigger generation
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};

export const useGenerateBriefing = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (force: boolean = false) => generateBriefing(force),
    onSuccess: (data) => {
      queryClient.setQueryData(["daily-briefing", "today"], data);
      toast.success("Daily operating loop updated");
    },
    onError: () => {
      toast.error("Failed to generate daily briefing");
    },
  });
};

export const useRefreshBriefing = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: refreshBriefing,
    onSuccess: (data) => {
      queryClient.setQueryData(["daily-briefing", "today"], data);
      toast.success("Refreshed successfully");
    },
    onError: () => {
      toast.error("Failed to refresh");
    },
  });
};

export const useCompletePriority = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: completePriority,
    onSuccess: (_, id) => {
      // Optimistically update the cache
      queryClient.setQueryData<DailyBusinessBriefingDto | undefined>(["daily-briefing", "today"], (oldData) => {
        if (!oldData) return undefined;
        return {
          ...oldData,
          completedPriorityCount: oldData.completedPriorityCount + 1,
          priorities: oldData.priorities.filter(p => p.id !== id),
        };
      });
      toast.success("Action completed successfully");
    },
    onError: () => {
      toast.error("Failed to complete action");
    },
  });
};

export const useDismissPriority = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: dismissPriority,
    onSuccess: (_, id) => {
      // Optimistically update the cache
      queryClient.setQueryData<DailyBusinessBriefingDto | undefined>(["daily-briefing", "today"], (oldData) => {
        if (!oldData) return undefined;
        return {
          ...oldData,
          priorities: oldData.priorities.filter(p => p.id !== id),
        };
      });
      toast.info("Priority dismissed");
    },
    onError: () => {
      toast.error("Failed to dismiss priority");
    },
  });
};

export const useSnoozePriority = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: snoozePriority,
    onSuccess: (_, variables) => {
      // Optimistically update the cache
      queryClient.setQueryData<DailyBusinessBriefingDto | undefined>(["daily-briefing", "today"], (oldData) => {
        if (!oldData) return undefined;
        return {
          ...oldData,
          priorities: oldData.priorities.filter(p => p.id !== variables.id),
        };
      });
      toast.success("Priority snoozed");
    },
    onError: () => {
      toast.error("Failed to snooze priority");
    },
  });
};
