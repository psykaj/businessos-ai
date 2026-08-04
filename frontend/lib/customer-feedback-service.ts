import apiClient from "./api-client";
import { z } from "zod";

// ==========================================
// Zod Validation Schemas for CX Actions
// ==========================================

export const submitFeedbackSchema = z.object({
  customerName: z.string().min(2, "Customer Name must be at least 2 characters"),
  customerCompany: z.string().optional(),
  type: z.enum(["Complaint", "Inquiry", "Praise", "FeatureRequest"]),
  ratingValue: z.number().min(1).max(10).default(5),
  content: z.string().min(10, "Feedback details must be at least 10 characters for AI analysis"),
  channel: z.enum(["Web Widget", "In-App Survey", "Support Email", "Executive Interception"]).default("Web Widget"),
  isUrgent: z.boolean().default(false),
});
export type SubmitFeedbackInput = z.infer<typeof submitFeedbackSchema>;

export const createSurveySchema = z.object({
  title: z.string().min(5, "Survey title must be at least 5 characters"),
  description: z.string().min(10, "Description must clearly explain the feedback incentive"),
  surveyType: z.enum(["CSAT", "NPS", "CES"]),
  targetAudience: z.string().default("All Active Accounts"),
});
export type CreateSurveyInput = z.infer<typeof createSurveySchema>;

export const addNoteSchema = z.object({
  note: z.string().min(5, "Internal note must contain actionable context"),
});
export type AddNoteInput = z.infer<typeof addNoteSchema>;

export const assignFeedbackSchema = z.object({
  assigneeName: z.string().min(2, "Please select an active CSM or Support Lead"),
});
export type AssignFeedbackInput = z.infer<typeof assignFeedbackSchema>;

// ==========================================
// DTO Interfaces & Domain Models
// ==========================================

export type FeedbackStatus = "New" | "InReview" | "Assigned" | "Resolved" | "Closed";
export type SentimentType = "Positive" | "Neutral" | "Negative";
export type ChurnRiskCohort = "Low" | "Medium" | "High" | "Critical";

export interface FeedbackItemDto {
  id: string;
  organizationId: string;
  customerId: string;
  customerName: string;
  customerCompany: string;
  type: "Complaint" | "Inquiry" | "Praise" | "FeatureRequest";
  ratingValue: number;
  content: string;
  channel: string;
  status: FeedbackStatus;
  isUrgent: boolean;
  assignedToUserName?: string;
  internalNotes: string[];
  submittedAt: string;
  sentimentLabel: SentimentType;
  confidenceScore: number;
  estimatedArrImpact: number; // $ ARR associated with this customer interaction
}

export interface SurveyQuestionDto {
  id: string;
  questionText: string;
  questionType: "StarRating" | "NpsScale" | "MultipleChoice" | "Text";
  orderIndex: number;
  isRequired: boolean;
  options?: string[];
}

export interface SurveyCampaignDto {
  id: string;
  title: string;
  description: string;
  surveyType: "CSAT" | "NPS" | "CES";
  isActive: boolean;
  targetAudience: string;
  totalResponses: number;
  completionRate: number;
  averageScore: number;
  questions: SurveyQuestionDto[];
  createdAt: string;
}

export interface RatingSummaryDto {
  entityType: string;
  entityName: string;
  entityId: string;
  averageScore: number;
  totalCount: number;
  distribution: Record<number, number>; // e.g. 5: 120, 4: 35...
}

export interface ServiceSlaMetricDto {
  date: string;
  firstResponseTimeMin: number;
  resolutionTimeMin: number;
  fcrRate: number; // First Contact Resolution %
  reopenRate: number;
  targetFcrRate: number;
  targetResponseMin: number;
}

export interface ComplaintCategoryDto {
  category: string;
  count: number;
  urgency: "High" | "Medium" | "Low";
  percentage: number;
}

export interface SentimentAnalyticsDto {
  totalAnalyzed: number;
  positivePercentage: number;
  neutralPercentage: number;
  negativePercentage: number;
  topComplaints: ComplaintCategoryDto[];
  trendHistory: { month: string; positive: number; neutral: number; negative: number }[];
}

