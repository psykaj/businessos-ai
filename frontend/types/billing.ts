export type PlanTier = 'Free' | 'Starter' | 'Professional' | 'Enterprise';

export interface Plan {
  id: string;
  name: PlanTier;
  description: string;
  monthlyPrice: number;
  yearlyPrice: number;
  features: string[];
  limits: {
    qrs: number;
    scans: number;
    storageMB: number;
    aiCredits: number;
    apiRequests: number;
    teamMembers: number;
  };
  isPopular?: boolean;
}

export interface Subscription {
  id: string;
  planId: string;
  status: 'active' | 'past_due' | 'canceled' | 'trialing';
  currentPeriodEnd: string;
  cancelAtPeriodEnd: boolean;
  plan: Plan;
}

export interface UsageMetrics {
  qrs: { current: number; limit: number };
  scans: { current: number; limit: number };
  storageMB: { current: number; limit: number };
  aiCredits: { current: number; limit: number };
  apiRequests: { current: number; limit: number };
  teamMembers: { current: number; limit: number };
}

export interface Invoice {
  id: string;
  number: string;
  amount: number;
  status: 'paid' | 'open' | 'void' | 'uncollectible';
  createdAt: string;
  pdfUrl?: string;
}

export interface PaymentHistory {
  id: string;
  date: string;
  planName: string;
  amount: number;
  status: 'succeeded' | 'pending' | 'failed';
  transactionId: string;
  invoiceId?: string;
}
