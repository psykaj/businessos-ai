# Implementation Plan: Executive Decision Center Frontend

This plan covers the implementation of the Day 21 Frontend for BusinessOS AI, focusing on the Executive Decision Center, KPI Dashboards, and AI Insights.

## User Review Required
> [!IMPORTANT]  
> Please review the proposed folder structure and page hierarchy to ensure it aligns perfectly with your expectations for the Executive Decision Center.

## Proposed Changes

### 1. API Services & Hooks
Create the integration layer to communicate with the Day 21 Backend endpoints.
- **`lib/executive-service.ts`**: Contains typed Axios calls for `/api/v1/kpi`, `/api/v1/forecasting`, `/api/v1/executive-insights`, `/api/v1/business-goals`, `/api/v1/scorecards`, `/api/v1/business-health`, `/api/v1/benchmarks`, `/api/v1/ai-recommendations`, and `/api/v1/decision-center`.
- **`hooks/use-executive.ts`**: Contains all React Query hooks (`useBusinessHealth`, `useKpis`, `useForecasts`, `useAiRecommendations`, etc.) for seamless data fetching, caching, and mutation state handling.

### 2. UI Components
Build reusable UI components tailored for executive insights.
- **`components/executive/metric-card.tsx`**: A standard KPI card showing current value, trend, and mini-sparkline.
- **`components/executive/trend-chart.tsx`**: A Recharts-based area/line chart for historical KPI data.
- **`components/executive/forecast-chart.tsx`**: A Recharts-based chart displaying actuals alongside forecasted confidence intervals.
- **`components/executive/health-gauge.tsx`**: A visual indicator for the 0-100 Business Health Score.

### 3. Dashboard Pages
Implement the following routes under `app/dashboard/`:
- **`/executive`**: The centralized command center. Displays the overall Business Health Score, top-level revenue forecasts, goal progress summaries, and critical AI alerts.
- **`/kpis`**: The KPI Workspace. Allows users to view and filter KPIs, compare periods, drill into KPI history, and view benchmark comparisons.
- **`/forecasting`**: Forecast Dashboard showing Revenue, Expense, Profit, and Customer forecasts with confidence indicators.
- **`/business-goals`**: Tracks strategic goals and objective completions.
- **`/scorecards`**: Evaluates employee and department performance.
- **`/recommendations`**: AI Recommendations center, allowing executives to dismiss, complete, or apply prioritized recommendations.
- **`/business-health`**: Deep dive into the Financial, Operational, and Customer health sub-scores.

### 4. Navigation Updates
- **`components/layout/sidebar.tsx` (or similar)**: Update the main navigation to include links to the new Executive Decision Center pages.

### 5. Documentation
Generate the requested markdown documentation in the `docs/` folder:
- `docs/executive-dashboard-ui.md`
- `docs/kpi-ui.md`
- `docs/forecasting-ui.md`
- `docs/business-goals-ui.md`

## Verification Plan

### Automated Checks
- `npm run lint` / `npm run build` to ensure no TypeScript or ESLint errors.

### Manual Verification
- Verify that each of the 7 new dashboard pages renders correctly and uses React Query to fetch data.
- Verify Recharts visualizations are responsive and support dark mode.
- Ensure the AI Recommendation "Apply" interaction works seamlessly.
