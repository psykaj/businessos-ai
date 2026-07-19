import apiClient from "./api-client";

export interface LandingPageSectionDto {
  id: string;
  sectionType: string;
  sortOrder: number;
  contentJson: string;
}

export interface LandingPageDto {
  id: string;
  organizationId: string;
  title: string;
  slug: string;
  status: string;
  publishedAt: string | null;
  createdAt: string;
  updatedAt: string;
  sections: LandingPageSectionDto[];
}

export interface CreateLandingPageDto {
  title: string;
  slug: string;
}

export interface UpdateLandingPageSectionDto {
  id?: string;
  sectionType: string;
  sortOrder: number;
  contentJson: string;
}

export interface UpdateLandingPageDto {
  title?: string;
  slug?: string;
  status?: string;
  sections?: UpdateLandingPageSectionDto[];
}

export const LandingPagesService = {
  getPages: async (): Promise<LandingPageDto[]> => {
    const response = await apiClient.get<LandingPageDto[]>("/api/landing-pages");
    return response.data;
  },

  getPage: async (id: string): Promise<LandingPageDto> => {
    const response = await apiClient.get<LandingPageDto>(`/api/landing-pages/${id}`);
    return response.data;
  },

  getPageBySlug: async (slug: string, orgId: string): Promise<LandingPageDto> => {
    const response = await apiClient.get<LandingPageDto>(`/api/landing-pages/slug/${slug}?orgId=${orgId}`);
    return response.data;
  },

  createPage: async (dto: CreateLandingPageDto): Promise<LandingPageDto> => {
    const response = await apiClient.post<LandingPageDto>("/api/landing-pages", dto);
    return response.data;
  },

  updatePage: async (id: string, dto: UpdateLandingPageDto): Promise<LandingPageDto> => {
    const response = await apiClient.put<LandingPageDto>(`/api/landing-pages/${id}`, dto);
    return response.data;
  },

  deletePage: async (id: string): Promise<void> => {
    await apiClient.delete(`/api/landing-pages/${id}`);
  },
};