export interface ChurnRiskAccountDto {
  customerId: string;
  customerName: string;
  companyName: string;
  mrr: number;
  churnRisk: ChurnRiskCohort;
  repeatComplaintRate: number;
  lastCSAT: number;
  aiRecommendedAction: string;
}

export interface CsatDashboardDto {
  overallCsatPercentage: number;
  npsScore: number;
  resolutionSatisfactionRate: number;
  promotersPercentage: number;
  passivesPercentage: number;
  detractorsPercentage: number;
  totalArrProtected: number;
  churnRiskAccounts: ChurnRiskAccountDto[];
  monthlyNpsTrend: { month: string; nps: number; target: number }[];
}

export interface ImprovementRecommendationDto {
  id: string;
  title: string;
  category: "Contact Unhappy Customers" | "Improve Response Time" | "Reward Loyal Customers" | "Follow Up After Purchase" | "Improve Product Quality";
  priority: "Urgent" | "High" | "Medium";
  estimatedArrImpact: number;
  rationale: string;
  suggestedAction: string;
  isCompleted: boolean;
}

// ==========================================
// Realistic Enterprise Mock Datasets (High ROI)
// ==========================================

let MOCK_FEEDBACKS: FeedbackItemDto[] = [
  {
    id: "fb-101",
    organizationId: "org-1",
    customerId: "cust-501",
    customerName: "Elena Vance (COO)",
    customerCompany: "VentureScale AI",
    type: "Complaint",
    ratingValue: 2,
    content: "Our API webhook latency spiked over 800ms during our peak sales event. If this isn't patched immediately, our enterprise renewal is off the table.",
    channel: "Executive Interception",
    status: "New",
    isUrgent: true,
    assignedToUserName: "Marcus Sterling (VP Tech)",
    internalNotes: ["Escalated directly to Infrastructure team. Patch deploying at 02:00 UTC."],
    submittedAt: new Date(Date.now() - 1000 * 60 * 45).toISOString(),
    sentimentLabel: "Negative",
    confidenceScore: 98.4,
    estimatedArrImpact: 48500,
  },
  {
    id: "fb-102",
    organizationId: "org-1",
    customerId: "cust-502",
    customerName: "David Thorne (DevOps Lead)",
    customerCompany: "Hyperion FinTech",
    type: "Praise",
    ratingValue: 5,
    content: "The new automated invoice reconciliation saved my financial operations staff over 25 hours this month alone. Flawless execution and zero errors!",
    channel: "In-App Survey",
    status: "Resolved",
    isUrgent: false,
    assignedToUserName: "Chloe Bennett (CSM)",
    internalNotes: ["Flagged as VIP Brand Promoter! Triggered referral incentive package."],
    submittedAt: new Date(Date.now() - 1000 * 3600 * 3).toISOString(),
    sentimentLabel: "Positive",
    confidenceScore: 99.2,
    estimatedArrImpact: 120000,
  },
  {
    id: "fb-103",
    organizationId: "org-1",
    customerId: "cust-503",
    customerName: "Sarah Jenkins (Head of Marketing)",
    customerCompany: "Omnipark Retail",
    type: "FeatureRequest",
    ratingValue: 4,
    content: "Would love an automatic Slack notification whenever an Instagram DM lead exceeds $5,000 lifetime value in the CRM.",
    channel: "Web Widget",
    status: "InReview",
    isUrgent: false,
    assignedToUserName: "Unassigned",
    internalNotes: [],
    submittedAt: new Date(Date.now() - 1000 * 3600 * 8).toISOString(),
    sentimentLabel: "Neutral",
    confidenceScore: 88.0,
    estimatedArrImpact: 18400,
  },
  {
    id: "fb-104",
    organizationId: "org-1",
    customerId: "cust-504",
    customerName: "Michael Chang (VP Logistics)",
    customerCompany: "Global Freight Solutions",
    type: "Complaint",
    ratingValue: 1,
    content: "Billing dashboard is miscalculating tax exemptions on international currency transfers. We need an account manager on this today.",
    channel: "Support Email",
    status: "Assigned",
    isUrgent: true,
    assignedToUserName: "Alex Rivera (Senior CSM)",
    internalNotes: ["Reaching out via phone right now."],
    submittedAt: new Date(Date.now() - 1000 * 3600 * 14).toISOString(),
    sentimentLabel: "Negative",
    confidenceScore: 96.1,
    estimatedArrImpact: 75000,
  },
  {
    id: "fb-105",
    organizationId: "org-1",
    customerId: "cust-505",
    customerName: "Jessica Alroy (Founder)",
    customerCompany: "Alroy Wealth Management",
    type: "Inquiry",
    ratingValue: 4,
    content: "How do we export our quarterly customer sentiment audits in Excel format for our compliance board audit next week?",
    channel: "Web Widget",
    status: "Resolved",
    isUrgent: false,
    assignedToUserName: "Chloe Bennett (CSM)",
    internalNotes: ["Sent documentation link and guided through Export Center."],
    submittedAt: new Date(Date.now() - 1000 * 3600 * 24).toISOString(),
    sentimentLabel: "Neutral",
    confidenceScore: 91.5,
    estimatedArrImpact: 29000,
  }
];

