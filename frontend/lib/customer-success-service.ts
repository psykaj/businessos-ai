import apiClient from "./api-client";

// ── Types ──────────────────────────────────────────────────────────────────────

export interface CustomerHealth {
  id: string;
  customerId: string;
  customerName?: string;
  healthScore: number;
  riskLevel: "Healthy" | "Stable" | "Needs Attention" | "High Risk";
  lastPurchaseDate?: string;
  lastInteractionDate?: string;
  lifetimeValue: number;
  outstandingPayments: number;
  supportTicketCount: number;
  satisfactionRating?: number;
  referralCount: number;
  purchaseFrequency: number;
  calculatedAt: string;
}

export interface HealthSummary {
  totalCustomers: number;
  healthyCount: number;
  stableCount: number;
  needsAttentionCount: number;
  highRiskCount: number;
  averageHealthScore: number;
}

export interface LoyaltyProgram {
  id: string;
  name: string;
  description?: string;
  status: "Active" | "Draft" | "Archived";
  pointsPerPurchase: number;
  minimumRedemptionPoints: number;
  pointsExpiryDays?: number;
  isDefault: boolean;
  createdAt: string;
}

export interface LoyaltyTransaction {
  id: string;
  customerId: string;
  customerName?: string;
  programId: string;
  programName?: string;
  pointsEarned: number;
  pointsRedeemed: number;
  balance: number;
  transactionType: "Earned" | "Redeemed" | "Adjusted" | "Expired";
  description?: string;
  expiryDate?: string;
  createdAt: string;
}

export interface CustomerLoyaltySummary {
  customerId: string;
  totalBalance: number;
  totalEarned: number;
  totalRedeemed: number;
  recentTransactions: LoyaltyTransaction[];
}

export interface Referral {
  id: string;
  referrerCustomerId: string;
  referrerName?: string;
  referredCustomerId?: string;
  referredName?: string;
  referralCode: string;
  status: "Pending" | "Converted" | "Rewarded" | "Expired";
  rewardIssued: boolean;
  rewardAmount: number;
  notes?: string;
  createdAt: string;
}

export interface ReferralAnalytics {
  totalReferrals: number;
  pendingCount: number;
  convertedCount: number;
  rewardedCount: number;
  totalRewardsIssued: number;
  conversionRatePercentage: number;
}

export interface CustomerFeedback {
  id: string;
  customerId: string;
  customerName?: string;
  rating: number;
  feedback?: string;
  channel: string;
  feedbackType: string;
  submittedAt: string;
}

export interface SatisfactionSummary {
  averageRating: number;
  totalSubmissions: number;
  ratingDistribution: Record<number, number>;
  positiveFeedbackPercentage: number;
  negativeFeedbackPercentage: number;
}

export interface SuccessTask {
  id: string;
  customerId: string;
  customerName?: string;
  assignedUserId?: string;
  assignedUserName?: string;
  taskType: "FollowUpInactive" | "ContactDissatisfied" | "WelcomeNew" | "CongratulateMilestone" | "UpsellHighValue" | "Manual";
  title: string;
  description?: string;
  dueDate?: string;
  priority: "Low" | "Medium" | "High" | "Critical";
  status: "Pending" | "InProgress" | "Completed" | "Cancelled";
  createdAt: string;
}

export interface CustomerSegment {
  id: string;
  name: string;
  segmentType: "Automated" | "Custom";
  criteriaJson?: string;
  customerCount: number;
  updatedAt: string;
}

export interface CustomerSegmentMember {
  customerId: string;
  customerName: string;
  email: string;
  segmentName: string;
  lifetimeValue: number;
  healthScore: number;
  riskLevel: string;
}

export interface RetentionActionItem {
  actionType: string;
  description: string;
  recommendedPriority: string;
  affectedCustomerCount: number;
}

export interface RetentionOverview {
  retentionRatePercentage: number;
  churnRatePercentage: number;
  atRiskCount: number;
  totalActiveCustomers: number;
  revenueAtRisk: number;
  recommendedActions: RetentionActionItem[];
}

export interface PagedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

// ── API Methods ────────────────────────────────────────────────────────────────

