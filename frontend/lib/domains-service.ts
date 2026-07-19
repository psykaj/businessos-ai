import apiClient from "./api-client";

export interface CustomDomainDto {
  id: string;
  organizationId: string;
  domain: string;
  verificationStatus: string;
  sslStatus: string;
  isPrimary: boolean;
  createdAt: string;
}

export interface CreateCustomDomainDto {
  domain: string;
}

export const DomainsService = {
  getDomains: async (): Promise<CustomDomainDto[]> => {
    const response = await apiClient.get<CustomDomainDto[]>("/api/domains");
    return response.data;
  },

  addDomain: async (dto: CreateCustomDomainDto): Promise<CustomDomainDto> => {
    const response = await apiClient.post<CustomDomainDto>("/api/domains", dto);
    return response.data;
  },

  setPrimary: async (id: string): Promise<CustomDomainDto> => {
    const response = await apiClient.put<CustomDomainDto>(`/api/domains/${id}`);
    return response.data;
  },

  deleteDomain: async (id: string): Promise<void> => {
    await apiClient.delete(`/api/domains/${id}`);
  },

  verifyDomain: async (id: string): Promise<{ verified: boolean }> => {
    const response = await apiClient.post<{ verified: boolean }>(`/api/domains/${id}/verify`);
    return response.data;
  },

  getDnsInstructions: async (id: string): Promise<{ domain: string; instructions: string; recordType: string; name: string; value: string }> => {
    const response = await apiClient.get(`/api/domains/${id}/dns`);
    return response.data;
  },
};