let MOCK_SURVEYS: SurveyCampaignDto[] = [
  {
    id: "srv-201",
    title: "Q3 Enterprise Account Health & NPS Campaign",
    description: "Quarterly automated relationship evaluation targeting executive sponsors and power users.",
    surveyType: "NPS",
    isActive: true,
    targetAudience: "Tier-1 Enterprise ($25k+ ARR)",
    totalResponses: 142,
    completionRate: 78.4,
    averageScore: 54.0, // NPS score
    createdAt: new Date(Date.now() - 1000 * 3600 * 24 * 14).toISOString(),
    questions: [
      { id: "q-1", questionText: "How likely are you to recommend BusinessOS AI to an industry colleague or partner?", questionType: "NpsScale", orderIndex: 1, isRequired: true },
      { id: "q-2", questionText: "Which feature has generated the highest revenue ROI for your team this quarter?", questionType: "MultipleChoice", orderIndex: 2, isRequired: true, options: ["AI Automation Studio", "CRM & Pipeline Engine", "Communication Hub", "BI & Forecasting"] },
      { id: "q-3", questionText: "What single capability could we add to accelerate your team's workflow?", questionType: "Text", orderIndex: 3, isRequired: false }
    ]
  },
  {
    id: "srv-202",
    title: "Post-Support Ticket CSAT Satisfaction Pulse",
    description: "Triggered instantly upon ticket resolution to evaluate support agent responsiveness and solution quality.",
    surveyType: "CSAT",
    isActive: true,
    targetAudience: "Resolved Support Tickets (All Tiers)",
    totalResponses: 524,
    completionRate: 64.2,
    averageScore: 4.8, // 5 star scale
    createdAt: new Date(Date.now() - 1000 * 3600 * 24 * 30).toISOString(),
    questions: [
      { id: "q-10", questionText: "How would you rate your satisfaction with our resolution speed and professionalism?", questionType: "StarRating", orderIndex: 1, isRequired: true },
      { id: "q-11", questionText: "Was your technical inquiry fully resolved on the initial contact?", questionType: "MultipleChoice", orderIndex: 2, isRequired: true, options: ["Yes, resolved completely", "No, needed repeat follow-up"] }
    ]
  }
];

