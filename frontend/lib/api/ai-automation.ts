import api from "../api-client";
import { Workflow, WorkflowExecution, WorkflowTemplate } from "@/types/automation";

export const aiAutomationApi = {
  getWorkflows: async () => {
    const response = await api.get<Workflow[]>("/api/automation/workflows");
    return response.data;
  },

  getWorkflowById: async (id: string) => {
    const response = await api.get<Workflow>(`/api/automation/workflows/${id}`);
    return response.data;
  },

  createWorkflow: async (payload: Partial<Workflow>) => {
    const response = await api.post<Workflow>("/api/automation/workflows", payload);
    return response.data;
  },

  updateWorkflow: async (id: string, payload: Partial<Workflow>) => {
    const response = await api.put<Workflow>(`/api/automation/workflows/${id}`, payload);
    return response.data;
  },

  deleteWorkflow: async (id: string) => {
    const response = await api.delete(`/api/automation/workflows/${id}`);
    return response.data;
  },

  activateWorkflow: async (id: string) => {
    const response = await api.post(`/api/automation/workflows/${id}/activate`);
    return response.data;
  },

  deactivateWorkflow: async (id: string) => {
    const response = await api.post(`/api/automation/workflows/${id}/deactivate`);
    return response.data;
  },

  testWorkflow: async (id: string, eventData: any) => {
    const response = await api.post(`/api/automation/workflows/${id}/test`, eventData);
    return response.data;
  },

  getExecutions: async (limit: number = 50) => {
    const response = await api.get<WorkflowExecution[]>(`/api/automation/executions?limit=${limit}`);
    return response.data;
  },

  getExecutionById: async (id: string) => {
    const response = await api.get<WorkflowExecution>(`/api/automation/executions/${id}`);
    return response.data;
  },

  getTemplates: async () => {
    const response = await api.get<WorkflowTemplate[]>("/api/automation/templates");
    return response.data;
  },

  installTemplate: async (id: string) => {
    const response = await api.post(`/api/automation/templates/${id}/install`);
    return response.data;
  },

  approveAction: async (id: string) => {
    const response = await api.post(`/api/automation/actions/${id}/approve`);
    return response.data;
  },

  rejectAction: async (id: string) => {
    const response = await api.post(`/api/automation/actions/${id}/reject`);
    return response.data;
  },
};
