import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { CustomerSuccessService } from "@/lib/customer-success-service";
import { toast } from "sonner";

export const CS_QUERY_KEYS = {
  healthSummary: ["cs", "health", "summary"],
  healthPaged: (riskLevel?: string, search?: string, page?: number, pageSize?: number) => [
    "cs", "health", "paged", riskLevel, search, page, pageSize
  ],
  healthCustomer: (customerId: string) => ["cs", "health", "customer", customerId],
  loyaltyPrograms: ["cs", "loyalty", "programs"],
  loyaltyCustomer: (customerId: string) => ["cs", "loyalty", "customer", customerId],
  loyaltyTransactions: (customerId?: string, programId?: string, page?: number) => [
    "cs", "loyalty", "transactions", customerId, programId, page
  ],
  referralsPaged: (status?: string, search?: string, page?: number) => [
    "cs", "referrals", "paged", status, search, page
  ],
  referralAnalytics: ["cs", "referrals", "analytics"],
  satisfactionSummary: (feedbackType?: string) => ["cs", "satisfaction", "summary", feedbackType],
  feedbackPaged: (customerId?: string, minRating?: number, page?: number) => [
    "cs", "satisfaction", "feedback", customerId, minRating, page
  ],
  tasksPaged: (status?: string, priority?: string, taskType?: string, page?: number) => [
    "cs", "tasks", "paged", status, priority, taskType, page
  ],
  segments: ["cs", "segments"],
  segmentMembers: (name: string) => ["cs", "segments", "members", name],
  retentionOverview: ["cs", "retention", "overview"]
};

// ── Health Score Hooks ─────────────────────────────────────────────────────────

export function useCustomerHealthSummary() {
  return useQuery({
    queryKey: CS_QUERY_KEYS.healthSummary,
    queryFn: () => CustomerSuccessService.getHealthSummary(),
    staleTime: 1000 * 60 * 5,
  });
}

export function useCustomerHealthPaged(params?: { riskLevel?: string; search?: string; page?: number; pageSize?: number }) {
  return useQuery({
    queryKey: CS_QUERY_KEYS.healthPaged(params?.riskLevel, params?.search, params?.page, params?.pageSize),
    queryFn: () => CustomerSuccessService.getHealthPaged(params),
    staleTime: 1000 * 60 * 2,
  });
}

export function useCustomerHealthByCustomer(customerId: string) {
  return useQuery({
    queryKey: CS_QUERY_KEYS.healthCustomer(customerId),
    queryFn: () => CustomerSuccessService.getHealthByCustomer(customerId),
    enabled: !!customerId,
  });
}

export function useRecalculateAllHealth() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: () => CustomerSuccessService.recalculateAllHealth(),
    onSuccess: (data) => {
      toast.success(data.message);
      queryClient.invalidateQueries({ queryKey: ["cs"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to recalculate health scores");
    }
  });
}

// ── Loyalty & Rewards Hooks ────────────────────────────────────────────────────

export function useLoyaltyPrograms() {
  return useQuery({
    queryKey: CS_QUERY_KEYS.loyaltyPrograms,
    queryFn: () => CustomerSuccessService.getLoyaltyPrograms(),
  });
}

export function useCustomerLoyalty(customerId: string) {
  return useQuery({
    queryKey: CS_QUERY_KEYS.loyaltyCustomer(customerId),
    queryFn: () => CustomerSuccessService.getCustomerLoyalty(customerId),
    enabled: !!customerId,
  });
}

export function useLoyaltyTransactions(params?: { customerId?: string; programId?: string; page?: number; pageSize?: number }) {
  return useQuery({
    queryKey: CS_QUERY_KEYS.loyaltyTransactions(params?.customerId, params?.programId, params?.page),
    queryFn: () => CustomerSuccessService.getLoyaltyTransactions(params),
  });
}

export function useCreateLoyaltyProgram() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: { name: string; description?: string; pointsPerPurchase: number; minimumRedemptionPoints: number; pointsExpiryDays?: number; isDefault: boolean }) =>
      CustomerSuccessService.createLoyaltyProgram(data),
    onSuccess: () => {
      toast.success("Loyalty program created successfully");
      queryClient.invalidateQueries({ queryKey: CS_QUERY_KEYS.loyaltyPrograms });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to create loyalty program");
    }
  });
}

export function useEarnLoyaltyPoints() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: { customerId: string; programId?: string; purchaseAmount: number; description?: string }) =>
      CustomerSuccessService.earnPoints(data),
    onSuccess: (tx) => {
      toast.success(`Earned ${tx.pointsEarned} loyalty points!`);
      queryClient.invalidateQueries({ queryKey: ["cs", "loyalty"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to earn points");
    }
  });
}

export function useRedeemLoyaltyPoints() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: { customerId: string; programId?: string; pointsToRedeem: number; description?: string }) =>
      CustomerSuccessService.redeemPoints(data),
    onSuccess: (tx) => {
      toast.success(`Redeemed ${tx.pointsRedeemed} points successfully!`);
      queryClient.invalidateQueries({ queryKey: ["cs", "loyalty"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to redeem points");
    }
  });
}

