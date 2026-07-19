export interface TeamMember {
  id: string;
  userId: string;
  organizationId: string;
  fullName: string;
  email: string;
  role: string;
  isActive: boolean;
  lastLoginAt?: string;
  joinedAt: string;
  profilePictureUrl?: string;
}

export interface Invitation {
  id: string;
  organizationId: string;
  email: string;
  role: string;
  token: string;
  status: "Pending" | "Accepted" | "Expired" | "Revoked";
  expiresAt: string;
  createdAt: string;
}

export interface InviteTeamMemberRequest {
  email: string;
  role: string;
}

export interface UpdateTeamMemberRoleRequest {
  role: string;
}
