import axios from "axios";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use((config) => {
  if (typeof window !== "undefined") {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
  }
  return config;
});

export interface WorkflowTrigger {
  id?: string;
  workflowId?: string;
  triggerType: string;
  triggerConfiguration: string;
  enabled: boolean;
}

export interface WorkflowCondition {
  id?: string;
  workflowId?: string;
  workflowActionId?: string;
  fieldName: string;
  operator: string;
  value: string;
  logicalOperator: string;
}

export interface WorkflowAction {
  id?: string;
  workflowId?: string;
  actionType: string;
  configuration: string;
  executionOrder: number;
  conditions?: WorkflowCondition[];
}

export interface Workflow {
  id: string;
  organizationId: string;
  name: string;
  description?: string;
  status: "Draft" | "Active" | "Paused" | "Archived";
  version: number;
  createdBy?: string;
  createdAt: string;
  updatedAt: string;
  trigger?: WorkflowTrigger;
  actions: WorkflowAction[];
  conditions: WorkflowCondition[];
}

export interface CreateWorkflowPayload {
  name: string;
  description?: string;
  trigger?: {
    triggerType: string;
    triggerConfiguration: string;
    enabled?: boolean;
  };
  actions?: {
    actionType: string;
    configuration: string;
    executionOrder: number;
  }[];
  conditions?: {
    workflowActionId?: string;
    fieldName: string;
    operator: string;
    value: string;
    logicalOperator?: string;
  }[];
}

export interface UpdateWorkflowPayload extends CreateWorkflowPayload {
  status: "Draft" | "Active" | "Paused" | "Archived";
}

export interface WorkflowExecutionLog {
  id: string;
  executionId: string;
  workflowId: string;
  stepName: string;
  stepType: string;
  status: "Pending" | "Running" | "Completed" | "Failed" | "Retrying" | "Cancelled";
  startedAt: string;
  finishedAt?: string;
  inputJson: string;
  outputJson: string;
  errorMessage?: string;
}

export interface WorkflowExecution {
  id: string;
  workflowId: string;
  organizationId: string;
  startedAt: string;
  finishedAt?: string;
  durationMs: number;
  status: "Pending" | "Running" | "Completed" | "Failed" | "Retrying" | "Cancelled";
  errorMessage?: string;
  executedBy: string;
  triggerDataJson: string;
  contextDataJson: string;
  logs?: WorkflowExecutionLog[];
}

export interface Integration {
  id: string;
  organizationId: string;
  provider: string;
  displayName: string;
  maskedApiKey?: string;
  status: "Active" | "Disconnected" | "Expired" | "Error";
  metadataJson: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateIntegrationPayload {
  provider: string;
  displayName: string;
  apiKey?: string;
  accessToken?: string;
  refreshToken?: string;
  metadataJson?: string;
}

export interface UpdateIntegrationPayload {
  displayName: string;
  apiKey?: string;
  accessToken?: string;
  refreshToken?: string;
  status: "Active" | "Disconnected" | "Expired" | "Error";
  metadataJson?: string;
}

export const workflowApi = {
  getWorkflows: async (pageNumber = 1, pageSize = 20, search?: string, status?: string) => {
    const params = new URLSearchParams();
    params.append("pageNumber", pageNumber.toString());
    params.append("pageSize", pageSize.toString());
    if (search) params.append("search", search);
    if (status) params.append("status", status);

    const res = await api.get<{ items: Workflow[]; totalCount: number; pageNumber: number; pageSize: number }>(`/api/workflows?${params.toString()}`);
    return res.data;
  },

  getWorkflowById: async (id: string) => {
    const res = await api.get<Workflow>(`/api/workflows/${id}`);
    return res.data;
  },

  createWorkflow: async (payload: CreateWorkflowPayload) => {
    const res = await api.post<Workflow>("/api/workflows", payload);
    return res.data;
  },

  updateWorkflow: async (id: string, payload: UpdateWorkflowPayload) => {
    const res = await api.put<Workflow>(`/api/workflows/${id}`, payload);
    return res.data;
  },

  deleteWorkflow: async (id: string) => {
    await api.delete(`/api/workflows/${id}`);
  },

  executeWorkflowManual: async (id: string, triggerData?: Record<string, string>) => {
    const res = await api.post<WorkflowExecution>(`/api/workflows/${id}/execute`, { triggerData });
    return res.data;
  },

  getTriggerTypes: async () => {
    const res = await api.get<string[]>("/api/workflows/triggers/types");
    return res.data;
  },

  getActionTypes: async () => {
    const res = await api.get<string[]>("/api/workflows/actions/types");
    return res.data;
  },

  getExecutions: async (pageNumber = 1, pageSize = 20, status?: string) => {
    const params = new URLSearchParams();
    params.append("pageNumber", pageNumber.toString());
    params.append("pageSize", pageSize.toString());
    if (status) params.append("status", status);

    const res = await api.get<{ items: WorkflowExecution[]; totalCount: number; pageNumber: number; pageSize: number }>(`/api/workflows/executions?${params.toString()}`);
    return res.data;
  },

  getExecutionById: async (id: string) => {
    const res = await api.get<WorkflowExecution>(`/api/workflows/executions/${id}`);
    return res.data;
  },

  getExecutionLogs: async (executionId: string) => {
    const res = await api.get<WorkflowExecutionLog[]>(`/api/workflows/logs/execution/${executionId}`);
    return res.data;
  },

  // Integrations
  getIntegrations: async () => {
    const res = await api.get<Integration[]>("/api/integrations");
    return res.data;
  },

  getIntegrationById: async (id: string) => {
    const res = await api.get<Integration>(`/api/integrations/${id}`);
    return res.data;
  },

  createIntegration: async (payload: CreateIntegrationPayload) => {
    const res = await api.post<Integration>("/api/integrations", payload);
    return res.data;
  },

  updateIntegration: async (id: string, payload: UpdateIntegrationPayload) => {
    const res = await api.put<Integration>(`/api/integrations/${id}`, payload);
    return res.data;
  },

  deleteIntegration: async (id: string) => {
    await api.delete(`/api/integrations/${id}`);
  },

  testIntegration: async (id: string) => {
    const res = await api.post<{ success: boolean; message: string; testedAt: string }>(`/api/integrations/${id}/test`);
    return res.data;
  }
};
