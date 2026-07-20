export enum LeadStatus {
  New,
  Contacted,
  Qualified,
  Unqualified,
  Customer
}

export enum PipelineStage {
  NewLead,
  Contacted,
  Qualified,
  ProposalSent,
  Negotiation,
  Won,
  Lost
}

export enum ActivityType {
  Call,
  Meeting,
  Email,
  Note,
  SystemEvent
}

export enum TaskPriority {
  Low,
  Medium,
  High,
  Urgent
}

export enum CrmTaskStatus {
  Pending,
  InProgress,
  Completed,
  Cancelled
}

export interface CrmTag {
  id: string;
  name: string;
  color: string;
}

export interface Lead {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  companyName: string;
  jobTitle?: string;
  source?: string;
  status: LeadStatus;
  estimatedValue: number;
  priority: number;
  assignedUserId?: string;
  createdAt: string;
  updatedAt: string;
}

export interface Contact {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  jobTitle?: string;
  companyId?: string;
  companyName?: string;
  address?: string;
  socialLinks?: string;
  tags?: string;
  createdAt: string;
  updatedAt: string;
}

export interface Company {
  id: string;
  companyName: string;
  industry?: string;
  website?: string;
  address?: string;
  annualRevenue: number;
  employeeCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface Deal {
  id: string;
  title: string;
  description?: string;
  amount: number;
  probability: number;
  expectedCloseDate?: string;
  pipelineStage: PipelineStage;
  leadId?: string;
  companyId?: string;
  assignedUserId?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CrmActivity {
  id: string;
  relatedEntity: string;
  relatedEntityId: string;
  type: ActivityType;
  description: string;
  activityDate: string;
  performedByUserId?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CrmTask {
  id: string;
  title: string;
  description?: string;
  relatedEntity: string;
  relatedEntityId: string;
  assignedToId?: string;
  priority: TaskPriority;
  dueDate?: string;
  status: CrmTaskStatus;
  createdAt: string;
  updatedAt: string;
}

export interface Note {
  id: string;
  relatedEntity: string;
  relatedEntityId: string;
  content: string;
  createdByUserId?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CrmOverview {
  totalLeads: number;
  totalDeals: number;
  revenueForecast: number;
  pipelineValue: number;
  conversionRate: number;
}

export interface SalesPerformance {
  date: string;
  revenue: number;
  dealsWon: number;
}

export interface GlobalSearchResult {
  entityType: string;
  entityId: string;
  title: string;
  description: string;
  matchReason: string;
}
