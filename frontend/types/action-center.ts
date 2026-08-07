export type AiActionCategory = 
  | 'GenerateInvoice'
  | 'SendPaymentReminder'
  | 'ReorderInventory'
  | 'CreateFollowUpTask'
  | 'CreateCrmActivity'
  | 'SendWhatsAppCampaign'
  | 'SendEmailCampaign'
  | 'GenerateDiscountCoupon'
  | 'ScheduleCustomerFollowUp'
  | 'MarkInvoicePaid'
  | 'Other';

export type AiActionPriority = 'Low' | 'Medium' | 'High' | 'Critical';
export type AiActionRiskLevel = 'Low' | 'Medium' | 'High';
export type AiActionStatus = 'Pending' | 'Approved' | 'Rejected' | 'Executed' | 'Failed';

export interface AiActionDto {
  id: string;
  organizationId: string;
  title: string;
  description: string;
  category: AiActionCategory;
  priority: AiActionPriority;
  businessImpact: string;
  estimatedRevenueIncrease: number;
  estimatedCostSaving: number;
  riskLevel: AiActionRiskLevel;
  status: AiActionStatus;
  createdAt: string;
  executedDate?: string;
  approvedBy?: string;
  executionResult?: string;
}

export interface ActionCenterSummary {
  revenueSaved: number;
  costReduced: number;
  timeSaved: number;
  actionsExecuted: number;
  pendingActions: number;
  successRate: number;
}
