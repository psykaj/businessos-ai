export interface AnalyticsOverview {
  totalScans: number;
  uniqueScans: number;
  scansToday: number;
  activeQRCodes: number;
}

export interface QRPerformance {
  qrCodeId: string;
  qrCodeName: string;
  totalScans: number;
  uniqueScans: number;
  lastScanDate: string | null;
}

export interface ScanTimeline {
  date: string;
  scanCount: number;
}

export interface DeviceAnalytics {
  deviceType: string;
  count: number;
  percentage: number;
}

export interface BrowserAnalytics {
  browser: string;
  count: number;
  percentage: number;
}

export interface CountryAnalytics {
  country: string;
  count: number;
  percentage: number;
}

export interface ReferrerAnalytics {
  referrer: string;
  count: number;
  percentage: number;
}

export interface ScanHistory {
  id: string;
  qrCodeId: string;
  qrCodeName: string;
  scanDateTime: string;
  ipAddress: string | null;
  deviceType: string | null;
  browser: string | null;
  operatingSystem: string | null;
  country: string | null;
  city: string | null;
  referrer: string | null;
  utmCampaign: string | null;
}

export interface PagedResult<T> {
  items: T[];
  totalItems: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
