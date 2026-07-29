import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import apiClient from "@/lib/api-client";
import { toast } from "sonner";

export interface WebhookEndpoint {
  id: string;
  url: string;
  description?: string;
  isActive: boolean;
  secret: string;
  eventTypes: string[];
  createdAt: string;
  lastDeliveryAt?: string;
  lastDeliveryStatus?: string;
}

export interface WebhookDeliveryLog {
  id: string;
  webhookEndpointId: string;
  eventId: string;
  eventType: string;
  deliveryAttempt: number;
  statusCode?: number;
  status: string;
  errorMessage?: string;
  requestPayload: string;
  responseHeaders?: string;
  responseBody?: string;
  deliveredAt: string;
  durationMs: number;
}

export const useWebhooks = () => {
  return useQuery({
    queryKey: ["webhooks"],
    queryFn: async () => {
      const response = await apiClient.get<WebhookEndpoint[]>("/api/webhooks");
      return response.data;
    },
  });
};

export const useCreateWebhook = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: Partial<WebhookEndpoint>) => {
      const response = await apiClient.post<WebhookEndpoint>("/api/webhooks", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["webhooks"] });
      toast.success("Webhook created successfully.");
    },
    onError: () => {
      toast.error("Failed to create webhook.");
    },
  });
};

export const useUpdateWebhook = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: Partial<WebhookEndpoint> }) => {
      const response = await apiClient.put<WebhookEndpoint>(`/api/webhooks/${id}`, data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["webhooks"] });
      toast.success("Webhook updated successfully.");
    },
    onError: () => {
      toast.error("Failed to update webhook.");
    },
  });
};

export const useDeleteWebhook = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/webhooks/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["webhooks"] });
      toast.success("Webhook deleted successfully.");
    },
    onError: () => {
      toast.error("Failed to delete webhook.");
    },
  });
};

export const useWebhookLogs = (endpointId: string) => {
  return useQuery({
    queryKey: ["webhooks", endpointId, "logs"],
    queryFn: async () => {
      const response = await apiClient.get<WebhookDeliveryLog[]>(`/api/webhooks/${endpointId}/logs`);
      return response.data;
    },
    enabled: !!endpointId,
  });
};

export const useTestWebhook = () => {
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.post(`/api/webhooks/${id}/test`);
    },
    onSuccess: () => {
      toast.success("Test event sent.");
    },
    onError: () => {
      toast.error("Failed to send test event.");
    },
  });
};