let MOCK_RECOMMENDATIONS: ImprovementRecommendationDto[] = [
  {
    id: "rec-301",
    title: "Intercept VentureScale AI & Global Freight ($123k ARR at Churn Risk)",
    category: "Contact Unhappy Customers",
    priority: "Urgent",
    estimatedArrImpact: 123500,
    rationale: "AI sentiment analysis detected critical billing and latency complaints from key decision-makers with negative NPS ratings.",
    suggestedAction: "Schedule a high-touch 15-minute Executive Sync with Marcus Sterling to verify patch deployment and apply a $1,000 SLA service credit.",
    isCompleted: false
  },
  {
    id: "rec-302",
    title: "Activate VIP Referral Program for Hyperion FinTech ($120k ARR Advocate)",
    category: "Reward Loyal Customers",
    priority: "High",
    estimatedArrImpact: 45000,
    rationale: "David Thorne gave consecutive 5-star reviews noting 25+ staff hours saved via automated invoice reconciliation.",
    suggestedAction: "Send customized VIP Partner Case Study invitation with a 15% revenue share commission link.",
    isCompleted: false
  },
  {
    id: "rec-303",
    title: "Implement Automated Support AI FAQ for Currency & Tax Settings",
    category: "Improve Product Quality",
    priority: "High",
    estimatedArrImpact: 32000,
    rationale: "Over 18.5% of neutral/negative support inquiries this month cluster around international tax exemption formulas in Accounting.",
    suggestedAction: "Enable Copilot contextual tool-tips inside the invoicing dashboard to eliminate repeat level-1 support ticket friction.",
    isCompleted: false
  },
  {
    id: "rec-304",
    title: "Automate 14-Day Onboarding Feedback Pulse for New Mid-Market Accounts",
    category: "Follow Up After Purchase",
    priority: "Medium",
    estimatedArrImpact: 19500,
    rationale: "Accounts without formal check-in points during their first 30 days show a 12% drop in workflow activation.",
    suggestedAction: "Deploy automated SMS and email pulse check via Communication Hub at day 14 of subscription activation.",
    isCompleted: false
  }
];

const MOCK_CSAT_DASHBOARD: CsatDashboardDto = {
  overallCsatPercentage: 94.6,
  npsScore: 62.0,
  resolutionSatisfactionRate: 98.1,
  promotersPercentage: 74.0,
  passivesPercentage: 14.0,
  detractorsPercentage: 12.0,
  totalArrProtected: 428900,
  monthlyNpsTrend: [
    { month: "Apr", nps: 48, target: 55 },
    { month: "May", nps: 53, target: 55 },
    { month: "Jun", nps: 58, target: 60 },
    { month: "Jul", nps: 64, target: 60 },
    { month: "Aug", nps: 62, target: 65 },
  ],
  churnRiskAccounts: [
    {
      customerId: "cust-501",
      customerName: "Elena Vance (COO)",
      companyName: "VentureScale AI",
      mrr: 4041,
      churnRisk: "Critical",
      repeatComplaintRate: 42.5,
      lastCSAT: 2.0,
      aiRecommendedAction: "Immediate VP Tech interception & latency SLA verification call."
    },
    {
      customerId: "cust-504",
      customerName: "Michael Chang (VP)",
      companyName: "Global Freight Solutions",
      mrr: 6250,
      churnRisk: "High",
      repeatComplaintRate: 28.0,
      lastCSAT: 1.0,
      aiRecommendedAction: "Assign dedicated billing specialist to correct currency exemption bug."
    },
    {
      customerId: "cust-509",
      customerName: "Robert Green (CTO)",
      companyName: "Apex Dynamics",
      mrr: 2150,
      churnRisk: "Medium",
      repeatComplaintRate: 15.0,
      lastCSAT: 3.5,
      aiRecommendedAction: "Send proactive performance optimization guide & invite to live Q&A webinar."
    }
  ]
};

const MOCK_SENTIMENT_ANALYTICS: SentimentAnalyticsDto = {
  totalAnalyzed: 1842,
  positivePercentage: 78.4,
  neutralPercentage: 14.1,
  negativePercentage: 7.5,
  topComplaints: [
    { category: "API Webhook Latency & Spike Tolerance", count: 24, urgency: "High", percentage: 38.0 },
    { category: "International Tax Exemption Calculation", count: 18, urgency: "High", percentage: 28.5 },
    { category: "Custom Report Export Formatting", count: 12, urgency: "Medium", percentage: 19.0 },
    { category: "User Role Permission Granularity", count: 9, urgency: "Low", percentage: 14.5 }
  ],
  trendHistory: [
    { month: "Apr", positive: 68, neutral: 22, negative: 10 },
    { month: "May", positive: 72, neutral: 19, negative: 9 },
    { month: "Jun", positive: 75, neutral: 17, negative: 8 },
    { month: "Jul", positive: 79, neutral: 15, negative: 6 },
    { month: "Aug", positive: 78, neutral: 14, negative: 8 }
  ]
};

