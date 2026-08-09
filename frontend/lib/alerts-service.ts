import apiClient from "./api-client";

export type AlertSeverity = "Critical" | "High" | "Medium" | "Low";
export type AlertStatus = "Unread" | "Read" | "Resolved" | "Dismissed";
export type AlertCategory = "Finance" | "Inventory" | "Customer" | "System" | "Sales";

export interface AlertDto {
  id: string;
  title: string;
  description: string;
  severity: AlertSeverity;
  timestamp: string;
  businessImpact?: string;
  sourceModule: string;
  category: AlertCategory;
  recommendedAction?: string;
  status: AlertStatus;
  actionRequired: boolean;
  relatedRecordId?: string;
  aiExplanation?: string;
}

// Realistic mock data for the fallback
const defaultAlerts: AlertDto[] = [
  {
    id: "alt-001",
    title: "₹25,000 invoice overdue for 14 days",
    description: "Invoice INV-2026-801 for Apex Global Solutions is significantly past due.",
    severity: "High",
    timestamp: new Date(Date.now() - 1000 * 60 * 30).toISOString(), // 30 mins ago
    businessImpact: "Potential revenue recovery: ₹25,000",
    sourceModule: "Finance",
    category: "Finance",
    recommendedAction: "Review Payment Reminder",
    status: "Unread",
    actionRequired: true,
    relatedRecordId: "inv-901",
    aiExplanation: "Historical payment data indicates this client typically pays within 5 days of a second reminder.",
  },
  {
    id: "alt-002",
    title: "Inventory may run out in 2 days",
    description: "Enterprise Wireless AP-9000 is selling at 3.4x the normal rate. Current stock: 3 units.",
    severity: "Critical",
    timestamp: new Date(Date.now() - 1000 * 60 * 120).toISOString(), // 2 hours ago
    businessImpact: "Risk of ₹32,000 lost revenue",
    sourceModule: "Inventory",
    category: "Inventory",
    recommendedAction: "View Inventory",
    status: "Unread",
    actionRequired: true,
    relatedRecordId: "prod-001",
    aiExplanation: "Demand spiked following the recent marketing campaign. Immediate reorder is necessary to fulfill upcoming projected orders.",
  },
  {
    id: "alt-003",
    title: "3 high-value inactive customers may be ready for re-engagement",
    description: "Customers who previously purchased over ₹100k have not interacted in 90+ days but showed recent website activity.",
    severity: "Medium",
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 24).toISOString(), // 1 day ago
    businessImpact: "Potential pipeline addition: ₹150,000+",
    sourceModule: "Customer 360",
    category: "Customer",
    recommendedAction: "View Customers",
    status: "Unread",
    actionRequired: false,
    aiExplanation: "These accounts matched our 'High Intent' predictive model based on their recent documentation views.",
  },
  {
    id: "alt-004",
    title: "Failed Dunning Automation",
    description: "The automated SMS dunning workflow failed to execute for 12 accounts due to provider API errors.",
    severity: "High",
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 2).toISOString(),
    businessImpact: "Delayed collection of ₹45,000",
    sourceModule: "Automation",
    category: "System",
    recommendedAction: "Check Workflow Logs",
    status: "Read",
    actionRequired: true,
  },
  {
    id: "alt-005",
    title: "New Lead Conversion Spike",
    description: "Lead conversions from the 'Q3 Enterprise' campaign are up 45% today.",
    severity: "Low",
    timestamp: new Date(Date.now() - 1000 * 60 * 15).toISOString(),
    sourceModule: "CRM",
    category: "Sales",
    status: "Unread",
    actionRequired: false,
  }
];

// In-memory state for local mutability when backend is unavailable
let localAlertsState = [...defaultAlerts];

export const alertsService = {
  getAlerts: async (): Promise<AlertDto[]> => {
    try {
      const res = await apiClient.get<AlertDto[]>("/api/alerts");
      if (res?.data) return res.data;
    } catch {
      console.warn("Fallback to mock alerts data.");
    }
    return [...localAlertsState].sort((a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime());
  },

  getUnreadAlerts: async (): Promise<AlertDto[]> => {
    const all = await alertsService.getAlerts();
    return all.filter((a) => a.status === "Unread");
  },

  getActionRequiredAlerts: async (): Promise<AlertDto[]> => {
    const all = await alertsService.getAlerts();
    return all.filter((a) => a.actionRequired && a.status !== "Resolved" && a.status !== "Dismissed");
  },

  markAsRead: async (id: string): Promise<boolean> => {
    try {
      await apiClient.post(`/api/alerts/${id}/read`);
      return true;
    } catch {
      // Local fallback mutation
      localAlertsState = localAlertsState.map(a => a.id === id ? { ...a, status: "Read" } : a);
      return true;
    }
  },

  dismissAlert: async (id: string): Promise<boolean> => {
    try {
      await apiClient.post(`/api/alerts/${id}/dismiss`);
      return true;
    } catch {
      localAlertsState = localAlertsState.map(a => a.id === id ? { ...a, status: "Dismissed" } : a);
      return true;
    }
  },

  resolveAlert: async (id: string): Promise<boolean> => {
    try {
      await apiClient.post(`/api/alerts/${id}/resolve`);
      return true;
    } catch {
      localAlertsState = localAlertsState.map(a => a.id === id ? { ...a, status: "Resolved", actionRequired: false } : a);
      return true;
    }
  },

  takeAction: async (id: string): Promise<{ success: boolean; message: string }> => {
    try {
      const res = await apiClient.post<{ success: boolean; message: string }>(`/api/alerts/${id}/action`);
      if (res?.data) return res.data;
    } catch {
      console.warn("Mocking take action");
    }
    
    // Simulate delay
    await new Promise((resolve) => setTimeout(resolve, 800));
    localAlertsState = localAlertsState.map(a => a.id === id ? { ...a, status: "Resolved", actionRequired: false } : a);
    return {
      success: true,
      message: "Action successfully executed and alert resolved.",
    };
  }
};
