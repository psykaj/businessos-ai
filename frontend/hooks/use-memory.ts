"use client";

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { memoryService } from "@/lib/memory-service";
import { CreateMemoryDto, UpdateMemoryDto, CopilotContextRequestDto } from "@/types/memory";
import { toast } from "sonner";

export function useMemory() {
  const queryClient = useQueryClient();

  const useBusinessMemories = () => {
    return useQuery({
      queryKey: ["business-memories"],
      queryFn: () => memoryService.getBusinessMemories(),
    });
  };

  const useCustomerMemories = (customerId: string | null) => {
    return useQuery({
      queryKey: ["customer-memories", customerId],
      queryFn: () => (customerId ? memoryService.getCustomerMemories(customerId) : []),
      enabled: !!customerId,
    });
  };

  const useRelevantMemories = (query: string) => {
    return useQuery({
      queryKey: ["relevant-memories", query],
      queryFn: () => memoryService.getRelevantMemories(query),
      enabled: !!query,
    });
  };

  const useCopilotContext = (request: CopilotContextRequestDto, enabled: boolean = false) => {
    return useQuery({
      queryKey: ["copilot-context", request.question, request.entityId],
      queryFn: () => memoryService.getCopilotContext(request),
      enabled: enabled && !!request.question,
    });
  };

  const createMemoryMutation = useMutation({
    mutationFn: (data: CreateMemoryDto) => memoryService.createMemory(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["business-memories"] });
      queryClient.invalidateQueries({ queryKey: ["customer-memories"] });
      toast.success("Business memory created successfully.");
    },
    onError: () => {
      toast.error("Failed to create business memory.");
    },
  });

  const updateMemoryMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateMemoryDto }) =>
      memoryService.updateMemory(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["business-memories"] });
      queryClient.invalidateQueries({ queryKey: ["customer-memories"] });
      toast.success("Memory updated successfully.");
    },
    onError: () => {
      toast.error("Failed to update memory.");
    },
  });

  const deleteMemoryMutation = useMutation({
    mutationFn: (id: string) => memoryService.deleteMemory(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["business-memories"] });
      queryClient.invalidateQueries({ queryKey: ["customer-memories"] });
      toast.success("Memory deleted.");
    },
    onError: () => {
      toast.error("Failed to delete memory.");
    },
  });

  return {
    useBusinessMemories,
    useCustomerMemories,
    useRelevantMemories,
    useCopilotContext,
    createMemory: createMemoryMutation.mutateAsync,
    updateMemory: updateMemoryMutation.mutateAsync,
    deleteMemory: deleteMemoryMutation.mutateAsync,
    isUpdating: updateMemoryMutation.isPending,
    isDeleting: deleteMemoryMutation.isPending,
  };
}
