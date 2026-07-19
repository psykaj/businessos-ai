import apiClient from "./api-client";

export const MediaService = {
  uploadMedia: async (file: File, folder: string = "general"): Promise<{ url: string }> => {
    const formData = new FormData();
    formData.append("file", file);
    
    // We send folder in query params for the MediaController
    const response = await apiClient.post<{ url: string }>(`/api/media/upload?folder=${encodeURIComponent(folder)}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    
    return response.data;
  },
};
