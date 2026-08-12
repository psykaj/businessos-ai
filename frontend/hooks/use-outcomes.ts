import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { outcomesService, CreateOutcomeDto } from "@/lib/outcomes-service";
import { toast } from "sonner";

export function useOutcomes(params?: {
  sourceType?: string;
  sourceId?: string;
  page?: number;
  pageSize?: number;
}) {
  return useQuery({
    queryKey: ["outcomes", params],
    queryFn: () => outcomesService.getOutcomes(params),
  });
}

export function useRoiSummary(params?: {
  startDate?: string;
  endDate?: string;
  estimatedHourlyCost?: number;
}) {
  return useQuery({
    queryKey: ["outcomes", "roi-summary", params],
    queryFn: () => outcomesService.getRoiSummary(params),
  });
}

export function useCreateOutcome() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateOutcomeDto) => outcomesService.createOutcome(data),
    onSuccess: () => {
      toast("Outcome Recorded", {
        description: "The business outcome has been successfully recorded.",
      });
      // Invalidate relevant queries
      queryClient.invalidateQueries({ queryKey: ["outcomes"] });
    },
    onError: (error: any) => {
      toast("Failed to Record Outcome", {
        description: error?.response?.data?.message || "An error occurred while saving the outcome.",
      });
    },
  });
}
