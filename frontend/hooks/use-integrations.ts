import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { workflowApi, CreateIntegrationPayload, UpdateIntegrationPayload } from "@/lib/api/workflows";
import { toast } from "sonner";

export function useIntegrations() {
  return useQuery({
    queryKey: ["integrations"],
    queryFn: () => workflowApi.getIntegrations(),
  });
}

export function useIntegration(id: string) {
  return useQuery({
    queryKey: ["integration", id],
    queryFn: () => workflowApi.getIntegrationById(id),
    enabled: !!id,
  });
}

export function useCreateIntegration() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: CreateIntegrationPayload) => workflowApi.createIntegration(payload),
    onSuccess: () => {
      toast.success("Integration connected successfully");
      queryClient.invalidateQueries({ queryKey: ["integrations"] });
    },
    onError: (err: any) => {
      toast.error(err?.response?.data?.message || "Failed to connect integration");
    },
  });
}

export function useUpdateIntegration() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, payload }: { id: string; payload: UpdateIntegrationPayload }) =>
      workflowApi.updateIntegration(id, payload),
    onSuccess: (_, variables) => {
      toast.success("Integration updated");
      queryClient.invalidateQueries({ queryKey: ["integrations"] });
      queryClient.invalidateQueries({ queryKey: ["integration", variables.id] });
    },
    onError: (err: any) => {
      toast.error(err?.response?.data?.message || "Failed to update integration");
    },
  });
}

export function useDeleteIntegration() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => workflowApi.deleteIntegration(id),
    onSuccess: () => {
      toast.success("Integration disconnected");
      queryClient.invalidateQueries({ queryKey: ["integrations"] });
    },
    onError: (err: any) => {
      toast.error(err?.response?.data?.message || "Failed to disconnect integration");
    },
  });
}

export function useTestIntegration() {
  return useMutation({
    mutationFn: (id: string) => workflowApi.testIntegration(id),
    onSuccess: (res) => {
      if (res.success) {
        toast.success(res.message);
      } else {
        toast.error(res.message);
      }
    },
    onError: (err: any) => {
      toast.error(err?.response?.data?.message || "Test connection failed");
    },
  });
}
