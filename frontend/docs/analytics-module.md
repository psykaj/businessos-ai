# QR Analytics Frontend Module

This document outlines the architecture, components, and implementation details for the QR Analytics Frontend Module in BusinessOS AI.

## Frontend Architecture
The Analytics Module follows a Clean Architecture approach integrated with Next.js 15 App Router:
- **Presentation**: `app/dashboard/analytics/` handles routing and layout compilation.
- **Components**: `components/analytics/` contains isolated, reusable UI components (`kpi-cards`, `analytics-charts`, `scan-history-table`, `date-range-picker`).
- **Data Fetching (State Management)**: Built with **React Query** (`@tanstack/react-query`) to handle server state, caching, background fetching, and loading states seamlessly.
- **Service Layer**: `lib/analytics-service.ts` encapsulates the API communication logic utilizing the existing Axios `apiClient`.

## Component Structure
1. **`analytics/page.tsx`**: The main dashboard aggregating all KPI cards, four charts, and the scan history table.
2. **`analytics/[qrId]/page.tsx`**: A scoped view of the analytics dedicated to a single QR code. Includes placeholders for AI Insights.
3. **`KPICards`**: Displays top-level metrics (Total Scans, Unique Visitors, Scans Today, Active QR Codes).
4. **`AnalyticsCharts`**: Wraps `Recharts` to provide responsive and styled visualizations:
   - `ScanTrendChart` (Area Chart)
   - `DeviceDistributionChart` (Pie Chart)
   - `BrowserUsageChart` (Horizontal Bar Chart)
   - `LocationChart` (Horizontal Bar Chart)
5. **`ScanHistoryTable`**: A server-paginated data table showing chronological scan events with sorting, filtering, and search capabilities.
6. **`DateRangePicker`**: Updates URL Search Parameters to filter the dashboard globally (e.g., Last 7 Days, Last 30 Days).

## API Integration Flow
- The React components utilize `useQuery` hooks.
- Keys are strictly typed based on URL parameters (`startDate`, `endDate`, `search`, `page`).
- When a user changes the date range or page, the URL updates. React Query detects the changed query keys and automatically refetches the data using the `analytics-service`.
- Loading skeletons are conditionally rendered based on React Query's `isLoading` state.

## State Management
- **Server State**: Managed via React Query (`@tanstack/react-query`). 
- **URL State**: Used for Global filters (`startDate`, `endDate`, `search`, `page`). This ensures deep linkability and browser history compatibility.
- **Local State**: Minimal `useState` used for controlled input fields (like the search bar debouncing).
- **Authentication**: Managed via the existing `AuthContext` to restrict API calls only when `user` is available.

## Responsive Strategy
- The layout relies entirely on Tailwind CSS grid and flex utilities.
- `grid-cols-1 md:grid-cols-2 lg:grid-cols-4` ensures KPI cards wrap cleanly.
- Recharts `<ResponsiveContainer>` wraps every chart, ensuring SVG charts scale down seamlessly on mobile devices.
- Tables are contained in an `overflow-x-auto` wrapper to prevent horizontal layout breakage on small screens.

## Future Enhancements
- **PDF Export**: Implement client-side PDF generation using `jspdf`.
- **Advanced Filtering**: Support filtering history by specific browsers or countries.
- **AI Integration**: Replace the static AI Insights placeholder on the `[qrId]` page with dynamic ChatGPT/Claude generation based on the QR's analytics data.
