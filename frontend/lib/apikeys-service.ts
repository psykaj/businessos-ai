import apiClient from "./api-client";
import { ApiKey, CreateApiKeyRequest, CreateApiKeyResponse } from "@/types/apikeys";

export const apikeysService = {
  getApiKeys: async (orgId: string): Promise<ApiKey[]> => {
    const response = await apiClient.get<ApiKey[]>(`/api/v1/organizations/${orgId}/apikeys`);
    return response.data;
  },

  createApiKey: async (orgId: string, data: CreateApiKeyRequest): Promise<CreateApiKeyResponse> => {
    const response = await apiClient.post<CreateApiKeyResponse>(`/api/v1/organizations/${orgId}/apikeys`, data);
    return response.data;
  },

  revokeApiKey: async (orgId: string, keyId: string): Promise<void> => {
    await apiClient.delete(`/api/v1/organizations/${orgId}/apikeys/${keyId}`);
  },

  rotateApiKey: async (orgId: string, keyId: string): Promise<CreateApiKeyResponse> => {
    const response = await apiClient.post<CreateApiKeyResponse>(`/api/v1/organizations/${orgId}/apikeys/${keyId}/rotate`);
    return response.data;
  },
};
