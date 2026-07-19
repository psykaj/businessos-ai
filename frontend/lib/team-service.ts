import apiClient from "./api-client";
import { TeamMember, Invitation, InviteTeamMemberRequest, UpdateTeamMemberRoleRequest } from "@/types/team";

export const teamService = {
  getTeamMembers: async (orgId: string): Promise<TeamMember[]> => {
    const response = await apiClient.get<TeamMember[]>(`/api/v1/organizations/${orgId}/team`);
    return response.data;
  },

  updateMemberRole: async (orgId: string, memberId: string, data: UpdateTeamMemberRoleRequest): Promise<void> => {
    await apiClient.put(`/api/v1/organizations/${orgId}/team/${memberId}/role`, data);
  },

  removeMember: async (orgId: string, memberId: string): Promise<void> => {
    await apiClient.delete(`/api/v1/organizations/${orgId}/team/${memberId}`);
  },

  deactivateMember: async (orgId: string, memberId: string): Promise<void> => {
    await apiClient.post(`/api/v1/organizations/${orgId}/team/${memberId}/deactivate`);
  },

  reactivateMember: async (orgId: string, memberId: string): Promise<void> => {
    await apiClient.post(`/api/v1/organizations/${orgId}/team/${memberId}/reactivate`);
  },

  getInvitations: async (orgId: string): Promise<Invitation[]> => {
    const response = await apiClient.get<Invitation[]>(`/api/v1/organizations/${orgId}/invitations`);
    return response.data;
  },

  inviteMember: async (orgId: string, data: InviteTeamMemberRequest): Promise<Invitation> => {
    const response = await apiClient.post<Invitation>(`/api/v1/organizations/${orgId}/team/invite`, data);
    return response.data;
  },

  cancelInvitation: async (orgId: string, id: string): Promise<void> => {
    await apiClient.delete(`/api/v1/organizations/${orgId}/invitations/${id}`);
  },
};
