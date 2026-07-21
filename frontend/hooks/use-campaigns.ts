import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import api from "@/lib/api-client";

export interface Campaign {
  id: string;
  name: string;
  status: string;
  startDate: string;
  endDate: string;
  campaignType: string;
  source: string;
  medium: string;
  budget: number;
  createdAt: string;
}

export function useCampaigns() {
  return useQuery({
    queryKey: ["campaigns"],
    queryFn: async () => {
      const { data } = await api.get<Campaign[]>("/campaigns");
      return data;
    },
  });
}

export function useCampaign(id: string) {
  return useQuery({
    queryKey: ["campaigns", id],
    queryFn: async () => {
      const { data } = await api.get<Campaign>(`/campaigns/${id}`);
      return data;
    },
    enabled: !!id,
  });
}

export function useCreateCampaign() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (payload: Partial<Campaign>) => {
      const { data } = await api.post<Campaign>("/campaigns", payload);
      return data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["campaigns"] });
    },
  });
}

export function useUpdateCampaign() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, payload }: { id: string; payload: Partial<Campaign> }) => {
      const { data } = await api.put<Campaign>(`/campaigns/${id}`, payload);
      return data;
    },
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ["campaigns"] });
      queryClient.invalidateQueries({ queryKey: ["campaigns", variables.id] });
    },
  });
}

export function useDeleteCampaign() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await api.delete(`/campaigns/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["campaigns"] });
    },
  });
}
