import apiClient from "./api-client";

export interface BrandDto {
  id: string;
  organizationId: string;
  companyName: string;
  logoUrl?: string;
  faviconUrl?: string;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
  fontFamily: string;
  footerText?: string;
  supportEmail?: string;
  createdAt: string;
  updatedAt: string;
}

export interface UpdateBrandDto {
  companyName?: string;
  primaryColor?: string;
  secondaryColor?: string;
  accentColor?: string;
  fontFamily?: string;
  footerText?: string;
  supportEmail?: string;
}

export const BrandingService = {
  getBrand: async (): Promise<BrandDto> => {
    const response = await apiClient.get<BrandDto>("/api/branding");
    return response.data;
  },

  updateBrand: async (dto: UpdateBrandDto): Promise<BrandDto> => {
    const response = await apiClient.put<BrandDto>("/api/branding", dto);
    return response.data;
  },

  uploadLogo: async (file: File): Promise<{ logoUrl: string }> => {
    const formData = new FormData();
    formData.append("file", file);
    const response = await apiClient.post<{ logoUrl: string }>("/api/branding/logo", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return response.data;
  },

  uploadFavicon: async (file: File): Promise<{ faviconUrl: string }> => {
    const formData = new FormData();
    formData.append("file", file);
    const response = await apiClient.post<{ faviconUrl: string }>("/api/branding/favicon", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return response.data;
  },
};
