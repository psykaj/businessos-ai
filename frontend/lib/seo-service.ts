import apiClient from "./api-client";

export interface SEODto {
  id: string;
  organizationId: string;
  metaTitle?: string;
  metaDescription?: string;
  keywords?: string;
  canonicalUrl?: string;
  openGraphTitle?: string;
  openGraphDescription?: string;
  openGraphImage?: string;
  twitterCard: string;
  robots: string;
  createdAt: string;
  updatedAt: string;
}

export interface UpdateSEODto {
  metaTitle?: string;
  metaDescription?: string;
  keywords?: string;
  canonicalUrl?: string;
  openGraphTitle?: string;
  openGraphDescription?: string;
  openGraphImage?: string;
  twitterCard?: string;
  robots?: string;
}

export const SEOService = {
  getSEO: async (): Promise<SEODto> => {
    const response = await apiClient.get<SEODto>("/api/seo");
    return response.data;
  },

  updateSEO: async (dto: UpdateSEODto): Promise<SEODto> => {
    const response = await apiClient.put<SEODto>("/api/seo", dto);
    return response.data;
  },
};
