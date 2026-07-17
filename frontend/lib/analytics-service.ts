import apiClient from "./api-client";
import {
  AnalyticsOverview,
  QRPerformance,
  ScanTimeline,
  DeviceAnalytics,
  BrowserAnalytics,
  CountryAnalytics,
  ReferrerAnalytics,
  ScanHistory,
  PagedResult,
} from "@/types/analytics";

export const analyticsService = {
  getOverview: async (startDate?: string, endDate?: string) => {
    const params = new URLSearchParams();
    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    const { data } = await apiClient.get<AnalyticsOverview>(`/api/analytics/overview?${params.toString()}`);
    return data;
  },

  getQRPerformance: async (qrCodeId: string, startDate?: string, endDate?: string) => {
    const params = new URLSearchParams();
    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    const { data } = await apiClient.get<QRPerformance>(`/api/analytics/qrcode/${qrCodeId}?${params.toString()}`);
    return data;
  },

  getTimeline: async (startDate?: string, endDate?: string) => {
    const params = new URLSearchParams();
    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    const { data } = await apiClient.get<ScanTimeline[]>(`/api/analytics/timeline?${params.toString()}`);
    return data;
  },

  getDevices: async (startDate?: string, endDate?: string) => {
    const params = new URLSearchParams();
    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    const { data } = await apiClient.get<DeviceAnalytics[]>(`/api/analytics/devices?${params.toString()}`);
    return data;
  },

  getBrowsers: async (startDate?: string, endDate?: string) => {
    const params = new URLSearchParams();
    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    const { data } = await apiClient.get<BrowserAnalytics[]>(`/api/analytics/browsers?${params.toString()}`);
    return data;
  },

  getCountries: async (startDate?: string, endDate?: string) => {
    const params = new URLSearchParams();
    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    const { data } = await apiClient.get<CountryAnalytics[]>(`/api/analytics/countries?${params.toString()}`);
    return data;
  },

  getReferrers: async (startDate?: string, endDate?: string) => {
    const params = new URLSearchParams();
    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    const { data } = await apiClient.get<ReferrerAnalytics[]>(`/api/analytics/referrers?${params.toString()}`);
    return data;
  },

  getHistory: async (
    qrCodeId?: string,
    search?: string,
    page: number = 1,
    pageSize: number = 10,
    startDate?: string,
    endDate?: string
  ) => {
    const params = new URLSearchParams();
    if (qrCodeId) params.append("qrCodeId", qrCodeId);
    if (search) params.append("search", search);
    params.append("page", page.toString());
    params.append("pageSize", pageSize.toString());
    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);

    const { data } = await apiClient.get<PagedResult<ScanHistory>>(`/api/analytics/history?${params.toString()}`);
    return data;
  },

  exportCSV: (startDate?: string, endDate?: string) => {
    const params = new URLSearchParams();
    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    
    // Instead of raw API client, we'll return the full URL so it can be opened in a new tab or trigger a download
    const token = typeof window !== "undefined" ? localStorage.getItem("businessos_token") : null;
    const baseUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5041";
    // NOTE: For a secure download with JWT, it's better to fetch as blob, then create a local URL.
    return apiClient.get(`/api/analytics/export?${params.toString()}`, { responseType: 'blob' });
  }
};
