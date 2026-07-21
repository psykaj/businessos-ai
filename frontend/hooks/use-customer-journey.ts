import { useQuery } from "@tanstack/react-query";
import api from "@/lib/api-client";

export interface CustomerJourney {
  id: string;
  leadId: string;
  currentStage: string;
  previousStage: string;
  enteredAt: string;
}

export function useCustomerJourneyHistory(leadId: string) {
  return useQuery({
    queryKey: ["customer-journey", leadId],
    queryFn: async () => {
      const { data } = await api.get<CustomerJourney[]>(`/customer-journey/${leadId}/history`);
      return data;
    },
    enabled: !!leadId,
  });
}
