import apiClient from "./api-client";
import { Organization, UpdateOrganizationRequest } from "@/types/organization";

export const organizationService = {
  getOrganization: async (id: string): Promise<Organization> => {
    const response = await apiClient.get<Organization>(`/api/v1/organizations/${id}`);
    return response.data;
  },

  updateOrganization: async (id: string, data: UpdateOrganizationRequest): Promise<Organization> => {
    const response = await apiClient.put<Organization>(`/api/v1/organizations/${id}`, data);
    return response.data;
  },

  uploadLogo: async (id: string, file: File): Promise<{ logoUrl: string }> => {
    const formData = new FormData();
    formData.append("file", file);
    const response = await apiClient.post<{ logoUrl: string }>(`/api/v1/organizations/${id}/logo`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    return response.data;
  },
};
