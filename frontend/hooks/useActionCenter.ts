import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import apiClient from '@/lib/api-client';
import { AiActionDto } from '@/types/action-center';

const ACTION_CENTER_QUERY_KEYS = {
  all: ['action-center'] as const,
  actions: () => [...ACTION_CENTER_QUERY_KEYS.all, 'actions'] as const,
  pending: () => [...ACTION_CENTER_QUERY_KEYS.all, 'pending'] as const,
  history: () => [...ACTION_CENTER_QUERY_KEYS.all, 'history'] as const,
};

// --- Queries ---

export function useGetActions() {
  return useQuery({
    queryKey: ACTION_CENTER_QUERY_KEYS.actions(),
    queryFn: async () => {
      const response = await apiClient.get<AiActionDto[]>('/api/action-center/actions');
      return response.data;
    },
  });
}

export function useGetPendingActions() {
  return useQuery({
    queryKey: ACTION_CENTER_QUERY_KEYS.pending(),
    queryFn: async () => {
      const response = await apiClient.get<AiActionDto[]>('/api/action-center/pending');
      return response.data;
    },
  });
}

export function useGetActionHistory() {
  return useQuery({
    queryKey: ACTION_CENTER_QUERY_KEYS.history(),
    queryFn: async () => {
      const response = await apiClient.get<AiActionDto[]>('/api/action-center/history');
      return response.data;
    },
  });
}

// --- Mutations ---

export function useExecuteActionMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (actionId: string) => {
      const response = await apiClient.post(`/api/action-center/execute/${actionId}`);
      return response.data;
    },
    onSuccess: () => {
      // Invalidate queries to refresh data
      queryClient.invalidateQueries({ queryKey: ACTION_CENTER_QUERY_KEYS.all });
    },
  });
}

export function useApproveActionMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (actionId: string) => {
      const response = await apiClient.post(`/api/action-center/approve/${actionId}`);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ACTION_CENTER_QUERY_KEYS.all });
    },
  });
}

export function useRejectActionMutation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (actionId: string) => {
      const response = await apiClient.post(`/api/action-center/reject/${actionId}`);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ACTION_CENTER_QUERY_KEYS.all });
    },
  });
}
