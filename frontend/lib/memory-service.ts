import apiClient from "./api-client";
import {
  MemoryDto,
  CreateMemoryDto,
  UpdateMemoryDto,
  CopilotContextRequestDto,
  CopilotContextResponseDto,
} from "@/types/memory";

export const memoryService = {
  getBusinessMemories: async (): Promise<MemoryDto[]> => {
    const response = await apiClient.get("/api/memory");
    return response.data;
  },

  getMemory: async (id: string): Promise<MemoryDto> => {
    const response = await apiClient.get(`/api/memory/${id}`);
    return response.data;
  },

  getCustomerMemories: async (customerId: string): Promise<MemoryDto[]> => {
    const response = await apiClient.get(`/api/memory/customer/${customerId}`);
    return response.data;
  },

  getRelevantMemories: async (query: string): Promise<MemoryDto[]> => {
    const response = await apiClient.get(`/api/memory/relevant?query=${encodeURIComponent(query)}`);
    return response.data;
  },

  createMemory: async (data: CreateMemoryDto): Promise<MemoryDto> => {
    const response = await apiClient.post("/api/memory", data);
    return response.data;
  },

  updateMemory: async (id: string, data: UpdateMemoryDto): Promise<MemoryDto> => {
    const response = await apiClient.put(`/api/memory/${id}`, data);
    return response.data;
  },

  deleteMemory: async (id: string): Promise<void> => {
    await apiClient.delete(`/api/memory/${id}`);
  },

  rebuildContext: async (entityType: string, entityId: string): Promise<void> => {
    await apiClient.post(`/api/memory/rebuild-context/${entityType}/${entityId}`);
  },

  getCopilotContext: async (
    data: CopilotContextRequestDto
  ): Promise<CopilotContextResponseDto> => {
    const response = await apiClient.post("/api/copilot/context", data);
    return response.data;
  },
};
