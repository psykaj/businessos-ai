"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { copilotService, ExecuteCommandRequest } from "@/lib/copilot-service";
import { toast } from "sonner";

export function useCopilot() {
  const queryClient = useQueryClient();

  // Conversations Query
  const useConversations = (params?: { page?: number; pageSize?: number; search?: string; status?: string }) => {
    return useQuery({
      queryKey: ["copilot-conversations", params],
      queryFn: () => copilotService.getConversations(params),
    });
  };

  // Single Conversation Query
  const useConversation = (id: string | null) => {
    return useQuery({
      queryKey: ["copilot-conversation", id],
      queryFn: () => (id ? copilotService.getConversationById(id) : null),
      enabled: !!id,
    });
  };

  // Recommendations Query
  const useRecommendations = (params?: { page?: number; pageSize?: number; category?: string; priority?: string; isApplied?: boolean }) => {
    return useQuery({
      queryKey: ["copilot-recommendations", params],
      queryFn: () => copilotService.getRecommendations(params),
    });
  };

  // Executions Audit Query
  const useExecutions = (params?: { page?: number; pageSize?: number; status?: string; tool?: string }) => {
    return useQuery({
      queryKey: ["copilot-executions", params],
      queryFn: () => copilotService.getExecutions(params),
    });
  };

  // Tools Query
  const useTools = () => {
    return useQuery({
      queryKey: ["copilot-tools"],
      queryFn: () => copilotService.getTools(),
    });
  };

  // Create Conversation Mutation
  const createConversationMutation = useMutation({
    mutationFn: (title?: string) => copilotService.createConversation(title),
    onSuccess: (newConv) => {
      queryClient.invalidateQueries({ queryKey: ["copilot-conversations"] });
      toast.success("New conversation created");
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message || "Failed to create conversation");
    },
  });

  // Archive Conversation Mutation
  const archiveConversationMutation = useMutation({
    mutationFn: (id: string) => copilotService.archiveConversation(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["copilot-conversations"] });
      toast.success("Conversation archived");
    },
  });

  // Delete Conversation Mutation
  const deleteConversationMutation = useMutation({
    mutationFn: (id: string) => copilotService.deleteConversation(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["copilot-conversations"] });
      toast.success("Conversation deleted");
    },
  });

  // Analyze Recommendations Mutation
  const analyzeRecommendationsMutation = useMutation({
    mutationFn: () => copilotService.analyzeRecommendations(),
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["copilot-recommendations"] });
      toast.success(`Analysis complete. Found ${data.length} recommendations.`);
    },
    onError: () => {
      toast.error("Failed to analyze business recommendations.");
    },
  });

  // Apply Recommendation Mutation
  const applyRecommendationMutation = useMutation({
    mutationFn: (id: string) => copilotService.applyRecommendation(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["copilot-recommendations"] });
      toast.success("Recommendation marked as applied");
    },
  });

  // Execute Command Mutation
  const executeCommandMutation = useMutation({
    mutationFn: (request: ExecuteCommandRequest) => copilotService.executeCommand(request),
    onSuccess: (res) => {
      queryClient.invalidateQueries({ queryKey: ["copilot-conversation", res.conversationId] });
      queryClient.invalidateQueries({ queryKey: ["copilot-executions"] });
      if (res.status === "RequiresConfirmation") {
        toast.warning("Action requires confirmation to proceed.");
      } else if (res.status === "Success") {
        toast.success(`Action executed successfully via ${res.toolInvoked}`);
      } else if (res.status === "Failed") {
        toast.error(res.errorMessage || "Action execution failed");
      }
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message || "Failed to execute command");
    },
  });

  return {
    useConversations,
    useConversation,
    useRecommendations,
    useExecutions,
    useTools,
    createConversation: createConversationMutation.mutateAsync,
    archiveConversation: archiveConversationMutation.mutateAsync,
    deleteConversation: deleteConversationMutation.mutateAsync,
    analyzeRecommendations: analyzeRecommendationsMutation.mutateAsync,
    applyRecommendation: applyRecommendationMutation.mutateAsync,
    executeCommand: executeCommandMutation.mutateAsync,
    isExecuting: executeCommandMutation.isPending,
    isAnalyzing: analyzeRecommendationsMutation.isPending,
  };
}