export const CustomerSuccessService = {
  // Health
  getHealthSummary: async (): Promise<HealthSummary> => {
    const res = await apiClient.get("/api/customer-success/health/summary");
    return res.data;
  },
  getHealthPaged: async (params?: { riskLevel?: string; search?: string; page?: number; pageSize?: number; sortBy?: string; descending?: boolean }): Promise<PagedResponse<CustomerHealth>> => {
    const res = await apiClient.get("/api/customer-success/health", { params });
    return res.data;
  },
  getHealthByCustomer: async (customerId: string): Promise<CustomerHealth> => {
    const res = await apiClient.get(`/api/customer-success/health/customer/${customerId}`);
    return res.data;
  },
  calculateHealth: async (customerId: string): Promise<CustomerHealth> => {
    const res = await apiClient.post(`/api/customer-success/health/calculate/${customerId}`);
    return res.data;
  },
  recalculateAllHealth: async (): Promise<{ message: string }> => {
    const res = await apiClient.post("/api/customer-success/health/recalculate-all");
    return res.data;
  },

  // Loyalty
  getLoyaltyPrograms: async (): Promise<LoyaltyProgram[]> => {
    const res = await apiClient.get("/api/customer-success/loyalty/programs");
    return res.data;
  },
  createLoyaltyProgram: async (data: { name: string; description?: string; pointsPerPurchase: number; minimumRedemptionPoints: number; pointsExpiryDays?: number; isDefault: boolean }): Promise<LoyaltyProgram> => {
    const res = await apiClient.post("/api/customer-success/loyalty/programs", data);
    return res.data;
  },
  earnPoints: async (data: { customerId: string; programId?: string; purchaseAmount: number; description?: string }): Promise<LoyaltyTransaction> => {
    const res = await apiClient.post("/api/customer-success/loyalty/earn", data);
    return res.data;
  },
  redeemPoints: async (data: { customerId: string; programId?: string; pointsToRedeem: number; description?: string }): Promise<LoyaltyTransaction> => {
    const res = await apiClient.post("/api/customer-success/loyalty/redeem", data);
    return res.data;
  },
  adjustPoints: async (data: { customerId: string; programId?: string; pointsDelta: number; reason: string }): Promise<LoyaltyTransaction> => {
    const res = await apiClient.post("/api/customer-success/loyalty/adjust", data);
    return res.data;
  },
  getCustomerLoyalty: async (customerId: string): Promise<CustomerLoyaltySummary> => {
    const res = await apiClient.get(`/api/customer-success/loyalty/customer/${customerId}`);
    return res.data;
  },
  getLoyaltyTransactions: async (params?: { customerId?: string; programId?: string; page?: number; pageSize?: number }): Promise<PagedResponse<LoyaltyTransaction>> => {
    const res = await apiClient.get("/api/customer-success/loyalty/transactions", { params });
    return res.data;
  },

  // Referrals
  createReferral: async (data: { referrerCustomerId: string; rewardAmount: number; notes?: string }): Promise<Referral> => {
    const res = await apiClient.post("/api/customer-success/referrals", data);
    return res.data;
  },
  getReferralsPaged: async (params?: { status?: string; search?: string; page?: number; pageSize?: number }): Promise<PagedResponse<Referral>> => {
    const res = await apiClient.get("/api/customer-success/referrals", { params });
    return res.data;
  },
  getReferralAnalytics: async (): Promise<ReferralAnalytics> => {
    const res = await apiClient.get("/api/customer-success/referrals/analytics");
    return res.data;
  },
  convertReferral: async (data: { referralCode: string; referredCustomerId: string }): Promise<Referral> => {
    const res = await apiClient.post("/api/customer-success/referrals/convert", data);
    return res.data;
  },

  // Satisfaction
  submitFeedback: async (data: { customerId: string; rating: number; feedback?: string; channel?: string; feedbackType?: string }): Promise<CustomerFeedback> => {
    const res = await apiClient.post("/api/customer-success/satisfaction", data);
    return res.data;
  },
  getSatisfactionSummary: async (feedbackType?: string): Promise<SatisfactionSummary> => {
    const res = await apiClient.get("/api/customer-success/satisfaction/summary", { params: { feedbackType } });
    return res.data;
  },
  getFeedbackPaged: async (params?: { customerId?: string; minRating?: number; maxRating?: number; feedbackType?: string; page?: number; pageSize?: number }): Promise<PagedResponse<CustomerFeedback>> => {
    const res = await apiClient.get("/api/customer-success/satisfaction/feedback", { params });
    return res.data;
  },

  // Success Tasks
  getSuccessTasks: async (params?: { customerId?: string; assignedUserId?: string; status?: string; priority?: string; taskType?: string; page?: number; pageSize?: number }): Promise<PagedResponse<SuccessTask>> => {
    const res = await apiClient.get("/api/customer-success/tasks", { params });
    return res.data;
  },
  createSuccessTask: async (data: { customerId: string; assignedUserId?: string; taskType: string; title: string; description?: string; dueDate?: string; priority: string }): Promise<SuccessTask> => {
    const res = await apiClient.post("/api/customer-success/tasks", data);
    return res.data;
  },
  updateSuccessTask: async (id: string, data: { assignedUserId?: string; title: string; description?: string; dueDate?: string; priority: string; status: string }): Promise<SuccessTask> => {
    const res = await apiClient.put(`/api/customer-success/tasks/${id}`, data);
    return res.data;
  },
  autoGenerateTasks: async (): Promise<{ message: string }> => {
    const res = await apiClient.post("/api/customer-success/tasks/auto-generate");
    return res.data;
  },

  // Segments
  getSegments: async (): Promise<CustomerSegment[]> => {
    const res = await apiClient.get("/api/customer-success/segments");
    return res.data;
  },
  getSegmentMembers: async (segmentName: string): Promise<CustomerSegmentMember[]> => {
    const res = await apiClient.get(`/api/customer-success/segments/members/${encodeURIComponent(segmentName)}`);
    return res.data;
  },
  recalculateSegments: async (): Promise<{ message: string }> => {
    const res = await apiClient.post("/api/customer-success/segments/recalculate");
    return res.data;
  },

  // Retention Overview
  getRetentionOverview: async (): Promise<RetentionOverview> => {
    const res = await apiClient.get("/api/customer-success/retention/overview");
    return res.data;
  }
};