const MOCK_SERVICE_SLAS: ServiceSlaMetricDto[] = [
  { date: "Monday", firstResponseTimeMin: 4.2, resolutionTimeMin: 34.5, fcrRate: 88.5, reopenRate: 3.2, targetFcrRate: 85.0, targetResponseMin: 5.0 },
  { date: "Tuesday", firstResponseTimeMin: 3.8, resolutionTimeMin: 28.0, fcrRate: 91.2, reopenRate: 2.1, targetFcrRate: 85.0, targetResponseMin: 5.0 },
  { date: "Wednesday", firstResponseTimeMin: 5.4, resolutionTimeMin: 42.1, fcrRate: 82.0, reopenRate: 4.5, targetFcrRate: 85.0, targetResponseMin: 5.0 },
  { date: "Thursday", firstResponseTimeMin: 3.2, resolutionTimeMin: 25.4, fcrRate: 94.0, reopenRate: 1.8, targetFcrRate: 85.0, targetResponseMin: 5.0 },
  { date: "Friday", firstResponseTimeMin: 3.6, resolutionTimeMin: 29.8, fcrRate: 90.5, reopenRate: 2.0, targetFcrRate: 85.0, targetResponseMin: 5.0 }
];

const MOCK_RATINGS_SUMMARY: RatingSummaryDto[] = [
  {
    entityType: "Product Module",
    entityName: "AI Automation Studio & Workflow Engine",
    entityId: "mod-auto-01",
    averageScore: 4.9,
    totalCount: 412,
    distribution: { 5: 380, 4: 25, 3: 5, 2: 2, 1: 0 }
  },
  {
    entityType: "Support Team",
    entityName: "Technical Enterprise Account CSMs",
    entityId: "team-csm-01",
    averageScore: 4.8,
    totalCount: 310,
    distribution: { 5: 275, 4: 25, 3: 8, 2: 1, 1: 1 }
  },
  {
    entityType: "Product Module",
    entityName: "Financial Accounting & Invoicing Suite",
    entityId: "mod-acct-01",
    averageScore: 4.6,
    totalCount: 198,
    distribution: { 5: 145, 4: 38, 3: 10, 2: 4, 1: 1 }
  },
  {
    entityType: "Service Velocity",
    entityName: "API Platform & Developer Webhooks",
    entityId: "srv-api-01",
    averageScore: 4.4,
    totalCount: 154,
    distribution: { 5: 98, 4: 36, 3: 12, 2: 5, 1: 3 }
  }
];

// ==========================================
// Service Implementation with Graceful Fallbacks
// ==========================================

