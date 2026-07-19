import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { teamService } from "@/lib/team-service";
import { InviteTeamMemberRequest, UpdateTeamMemberRoleRequest } from "@/types/team";

export function useTeamMembers(orgId: string | undefined) {
  return useQuery({
    queryKey: ["teamMembers", orgId],
    queryFn: () => teamService.getTeamMembers(orgId!),
    enabled: !!orgId,
  });
}

export function useInvitations(orgId: string | undefined) {
  return useQuery({
    queryKey: ["invitations", orgId],
    queryFn: () => teamService.getInvitations(orgId!),
    enabled: !!orgId,
  });
}

export function useInviteMember() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, data }: { orgId: string; data: InviteTeamMemberRequest }) =>
      teamService.inviteMember(orgId, data),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["invitations", orgId] });
    },
  });
}

export function useUpdateMemberRole() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, memberId, data }: { orgId: string; memberId: string; data: UpdateTeamMemberRoleRequest }) =>
      teamService.updateMemberRole(orgId, memberId, data),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["teamMembers", orgId] });
    },
  });
}

export function useRemoveMember() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, memberId }: { orgId: string; memberId: string }) =>
      teamService.removeMember(orgId, memberId),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["teamMembers", orgId] });
    },
  });
}

export function useDeactivateMember() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, memberId }: { orgId: string; memberId: string }) =>
      teamService.deactivateMember(orgId, memberId),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["teamMembers", orgId] });
    },
  });
}

export function useReactivateMember() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, memberId }: { orgId: string; memberId: string }) =>
      teamService.reactivateMember(orgId, memberId),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["teamMembers", orgId] });
    },
  });
}

export function useCancelInvitation() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, id }: { orgId: string; id: string }) =>
      teamService.cancelInvitation(orgId, id),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["invitations", orgId] });
    },
  });
}
