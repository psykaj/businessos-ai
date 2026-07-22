import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { workflowApi, CreateWorkflowPayload, UpdateWorkflowPayload } from "@/lib/api/workflows";
import { toast } from "sonner";

export function useWorkflows(pageNumber = 1, pageSize = 20, search?: string, status?: string) {
  return useQuery({
    queryKey: ["workflows", pageNumber, pageSize, search, status],
    queryFn: () => workflowApi.getWorkflows(pageNumber, pageSize, search, status),
  });
}

export function useWorkflow(id: string) {
  return useQuery({
    queryKey: ["workflow", id],
    queryFn: () => workflowApi.getWorkflowById(id),
    enabled: !!id,
  });
}

export function useCreateWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: CreateWorkflowPayload) => workflowApi.createWorkflow(payload),
    onSuccess: () => {
      toast.success("Workflow created successfully");
      queryClient.invalidateQueries({ queryKey: ["workflows"] });
    },
    onError: (err: any) => {
      toast.error(err?.response?.data?.message || "Failed to create workflow");
    },
  });
}

export function useUpdateWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, payload }: { id: string; payload: UpdateWorkflowPayload }) =>
      workflowApi.updateWorkflow(id, payload),
    onSuccess: (_, variables) => {
      toast.success("Workflow updated successfully");
      queryClient.invalidateQueries({ queryKey: ["workflows"] });
      queryClient.invalidateQueries({ queryKey: ["workflow", variables.id] });
    },
    onError: (err: any) => {
      toast.error(err?.response?.data?.message || "Failed to update workflow");
    },
  });
}

export function useDeleteWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => workflowApi.deleteWorkflow(id),
    onSuccess: () => {
      toast.success("Workflow deleted");
      queryClient.invalidateQueries({ queryKey: ["workflows"] });
    },
    onError: (err: any) => {
      toast.error(err?.response?.data?.message || "Failed to delete workflow");
    },
  });
}

export function useExecuteWorkflowManual() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, triggerData }: { id: string; triggerData?: Record<string, string> }) =>
      workflowApi.executeWorkflowManual(id, triggerData),
    onSuccess: () => {
      toast.success("Workflow execution started!");
      queryClient.invalidateQueries({ queryKey: ["workflow-executions"] });
    },
    onError: (err: any) => {
      toast.error(err?.response?.data?.message || "Execution failed");
    },
  });
}

export function useWorkflowExecutions(pageNumber = 1, pageSize = 20, status?: string) {
  return useQuery({
    queryKey: ["workflow-executions", pageNumber, pageSize, status],
    queryFn: () => workflowApi.getExecutions(pageNumber, pageSize, status),
  });
}

export function useWorkflowExecutionLogs(executionId: string) {
  return useQuery({
    queryKey: ["workflow-execution-logs", executionId],
    queryFn: () => workflowApi.getExecutionLogs(executionId),
    enabled: !!executionId,
  });
}