export const CustomerFeedbackService = {
  // Feedback Engine
  async getFeedbacks(keyword?: string, status?: string, type?: string): Promise<{ items: FeedbackItemDto[]; total: number }> {
    try {
      const res = await apiClient.get<{ items: FeedbackItemDto[]; total: number }>("/api/v1/customer-feedback/search", {
        params: { keyword, status: status === "All" ? undefined : status, type: type === "All" ? undefined : type }
      });
      if (res.data?.items) return res.data;
    } catch {
      // Offline graceful fallback
    }
    let filtered = [...MOCK_FEEDBACKS];
    if (keyword) {
      const kw = keyword.toLowerCase();
      filtered = filtered.filter(f => f.customerName.toLowerCase().includes(kw) || f.customerCompany.toLowerCase().includes(kw) || f.content.toLowerCase().includes(kw));
    }
    if (status && status !== "All") filtered = filtered.filter(f => f.status === status);
    if (type && type !== "All") filtered = filtered.filter(f => f.type === type);
    return { items: filtered, total: filtered.length };
  },

  async submitFeedback(input: SubmitFeedbackInput): Promise<FeedbackItemDto> {
    try {
      const res = await apiClient.post<FeedbackItemDto>("/api/v1/customer-feedback/submit", input);
      if (res.data) return res.data;
    } catch {
      // Offline fallback simulation
    }
    const newFb: FeedbackItemDto = {
      id: `fb-sim-${Date.now()}`,
      organizationId: "org-1",
      customerId: "cust-sim",
      customerName: input.customerName,
      customerCompany: input.customerCompany || "Independent Account",
      type: input.type,
      ratingValue: input.ratingValue,
      content: input.content,
      channel: input.channel,
      status: "New",
      isUrgent: input.isUrgent || input.ratingValue <= 2 || input.type === "Complaint",
      assignedToUserName: "Unassigned",
      internalNotes: [],
      submittedAt: new Date().toISOString(),
      sentimentLabel: input.ratingValue >= 4 ? "Positive" : input.ratingValue <= 2 ? "Negative" : "Neutral",
      confidenceScore: 95.8,
      estimatedArrImpact: input.ratingValue <= 2 ? 25000 : 8500,
    };
    MOCK_FEEDBACKS = [newFb, ...MOCK_FEEDBACKS];
    return newFb;
  },

  async updateFeedbackStatus(id: string, status: FeedbackStatus): Promise<FeedbackItemDto | undefined> {
    try {
      const res = await apiClient.put<FeedbackItemDto>(`/api/v1/customer-feedback/${id}/status`, { status });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const index = MOCK_FEEDBACKS.findIndex(f => f.id === id);
    if (index !== -1) {
      MOCK_FEEDBACKS[index] = { ...MOCK_FEEDBACKS[index], status };
      return MOCK_FEEDBACKS[index];
    }
    return undefined;
  },

  async assignFeedback(id: string, assigneeName: string): Promise<FeedbackItemDto | undefined> {
    try {
      const res = await apiClient.put<FeedbackItemDto>(`/api/v1/customer-feedback/${id}/assign`, { assigneeName });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const index = MOCK_FEEDBACKS.findIndex(f => f.id === id);
    if (index !== -1) {
      MOCK_FEEDBACKS[index] = { ...MOCK_FEEDBACKS[index], assignedToUserName: assigneeName, status: "Assigned" };
      return MOCK_FEEDBACKS[index];
    }
    return undefined;
  },

  async addInternalNote(id: string, note: string): Promise<FeedbackItemDto | undefined> {
    try {
      const res = await apiClient.post<FeedbackItemDto>(`/api/v1/customer-feedback/${id}/notes`, { note });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const index = MOCK_FEEDBACKS.findIndex(f => f.id === id);
    if (index !== -1) {
      const updatedNotes = [...MOCK_FEEDBACKS[index].internalNotes, `[${new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}] ${note}`];
      MOCK_FEEDBACKS[index] = { ...MOCK_FEEDBACKS[index], internalNotes: updatedNotes };
      return MOCK_FEEDBACKS[index];
    }
    return undefined;
  },

  async exportCsv(): Promise<string> {
    // Simulates instantaneous CSV download link generation
    return "id,customer_name,company,type,rating,status,sentiment,arr_impact\nfb-101,Elena Vance,VentureScale AI,Complaint,2,New,Negative,48500\nfb-102,David Thorne,Hyperion FinTech,Praise,5,Resolved,Positive,120000";
  },

  // Surveys & Campaigns
  async getSurveys(): Promise<SurveyCampaignDto[]> {
    try {
      const res = await apiClient.get<SurveyCampaignDto[]>("/api/v1/surveys");
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    return MOCK_SURVEYS;
  },

  async createSurvey(input: CreateSurveyInput): Promise<SurveyCampaignDto> {
    try {
      const res = await apiClient.post<SurveyCampaignDto>("/api/v1/surveys", input);
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const newSurvey: SurveyCampaignDto = {
      id: `srv-${Date.now()}`,
      title: input.title,
      description: input.description,
      surveyType: input.surveyType,
      isActive: false,
      targetAudience: input.targetAudience,
      totalResponses: 0,
      completionRate: 0,
      averageScore: 0,
      questions: [
        {
          id: `q-${Date.now()}`,
          questionText: input.surveyType === "NPS" ? "How likely are you to recommend our platform to a colleague?" : "How satisfied are you with your overall interaction?",
          questionType: input.surveyType === "NPS" ? "NpsScale" : "StarRating",
          orderIndex: 1,
          isRequired: true
        }
      ],
      createdAt: new Date().toISOString()
    };
    MOCK_SURVEYS = [newSurvey, ...MOCK_SURVEYS];
    return newSurvey;
  },

  async updateSurveyQuestions(surveyId: string, questions: SurveyQuestionDto[]): Promise<SurveyCampaignDto | undefined> {
    try {
      const res = await apiClient.put<SurveyCampaignDto>(`/api/v1/surveys/${surveyId}/questions`, { questions });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const idx = MOCK_SURVEYS.findIndex(s => s.id === surveyId);
    if (idx !== -1) {
      MOCK_SURVEYS[idx] = { ...MOCK_SURVEYS[idx], questions };
      return MOCK_SURVEYS[idx];
    }
    return undefined;
  },

  async togglePublishSurvey(surveyId: string, isActive: boolean): Promise<SurveyCampaignDto | undefined> {
    try {
      const res = await apiClient.put<SurveyCampaignDto>(`/api/v1/surveys/${surveyId}/publish`, { isActive });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const idx = MOCK_SURVEYS.findIndex(s => s.id === surveyId);
    if (idx !== -1) {
      MOCK_SURVEYS[idx] = { ...MOCK_SURVEYS[idx], isActive };
      return MOCK_SURVEYS[idx];
    }
    return undefined;
  },

  // Ratings Summary
  async getRatingsSummary(): Promise<RatingSummaryDto[]> {
    try {
      const res = await apiClient.get<RatingSummaryDto[]>("/api/v1/ratings/summary/all/all");
      if (res.data && res.data.length > 0) return res.data;
    } catch {
      // ignore
    }
    return MOCK_RATINGS_SUMMARY;
  },

  // Service Quality SLA Monitor
  async getServiceQualityMetrics(): Promise<ServiceSlaMetricDto[]> {
    try {
      const res = await apiClient.get<ServiceSlaMetricDto[]>("/api/v1/service-quality/sla/daily");
      if (res.data && res.data.length > 0) return res.data;
    } catch {
      // ignore
    }
    return MOCK_SERVICE_SLAS;
  },

  // Sentiment Analytics
  async getSentimentAnalytics(): Promise<SentimentAnalyticsDto> {
    try {
      const res = await apiClient.get<SentimentAnalyticsDto>("/api/v1/sentiment/analytics");
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    return MOCK_SENTIMENT_ANALYTICS;
  },

  // CSAT Dashboard & Churn Cohorts
  async getCsatDashboard(): Promise<CsatDashboardDto> {
    try {
      const res = await apiClient.get<CsatDashboardDto>("/api/v1/csat-metrics/dashboard");
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    return MOCK_CSAT_DASHBOARD;
  },

  // AI Improvement Recommendations
  async getImprovementRecommendations(): Promise<ImprovementRecommendationDto[]> {
    try {
      const res = await apiClient.get<ImprovementRecommendationDto[]>("/api/v1/improvement-actions");
      if (res.data && res.data.length > 0) return res.data;
    } catch {
      // ignore
    }
    return MOCK_RECOMMENDATIONS;
  },

  async completeRecommendation(id: string): Promise<ImprovementRecommendationDto | undefined> {
    try {
      const res = await apiClient.post<ImprovementRecommendationDto>(`/api/v1/improvement-actions/${id}/execute`, {});
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const idx = MOCK_RECOMMENDATIONS.findIndex(r => r.id === id);
    if (idx !== -1) {
      MOCK_RECOMMENDATIONS[idx] = { ...MOCK_RECOMMENDATIONS[idx], isCompleted: true };
      return MOCK_RECOMMENDATIONS[idx];
    }
    return undefined;
  }
};
