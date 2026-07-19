import apiClient from "./api-client";

export interface ThemeDto {
  id: string;
  organizationId: string;
  themeName: string;
  themeMode: string;
  themeJson: string;
  isDefault: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateThemeDto {
  themeName: string;
  themeMode: string;
  themeJson: string;
}

export interface UpdateThemeDto {
  themeName?: string;
  themeMode?: string;
  themeJson?: string;
}

export const ThemesService = {
  getThemes: async (): Promise<ThemeDto[]> => {
    const response = await apiClient.get<ThemeDto[]>("/api/themes");
    return response.data;
  },

  getTheme: async (id: string): Promise<ThemeDto> => {
    const response = await apiClient.get<ThemeDto>(`/api/themes/${id}`);
    return response.data;
  },

  createTheme: async (dto: CreateThemeDto): Promise<ThemeDto> => {
    const response = await apiClient.post<ThemeDto>("/api/themes", dto);
    return response.data;
  },

  updateTheme: async (id: string, dto: UpdateThemeDto): Promise<ThemeDto> => {
    const response = await apiClient.put<ThemeDto>(`/api/themes/${id}`, dto);
    return response.data;
  },

  setDefault: async (id: string): Promise<ThemeDto> => {
    const response = await apiClient.put<ThemeDto>(`/api/themes/${id}/default`);
    return response.data;
  },

  deleteTheme: async (id: string): Promise<void> => {
    await apiClient.delete(`/api/themes/${id}`);
  },
};
