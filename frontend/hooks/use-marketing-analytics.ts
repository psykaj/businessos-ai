import { useQuery } from "@tanstack/react-query";
import api from "@/lib/api-client";

export interface MarketingDashboardMetrics {
  totalForms: number;
  totalSubmissions: number;
  conversionRate: string;
  bestCampaign: string;
  topLeadSource: string;
  dailyLeads: number;
  monthlyLeads: number;
}

export function useMarketingDashboardMetrics() {
  return useQuery({
    queryKey: ["marketing-analytics", "dashboard"],
    queryFn: async () => {
      const { data } = await api.get<MarketingDashboardMetrics>("/marketing/analytics/dashboard");
      return data;
    },
  });
}
