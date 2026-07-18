import { Plan, Subscription, UsageMetrics, Invoice, PaymentHistory } from '../types/billing';
import apiClient from './api-client';

export const billingService = {
  getPlans: async (): Promise<Plan[]> => {
    const { data } = await apiClient.get<Plan[]>('/api/billing/plans');
    return data;
  },

  getSubscription: async (): Promise<Subscription> => {
    const { data } = await apiClient.get<Subscription>('/api/billing/subscription');
    return data;
  },

  getUsageMetrics: async (): Promise<UsageMetrics> => {
    const { data } = await apiClient.get<UsageMetrics>('/api/billing/usage');
    return data;
  },

  getInvoices: async (): Promise<Invoice[]> => {
    const { data } = await apiClient.get<Invoice[]>('/api/billing/invoices');
    return data;
  },

  getPaymentHistory: async (): Promise<PaymentHistory[]> => {
    const { data } = await apiClient.get<PaymentHistory[]>('/api/billing/payments');
    return data;
  },

  createRazorpayOrder: async (planId: string, billingCycle: 'monthly' | 'yearly'): Promise<{ orderId: string, amount: number, key: string }> => {
    const { data } = await apiClient.post<{ orderId: string, amount: number, key: string }>('/api/billing/checkout/razorpay', { planId, billingCycle });
    return data;
  },
  
  verifyRazorpayPayment: async (paymentId: string, orderId: string, signature: string): Promise<boolean> => {
    try {
      const { data } = await apiClient.post<{ success: boolean }>('/api/billing/verify', { paymentId, orderId, signature });
      return data.success;
    } catch {
      return false;
    }
  },
  
  cancelSubscription: async (): Promise<boolean> => {
    await apiClient.post('/api/billing/subscription/cancel');
    return true;
  },
  
  resumeSubscription: async (): Promise<boolean> => {
    await apiClient.post('/api/billing/subscription/resume');
    return true;
  }
};
