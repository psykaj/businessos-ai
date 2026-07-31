import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { automationStudioApi } from "../lib/api/automation-studio";
import { toast } from "sonner";

export function useAutomationWorkflows() {
  return useQuery({
    queryKey: ["automation-workflows"],
    queryFn: automationStudioApi.getWorkflows,
  });
}

export function useAutomationWorkflow(id: string) {
  return useQuery({
    queryKey: ["automation-workflows", id],
    queryFn: () => automationStudioApi.getWorkflowById(id),
    enabled: !!id,
  });
}

export function useSaveWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, payload }: { id: string; payload: Record<string, unknown> }) => automationStudioApi.saveWorkflow(id, payload),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ["automation-workflows"] });
      queryClient.invalidateQueries({ queryKey: ["automation-workflows", variables.id] });
      toast.success("Workflow saved successfully");
    },
    onError: () => {
      toast.error("Failed to save workflow");
    }
  });
}

export function useAutomationTemplates() {
  return useQuery({
    queryKey: ["automation-templates"],
    queryFn: automationStudioApi.getTemplates,
  });
}

export function useCloneTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => automationStudioApi.cloneTemplate(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["automation-workflows"] });
      toast.success("Template cloned successfully");
    },
    onError: () => {
      toast.error("Failed to clone template");
    }
  });
}

export function useAutomationExecutions() {
  return useQuery({
    queryKey: ["automation-executions"],
    queryFn: automationStudioApi.getExecutions,
  });
}
