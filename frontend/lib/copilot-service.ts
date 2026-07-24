import apiClient from "./api-client";

export interface ExecuteCommandRequest {
  command: string;
  conversationId?: string;
  isConfirmed?: boolean;
  specifiedTool?: string;
  parameters?: Record<string, any>;
}

export interface ExecutionResponse {
  executionId: string;
  organizationId: string;
  userId: string;
  command: string;
  toolInvoked: string;
  status: "Success" | "Failed" | "RequiresConfirmation" | "Pending" | "Running";
  startedAt: string;
  finishedAt?: string;
  resultSummary: string;
  data?: any;
  requiresConfirmation: boolean;
  errorMessage?: string;
  conversationId?: string;
}

export interface Conversation {
  id: string;
  organizationId: string;
  userId: string;
  title: string;
  status: "Active" | "Archived";
  createdAt: string;
  updatedAt: string;
  messageCount: number;
  messages?: Message[];
}

export interface Message {
  id: string;
  conversationId: string;
  role: "User" | "Assistant" | "System";
  content: string;
  toolInvoked?: string;
  executionId?: string;
  createdAt: string;
}

export interface Recommendation {
  id: string;
  organizationId: string;
  category: string;
  title: string;
  description: string;
  priority: "High" | "Medium" | "Low";
  reason?: string;
  suggestedAction: string;
  isApplied: boolean;
  appliedAt?: string;
  createdAt: string;
}

export interface CommandExecution {
  id: string;
  organizationId: string;
  userId: string;
  command: string;
  toolInvoked: string;
  status: string;
  startedAt: string;
  finishedAt?: string;
  resultSummary?: string;
  errorMessage?: string;
  parametersJson?: string;
}

export interface ToolDefinition {
  name: string;
  description: string;
  category: string;
  requiredPermissions: string[];
  isDestructive: boolean;
  parametersSchema?: string;
}

export const copilotService = {
  // Execute Command
  executeCommand: async (request: ExecuteCommandRequest): Promise<ExecutionResponse> => {
    const response = await apiClient.post<ExecutionResponse>("/api/ai-agent/execute", request);
    return response.data;
  },

  getAgentStatus: async () => {
    const response = await apiClient.get("/api/ai-agent/status");
    return response.data;
  },

  // Conversations
  createConversation: async (title: string = "New Business Conversation"): Promise<Conversation> => {
    const response = await apiClient.post<Conversation>("/api/ai-agent/conversations", { title });
    return response.data;
  },

  getConversations: async (params?: { page?: number; pageSize?: number; search?: string; status?: string }) => {
    const response = await apiClient.get("/api/ai-agent/conversations", { params });
    return response.data;
  },

  getConversationById: async (id: string): Promise<Conversation> => {
    const response = await apiClient.get<Conversation>(`/api/ai-agent/conversations/${id}`);
    return response.data;
  },

  updateConversation: async (id: string, data: { title?: string; status?: string }): Promise<Conversation> => {
    const response = await apiClient.put<Conversation>(`/api/ai-agent/conversations/${id}`, data);
    return response.data;
  },

  archiveConversation: async (id: string) => {
    const response = await apiClient.post(`/api/ai-agent/conversations/${id}/archive`);
    return response.data;
  },

  deleteConversation: async (id: string) => {
    const response = await apiClient.delete(`/api/ai-agent/conversations/${id}`);
    return response.data;
  },

  getMessageHistory: async (id: string): Promise<Message[]> => {
    const response = await apiClient.get<Message[]>(`/api/ai-agent/conversations/${id}/messages`);
    return response.data;
  },

  // Recommendations
  getRecommendations: async (params?: { page?: number; pageSize?: number; category?: string; priority?: string; isApplied?: boolean }) => {
    const response = await apiClient.get("/api/ai-agent/recommendations", { params });
    return response.data;
  },

  analyzeRecommendations: async (): Promise<Recommendation[]> => {
    const response = await apiClient.post<Recommendation[]>("/api/ai-agent/recommendations/analyze");
    return response.data;
  },

  applyRecommendation: async (id: string): Promise<Recommendation> => {
    const response = await apiClient.post<Recommendation>(`/api/ai-agent/recommendations/${id}/apply`);
    return response.data;
  },

  // Executions History Audit
  getExecutions: async (params?: { page?: number; pageSize?: number; status?: string; tool?: string }) => {
    const response = await apiClient.get("/api/ai-agent/executions", { params });
    return response.data;
  },

  getExecutionById: async (id: string): Promise<CommandExecution> => {
    const response = await apiClient.get<CommandExecution>(`/api/ai-agent/executions/${id}`);
    return response.data;
  },

  // Tool Definitions
  getTools: async (): Promise<ToolDefinition[]> => {
    const response = await apiClient.get<ToolDefinition[]>("/api/ai-agent/tools");
    return response.data;
  }
};