export function useAdjustLoyaltyPoints() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: { customerId: string; programId?: string; pointsDelta: number; reason: string }) =>
      CustomerSuccessService.adjustPoints(data),
    onSuccess: () => {
      toast.success("Loyalty point balance adjusted");
      queryClient.invalidateQueries({ queryKey: ["cs", "loyalty"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to adjust points");
    }
  });
}

// ── Referral Hooks ─────────────────────────────────────────────────────────────

export function useReferralAnalytics() {
  return useQuery({
    queryKey: CS_QUERY_KEYS.referralAnalytics,
    queryFn: () => CustomerSuccessService.getReferralAnalytics(),
  });
}

export function useReferralsPaged(params?: { status?: string; search?: string; page?: number; pageSize?: number }) {
  return useQuery({
    queryKey: CS_QUERY_KEYS.referralsPaged(params?.status, params?.search, params?.page),
    queryFn: () => CustomerSuccessService.getReferralsPaged(params),
  });
}

export function useCreateReferral() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: { referrerCustomerId: string; rewardAmount: number; notes?: string }) =>
      CustomerSuccessService.createReferral(data),
    onSuccess: (ref) => {
      toast.success(`Referral code generated: ${ref.referralCode}`);
      queryClient.invalidateQueries({ queryKey: ["cs", "referrals"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to create referral");
    }
  });
}

export function useConvertReferral() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: { referralCode: string; referredCustomerId: string }) =>
      CustomerSuccessService.convertReferral(data),
    onSuccess: () => {
      toast.success("Referral converted & reward credited!");
      queryClient.invalidateQueries({ queryKey: ["cs", "referrals"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to convert referral");
    }
  });
}

// ── Customer Satisfaction Hooks ────────────────────────────────────────────────

export function useSatisfactionSummary(feedbackType?: string) {
  return useQuery({
    queryKey: CS_QUERY_KEYS.satisfactionSummary(feedbackType),
    queryFn: () => CustomerSuccessService.getSatisfactionSummary(feedbackType),
  });
}

export function useFeedbackPaged(params?: { customerId?: string; minRating?: number; maxRating?: number; feedbackType?: string; page?: number; pageSize?: number }) {
  return useQuery({
    queryKey: CS_QUERY_KEYS.feedbackPaged(params?.customerId, params?.minRating, params?.page),
    queryFn: () => CustomerSuccessService.getFeedbackPaged(params),
  });
}

export function useSubmitFeedback() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: { customerId: string; rating: number; feedback?: string; channel?: string; feedbackType?: string }) =>
      CustomerSuccessService.submitFeedback(data),
    onSuccess: () => {
      toast.success("Customer feedback submitted successfully");
      queryClient.invalidateQueries({ queryKey: ["cs", "satisfaction"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to submit feedback");
    }
  });
}

// ── Success Tasks Hooks ────────────────────────────────────────────────────────

export function useSuccessTasksPaged(params?: { customerId?: string; assignedUserId?: string; status?: string; priority?: string; taskType?: string; page?: number; pageSize?: number }) {
  return useQuery({
    queryKey: CS_QUERY_KEYS.tasksPaged(params?.status, params?.priority, params?.taskType, params?.page),
    queryFn: () => CustomerSuccessService.getSuccessTasks(params),
  });
}

export function useCreateSuccessTask() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: { customerId: string; assignedUserId?: string; taskType: string; title: string; description?: string; dueDate?: string; priority: string }) =>
      CustomerSuccessService.createSuccessTask(data),
    onSuccess: () => {
      toast.success("Success task created");
      queryClient.invalidateQueries({ queryKey: ["cs", "tasks"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to create task");
    }
  });
}

export function useUpdateSuccessTask() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: { assignedUserId?: string; title: string; description?: string; dueDate?: string; priority: string; status: string } }) =>
      CustomerSuccessService.updateSuccessTask(id, data),
    onSuccess: () => {
      toast.success("Success task updated");
      queryClient.invalidateQueries({ queryKey: ["cs", "tasks"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to update task");
    }
  });
}

export function useAutoGenerateTasks() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: () => CustomerSuccessService.autoGenerateTasks(),
    onSuccess: (res) => {
      toast.success(res.message);
      queryClient.invalidateQueries({ queryKey: ["cs", "tasks"] });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to generate tasks");
    }
  });
}

// ── Segments & Retention Hooks ─────────────────────────────────────────────────

export function useCustomerSegments() {
  return useQuery({
    queryKey: CS_QUERY_KEYS.segments,
    queryFn: () => CustomerSuccessService.getSegments(),
  });
}

export function useCustomerSegmentMembers(segmentName: string) {
  return useQuery({
    queryKey: CS_QUERY_KEYS.segmentMembers(segmentName),
    queryFn: () => CustomerSuccessService.getSegmentMembers(segmentName),
    enabled: !!segmentName,
  });
}

export function useRecalculateSegments() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: () => CustomerSuccessService.recalculateSegments(),
    onSuccess: (data) => {
      toast.success(data.message);
      queryClient.invalidateQueries({ queryKey: CS_QUERY_KEYS.segments });
    },
    onError: (err: any) => {
      toast.error(err.response?.data?.message ?? "Failed to recalculate segments");
    }
  });
}

export function useRetentionOverview() {
  return useQuery({
    queryKey: CS_QUERY_KEYS.retentionOverview,
    queryFn: () => CustomerSuccessService.getRetentionOverview(),
  });
}
