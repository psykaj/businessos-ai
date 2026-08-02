import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  CommunicationService,
  ConversationFilterDto,
  CommunicationChannelType
} from "@/lib/communication-service";
import { toast } from "sonner";

export const communicationKeys = {
  all: ["communication"] as const,
  inboxSummary: () => [...communicationKeys.all, "inbox-summary"] as const,
  conversations: (filter?: ConversationFilterDto) => [...communicationKeys.all, "conversations", filter] as const,
  conversationById: (id: string) => [...communicationKeys.all, "conversation", id] as const,
  messages: (conversationId: string) => [...communicationKeys.all, "messages", conversationId] as const,
  templates: (channel?: CommunicationChannelType, category?: string) => [...communicationKeys.all, "templates", { channel, category }] as const,
  analytics: () => [...communicationKeys.all, "analytics"] as const,
};

export function useInboxSummary() {
  return useQuery({
    queryKey: communicationKeys.inboxSummary(),
    queryFn: () => CommunicationService.getInboxSummary(),
    staleTime: 30_000,
  });
}

export function useConversations(filter?: ConversationFilterDto) {
  return useQuery({
    queryKey: communicationKeys.conversations(filter),
    queryFn: () => CommunicationService.getConversations(filter),
    staleTime: 15_000,
  });
}

export function useConversationById(id: string) {
  return useQuery({
    queryKey: communicationKeys.conversationById(id),
    queryFn: () => CommunicationService.getConversationById(id),
    enabled: !!id,
  });
}

export function useConversationMessages(conversationId: string) {
  return useQuery({
    queryKey: communicationKeys.messages(conversationId),
    queryFn: () => CommunicationService.getMessages(conversationId),
    enabled: !!conversationId,
    refetchInterval: 10_000, // Real-time chat sync simulation
  });
}

export function useSendMessage() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ conversationId, content, attachmentsJson }: { conversationId: string; content: string; attachmentsJson?: string }) =>
      CommunicationService.sendMessage(conversationId, content, attachmentsJson),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: communicationKeys.messages(variables.conversationId) });
      queryClient.invalidateQueries({ queryKey: communicationKeys.all });
      toast.success("Message sent successfully!");
    },
    onError: () => {
      toast.error("Failed to send message via channel provider.");
    },
  });
}

export function useAddInternalNote() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ conversationId, content }: { conversationId: string; content: string }) =>
      CommunicationService.addInternalNote(conversationId, content),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: communicationKeys.messages(variables.conversationId) });
      toast.info("Internal team note added to timeline.");
    },
    onError: () => {
      toast.error("Could not add internal note.");
    },
  });
}

export function useUpdateConversationStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, status }: { id: string; status: string }) =>
      CommunicationService.updateConversationStatus(id, status),
    onSuccess: (updated) => {
      queryClient.invalidateQueries({ queryKey: communicationKeys.all });
      toast.success(`Conversation marked as ${updated?.status || "updated"}.`);
    },
    onError: () => {
      toast.error("Failed to update conversation status.");
    },
  });
}

export function useAssignConversation() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, assignedToUserId, assignedToUserName, assignmentReason }: { id: string; assignedToUserId: string; assignedToUserName: string; assignmentReason?: string }) =>
      CommunicationService.assignConversation(id, assignedToUserId, assignedToUserName, assignmentReason),
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: communicationKeys.all });
      toast.success(`Assigned conversation to ${data?.assignedToUserName || "agent"}.`);
    },
    onError: () => {
      toast.error("Failed to assign conversation.");
    },
  });
}

export function useMessageTemplates(channel?: CommunicationChannelType, category?: string) {
  return useQuery({
    queryKey: communicationKeys.templates(channel, category),
    queryFn: () => CommunicationService.getTemplates(channel, category),
  });
}

export function useCreateTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (req: { title: string; content: string; channelType?: CommunicationChannelType; category?: string; shortcutCode?: string; parametersJson?: string }) =>
      CommunicationService.createTemplate(req),
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: communicationKeys.all });
      toast.success(`Created template "${data.title}" (${data.shortcutCode}).`);
    },
    onError: () => {
      toast.error("Failed to save message template.");
    },
  });
}

export function useUpdateTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, req }: { id: string; req: { title: string; content: string; channelType?: CommunicationChannelType; category?: string; shortcutCode?: string; parametersJson?: string } }) =>
      CommunicationService.updateTemplate(id, req),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: communicationKeys.all });
      toast.success("Updated message template.");
    },
  });
}

export function useDeleteTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => CommunicationService.deleteTemplate(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: communicationKeys.all });
      toast.success("Template deleted.");
    },
  });
}

export function useCommunicationAnalytics() {
  return useQuery({
    queryKey: communicationKeys.analytics(),
    queryFn: () => CommunicationService.getAnalyticsOverview(),
    staleTime: 60_000,
  });
}
