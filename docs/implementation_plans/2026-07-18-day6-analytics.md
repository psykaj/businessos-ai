# Day 6 Frontend Implementation (QR Analytics Dashboard)

This document outlines the implementation plan for the Frontend Analytics Dashboard for BusinessOS AI.

## User Review Required

- **Backend Gap Identified**: The frontend prompt requires a detailed "Scan History Table" with search, sort, and pagination of individual scans (showing Device Type, OS, City, UTMs, etc.). The backend APIs implemented earlier today only provide aggregations (Timeline, Devices, Browsers) and an export. 
  - **Proposed Solution**: I will first add a `GET /api/analytics/history` endpoint to the backend `AnalyticsController` and `AnalyticsRepository` to support paginated fetching of individual `QRScan` records before building the frontend table.

## Proposed Changes

### 1. Backend Updates (Prerequisite)

#### [NEW] backend/Modules/Analytics/DTOs/ScanHistoryDto.cs
#### [MODIFY] backend/Modules/Analytics/Repositories/IAnalyticsRepository.cs
#### [MODIFY] backend/Modules/Analytics/Repositories/AnalyticsRepository.cs
#### [MODIFY] backend/Modules/Analytics/Controllers/AnalyticsController.cs
- Add `GetHistoryAsync(Guid organizationId, Guid? qrCodeId, string? search, int page, int pageSize, DateTime? startDate, DateTime? endDate)` to support the detailed history table.

### 2. Frontend Architecture & Types

#### [NEW] frontend/types/analytics.ts
- Define TypeScript interfaces matching the backend DTOs (`AnalyticsOverview`, `ScanTimeline`, `DeviceAnalytics`, `BrowserAnalytics`, `CountryAnalytics`, `ReferrerAnalytics`, `ScanHistory`).

#### [NEW] frontend/lib/analytics-service.ts
- Create `AnalyticsService` using `apiClient.ts` to fetch data from `/api/analytics/*`.

#### [NEW] frontend/components/providers/query-provider.tsx
#### [MODIFY] frontend/app/layout.tsx
- Integrate `@tanstack/react-query` to provide a robust caching and data-fetching layer across the dashboard.

### 3. Analytics Components

#### [NEW] frontend/components/analytics/kpi-cards.tsx
- Reusable KPI metric cards (Total Scans, Active QR Codes, etc.) utilizing `shadcn/ui` Card components.

#### [NEW] frontend/components/analytics/analytics-charts.tsx
- Create Recharts-based visualizations:
  - `ScanTrendChart` (AreaChart/LineChart)
  - `DeviceDistributionChart` (PieChart)
  - `BrowserUsageChart` (BarChart)
  - `LocationChart` (BarChart for Country Distribution)

#### [NEW] frontend/components/analytics/scan-history-table.tsx
- A data table displaying raw scan logs with pagination, searching, and filtering.

#### [NEW] frontend/components/analytics/date-range-picker.tsx
- A reusable date range selector that updates the URL search params to globally filter the dashboard.

### 4. Pages

#### [MODIFY] frontend/app/dashboard/analytics/page.tsx
- Assemble the Global Analytics Dashboard.
- Consume the React Query hooks to fetch aggregated data.
- Layout: KPI Cards at the top, followed by Scan Trend Chart, secondary charts (Devices, Browsers, Locations), and the Scan History Table at the bottom.

#### [NEW] frontend/app/dashboard/analytics/[qrId]/page.tsx
- A dedicated view for a single QR code.
- Reuses the KPI cards and charts but filtered down to a specific `qrId`.

## Verification Plan

### Automated Tests
- Verify backend builds successfully after adding the history endpoint.
- Verify frontend builds (`npm run build`) without TypeScript or ESLint errors.

### Manual Verification
- Navigate to `/dashboard/analytics`.
- Change date ranges and ensure all charts and KPIs update correctly.
- Ensure the Scan History table fetches paginated data and supports searching.
- Navigate to `/dashboard/analytics/[qrId]` and verify it shows data isolated to that specific QR code.
- Click the Export button to verify CSV generation triggers correctly.
- Test responsiveness on mobile and tablet views.
