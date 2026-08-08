import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { aiAutomationApi } from "@/lib/api/ai-automation";
import { toast } from "sonner";
import { Workflow } from "@/types/automation";

export function useAiWorkflows() {
  return useQuery({
    queryKey: ["ai-automation-workflows"],
    queryFn: aiAutomationApi.getWorkflows,
  });
}

export function useAiWorkflow(id: string) {
  return useQuery({
    queryKey: ["ai-automation-workflows", id],
    queryFn: () => aiAutomationApi.getWorkflowById(id),
    enabled: !!id,
  });
}

export function useCreateAiWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: Partial<Workflow>) => aiAutomationApi.createWorkflow(payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["ai-automation-workflows"] });
      toast.success("Automation created successfully.");
    },
    onError: (err) => {
      toast.error("Failed to create automation.");
      console.error(err);
    }
  });
}

export function useActivateAiWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => aiAutomationApi.activateWorkflow(id),
    onSuccess: (_, id) => {
      queryClient.invalidateQueries({ queryKey: ["ai-automation-workflows"] });
      queryClient.invalidateQueries({ queryKey: ["ai-automation-workflows", id] });
      toast.success("Automation activated.");
    },
    onError: () => toast.error("Failed to activate automation.")
  });
}

export function useDeactivateAiWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => aiAutomationApi.deactivateWorkflow(id),
    onSuccess: (_, id) => {
      queryClient.invalidateQueries({ queryKey: ["ai-automation-workflows"] });
      queryClient.invalidateQueries({ queryKey: ["ai-automation-workflows", id] });
      toast.success("Automation paused.");
    },
    onError: () => toast.error("Failed to pause automation.")
  });
}

export function useDeleteAiWorkflow() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => aiAutomationApi.deleteWorkflow(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["ai-automation-workflows"] });
      toast.success("Automation deleted.");
    },
    onError: () => toast.error("Failed to delete automation.")
  });
}

export function useAiExecutions(limit = 50) {
  return useQuery({
    queryKey: ["ai-automation-executions", limit],
    queryFn: () => aiAutomationApi.getExecutions(limit),
  });
}

export function useAiExecution(id: string) {
  return useQuery({
    queryKey: ["ai-automation-executions", id],
    queryFn: () => aiAutomationApi.getExecutionById(id),
    enabled: !!id,
  });
}

export function useAiTemplates() {
  return useQuery({
    queryKey: ["ai-automation-templates"],
    queryFn: aiAutomationApi.getTemplates,
  });
}

export function useInstallAiTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => aiAutomationApi.installTemplate(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["ai-automation-workflows"] });
      toast.success("Template installed successfully.");
    },
    onError: () => toast.error("Failed to install template.")
  });
}
