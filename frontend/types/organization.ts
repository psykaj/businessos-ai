export interface Organization {
  id: string;
  name: string;
  slug: string;
  email: string;
  industry?: string;
  website?: string;
  address?: string;
  timeZone: string;
  language: string;
  currency: string;
  logoUrl?: string;
  subscriptionPlan: string;
  subscriptionStatus: string;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
}

export interface UpdateOrganizationRequest {
  name: string;
  slug: string;
  industry?: string;
  website?: string;
  address?: string;
  timeZone: string;
  language: string;
  currency: string;
}
