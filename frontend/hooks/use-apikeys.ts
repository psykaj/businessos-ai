import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apikeysService } from "@/lib/apikeys-service";
import { CreateApiKeyRequest } from "@/types/apikeys";

export function useApiKeys(orgId: string | undefined) {
  return useQuery({
    queryKey: ["apiKeys", orgId],
    queryFn: () => apikeysService.getApiKeys(orgId!),
    enabled: !!orgId,
  });
}

export function useCreateApiKey() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, data }: { orgId: string; data: CreateApiKeyRequest }) =>
      apikeysService.createApiKey(orgId, data),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["apiKeys", orgId] });
    },
  });
}

export function useRevokeApiKey() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, keyId }: { orgId: string; keyId: string }) =>
      apikeysService.revokeApiKey(orgId, keyId),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["apiKeys", orgId] });
    },
  });
}

export function useRotateApiKey() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, keyId }: { orgId: string; keyId: string }) =>
      apikeysService.rotateApiKey(orgId, keyId),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["apiKeys", orgId] });
    },
  });
}
