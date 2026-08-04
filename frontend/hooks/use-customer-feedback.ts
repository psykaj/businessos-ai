import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  CustomerFeedbackService,
  SubmitFeedbackInput,
  CreateSurveyInput,
  FeedbackStatus,
  SurveyQuestionDto
} from "@/lib/customer-feedback-service";
import { toast } from "sonner";

export const customerFeedbackKeys = {
  all: ["customer-feedback"] as const,
  feedbacks: (keyword?: string, status?: string, type?: string) => [...customerFeedbackKeys.all, "list", { keyword, status, type }] as const,
  surveys: () => [...customerFeedbackKeys.all, "surveys"] as const,
  ratingsSummary: () => [...customerFeedbackKeys.all, "ratings-summary"] as const,
  serviceQuality: () => [...customerFeedbackKeys.all, "service-quality"] as const,
  sentimentAnalytics: () => [...customerFeedbackKeys.all, "sentiment-analytics"] as const,
  csatDashboard: () => [...customerFeedbackKeys.all, "csat-dashboard"] as const,
  recommendations: () => [...customerFeedbackKeys.all, "recommendations"] as const,
};

// ==========================================
// Feedback Queries & Mutations
// ==========================================

export function useFeedbackSearch(keyword?: string, status?: string, type?: string) {
  return useQuery({
    queryKey: customerFeedbackKeys.feedbacks(keyword, status, type),
    queryFn: () => CustomerFeedbackService.getFeedbacks(keyword, status, type),
    staleTime: 20_000,
  });
}

export function useSubmitFeedback() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (input: SubmitFeedbackInput) => CustomerFeedbackService.submitFeedback(input),
    onSuccess: (newFb) => {
      queryClient.invalidateQueries({ queryKey: customerFeedbackKeys.all });
      toast.success(`Feedback recorded! AI Sentiment classified as ${newFb.sentimentLabel} (Urgency: ${newFb.isUrgent ? "High" : "Standard"}).`);
    },
    onError: () => {
      toast.error("Failed to submit customer feedback.");
    },
  });
}

export function useUpdateFeedbackStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, status }: { id: string; status: FeedbackStatus }) =>
      CustomerFeedbackService.updateFeedbackStatus(id, status),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: customerFeedbackKeys.all });
      toast.success(`Feedback ticket marked as ${variables.status}.`);
    },
    onError: () => {
      toast.error("Failed to update ticket status.");
    },
  });
}

export function useAssignFeedback() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, assigneeName }: { id: string; assigneeName: string }) =>
      CustomerFeedbackService.assignFeedback(id, assigneeName),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: customerFeedbackKeys.all });
      toast.success(`Ticket assigned to ${variables.assigneeName}. SLA timer running!`);
    },
    onError: () => {
      toast.error("Failed to assign ticket.");
    },
  });
}

export function useAddFeedbackNote() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, note }: { id: string; note: string }) =>
      CustomerFeedbackService.addInternalNote(id, note),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: customerFeedbackKeys.all });
      toast.info("Internal triage note attached to customer record.");
    },
    onError: () => {
      toast.error("Failed to append note.");
    },
  });
}

export function useExportFeedbackCsv() {
  return useMutation({
    mutationFn: () => CustomerFeedbackService.exportCsv(),
    onSuccess: (csvContent) => {
      const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
      const link = document.createElement("a");
      const url = URL.createObjectURL(blob);
      link.setAttribute("href", url);
      link.setAttribute("download", `customer_feedback_${new Date().toISOString().slice(0, 10)}.csv`);
      link.style.visibility = "hidden";
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      toast.success("High-speed CSV export completed successfully!");
    },
    onError: () => {
      toast.error("CSV Export failed.");
    }
  });
}

// ==========================================
// Survey Builder & Campaign Hooks
// ==========================================

export function useSurveys() {
  return useQuery({
    queryKey: customerFeedbackKeys.surveys(),
    queryFn: () => CustomerFeedbackService.getSurveys(),
    staleTime: 30_000,
  });
}

export function useCreateSurvey() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (input: CreateSurveyInput) => CustomerFeedbackService.createSurvey(input),
    onSuccess: (survey) => {
      queryClient.invalidateQueries({ queryKey: customerFeedbackKeys.surveys() });
      toast.success(`Survey campaign "${survey.title}" initialized in draft state.`);
    },
    onError: () => {
      toast.error("Could not create survey campaign.");
    }
  });
}

export function useUpdateSurveyQuestions() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ surveyId, questions }: { surveyId: string; questions: SurveyQuestionDto[] }) =>
      CustomerFeedbackService.updateSurveyQuestions(surveyId, questions),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: customerFeedbackKeys.surveys() });
      toast.success("Survey questions & ordering saved!");
    },
    onError: () => {
      toast.error("Failed to update survey template.");
    }
  });
}

export function useTogglePublishSurvey() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ surveyId, isActive }: { surveyId: string; isActive: boolean }) =>
      CustomerFeedbackService.togglePublishSurvey(surveyId, isActive),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: customerFeedbackKeys.surveys() });
      toast.success(variables.isActive ? "Survey campaign published & active!" : "Survey campaign paused.");
    },
    onError: () => {
      toast.error("Failed to alter survey publishing state.");
    }
  });
}

// ==========================================
// Ratings, SLA Quality & Analytics Hooks
// ==========================================

export function useRatingsSummary() {
  return useQuery({
    queryKey: customerFeedbackKeys.ratingsSummary(),
    queryFn: () => CustomerFeedbackService.getRatingsSummary(),
    staleTime: 30_000,
  });
}

export function useServiceQualityMetrics() {
  return useQuery({
    queryKey: customerFeedbackKeys.serviceQuality(),
    queryFn: () => CustomerFeedbackService.getServiceQualityMetrics(),
    staleTime: 30_000,
  });
}

export function useSentimentAnalytics() {
  return useQuery({
    queryKey: customerFeedbackKeys.sentimentAnalytics(),
    queryFn: () => CustomerFeedbackService.getSentimentAnalytics(),
    staleTime: 30_000,
  });
}

export function useCsatDashboard() {
  return useQuery({
    queryKey: customerFeedbackKeys.csatDashboard(),
    queryFn: () => CustomerFeedbackService.getCsatDashboard(),
    staleTime: 30_000,
  });
}

// ==========================================
// AI Improvement Action Engine Hooks
// ==========================================

export function useImprovementRecommendations() {
  return useQuery({
    queryKey: customerFeedbackKeys.recommendations(),
    queryFn: () => CustomerFeedbackService.getImprovementRecommendations(),
    staleTime: 20_000,
  });
}

export function useCompleteRecommendation() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => CustomerFeedbackService.completeRecommendation(id),
    onSuccess: (res) => {
      queryClient.invalidateQueries({ queryKey: customerFeedbackKeys.recommendations() });
      queryClient.invalidateQueries({ queryKey: customerFeedbackKeys.csatDashboard() });
      if (res) {
        toast.success(`Action executed! Protected estimated $${res.estimatedArrImpact.toLocaleString()} in ARR.`);
      } else {
        toast.success("Recommendation action executed successfully!");
      }
    },
    onError: () => {
      toast.error("Failed to complete recommendation.");
    }
  });
}
