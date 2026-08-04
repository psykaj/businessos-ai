# Day 25 Frontend: Business Performance Dashboard & AI Growth Center

We will design and implement an enterprise-grade, high-performance **Business Performance Dashboard & AI Growth Center** for BusinessOS AI using Next.js 15, React, TypeScript, Tailwind CSS, Shadcn UI components, Recharts, Framer Motion, and React Query.

Guided by the **Business Value Rule**, every screen moves beyond passive charts to deliver direct, actionable business intelligence: enabling users to instantly trigger automated workflows to **save time**, **increase revenue**, **reduce operational costs**, and **improve business decision-making**.

## User Review Required

> [!IMPORTANT]
> **Action-Oriented Intelligence**: Every chart and KPI card includes interactive action hooks (e.g. *"Reallocate $5,000 Ads Budget"*, *"Trigger VIP Customer Win-Back Sequence"*, *"Eliminate Negative Margin Loss Centers"*). When executed, the UI applies optimistic mutations via React Query and surfaces instant financial impact notifications using Sonner toasts.
> **Navigation Placement**: We will insert a high-visibility `"AI Growth & Performance"` group directly below `"Overview"` in the main app sidebar to make these 7 strategic analytical centers easily accessible to executive decision-makers.

## Open Questions

There are no blockers or critical open questions. We will implement rich fallback mock datasets representing an enterprise SaaS/E-Commerce company ($1.2M ARR, >124% NRR, 4.2x LTV/CAC) to ensure smooth local testing and immediate visual verification even when offline from the backend.

## Proposed Changes

We will organize our changes into reusable components, state management services & hooks, page routes, navigation updates, and comprehensive system documentation.
All git changes will be implemented on a dedicated feature branch: `feature/day25-business-performance` before any push.

### State & API Management

#### [NEW] [business-performance-service.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/lib/business-performance-service.ts)
- Defines TypeScript DTOs for `BusinessHealthScore`, `RevenueTrend`, `ProfitMarginItem`, `CustomerLtvCac`, `MarketingChannelRoas`, `GrowthRecommendationDto`, and `IndustryBenchmarkDto`.
- Configures Axios API endpoints targeting `/api/v1/business-performance`, `/api/v1/growth-center`, `/api/v1/revenue-analytics`, `/api/v1/customer-analytics`, `/api/v1/product-analytics`, `/api/v1/marketing-roi`, and `/api/v1/benchmarks`.
- Includes audit-grade fallback mock data with realistic executive numbers and simulation of interactive action triggers (e.g. resolving a recommendation or shifting budget).

#### [NEW] [use-business-performance.ts](file:///Users/pankajanilyadav/Documents/Simplify/frontend/hooks/use-business-performance.ts)
- Implements React Query hooks (`useBusinessPerformanceSummary`, `useGrowthRecommendations`, `useRevenueAnalytics`, `useCustomerAnalytics`, `useProductAnalytics`, `useMarketingRoi`, `useIndustryBenchmarks`) with optimized stale times and retry logic.
- Implements mutation hooks (`useExecuteGrowthAction`, `useReallocateMarketingBudget`, `useOptimizeStock`) with optimistic updates and financial lift toast confirmations.

---

### Reusable UI & Chart Components

#### [NEW] [kpi-action-card.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/business-performance/kpi-action-card.tsx)
- Reusable KPI display card featuring value formatting, trend indicator (+14.2% YoY), miniature trendline preview, and an embedded business action button/tooltip explaining immediate revenue or efficiency impact.

#### [NEW] [growth-opportunity-banner.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/business-performance/growth-opportunity-banner.tsx)
- Animated AI hero highlight banner highlighting the top prioritized revenue growth recommendation with confidence score and one-click execution.

#### [NEW] [revenue-trend-chart.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/business-performance/revenue-trend-chart.tsx)
- High-performance Recharts multi-series area and bar visualization supporting interactive toggles between MRR decomposition, net profit margin trends, and global region foundations (NA, EMEA, APAC, LATAM).

#### [NEW] [ltv-cac-matrix.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/business-performance/ltv-cac-matrix.tsx)
- Interactive cohort visualization comparing customer Lifetime Value against Acquisition Costs across user segments, flagging churn risk accounts with AI retention suggestions.

#### [NEW] [margin-ranking-table.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/business-performance/margin-ranking-table.tsx)
- Filterable product analytics table categorizing high-profit leaders versus low-performing loss centers, with interactive buttons to optimize inventory stock or repricing.

#### [NEW] [roas-channel-optimizer.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/business-performance/roas-channel-optimizer.tsx)
- Comparative channel ROI analysis charting ad spend versus return on ad spend (ROAS), complete with AI budget shifting recommendations.

#### [NEW] [benchmark-gauge.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/business-performance/benchmark-gauge.tsx)
- Visual quartile ranking gauge comparing real company metrics against SaaS and E-Commerce industry medians.

---

### Page Implementations

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/business-performance/page.tsx)
- **Business Performance Dashboard**: Master command page showing Revenue Growth, Profit Growth, Business Health Score (0-100), Customer Growth, Marketing ROI, Top Products, and AI Growth Opportunities.

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/growth-center/page.tsx)
- **AI Growth Center**: Prioritized AI recommendations engine displaying financial yield, confidence score bar, priority tags, and automated execution triggers for Campaign A budget increase, Product X reordering, VIP win-back calls, Plan upselling, and slow-moving stock reduction.

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/revenue-analytics/page.tsx)
- **Revenue & Profit Analytics**: Deep-dive analytics covering MRR expansion vs contraction, profit trend ledgers, revenue by product, customer revenue breakdown, geographic revenue foundations, and YoY monthly comparisons with drill-down modals.

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/customer-analytics/page.tsx)
- **Customer Analytics**: Highlights Customer Lifetime Value (LTV), Repeat Purchase Rate, Top Customers table, VIP churn risk radar, and high-margin AI upsell opportunities.

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/product-analytics/page.tsx)
- **Product Analytics**: Margin-ranked performance matrix displaying Top Products, Low Performing Products, gross margin yield, and AI inventory optimization flags.

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/marketing-roi/page.tsx)
- **Marketing ROI & Attribution**: Evaluates customer acquisition efficiency, CAC trajectory, channel ROAS rankings, and automated budget shifting simulations.

#### [NEW] [page.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/app/dashboard/benchmarks/page.tsx)
- **Industry Benchmarks**: Compares organization performance against SaaS/E-Commerce industry medians and top 25th percentile performers across NRR, LTV/CAC, and Gross Profit.

---

### Navigation & Roadmap

#### [MODIFY] [sidebar.tsx](file:///Users/pankajanilyadav/Documents/Simplify/frontend/components/layout/sidebar.tsx)
- Inserts `"AI Growth & Performance"` nav group with all 7 strategic dashboard links and proper icons.

#### [MODIFY] [roadmap.md](file:///Users/pankajanilyadav/Documents/Simplify/roadmap.md)
- Marks Frontend Implementation Status items completed:
  - Business Performance UI ✅
  - Growth Center UI ✅
  - Revenue Analytics UI ✅
  - AI Growth Dashboard ✅

---

### System Documentation

#### [NEW] [business-performance-ui.md](file:///Users/pankajanilyadav/Documents/Simplify/docs/business-performance-ui.md)
- Documents executive health calculations, KPI action cards, and responsive architecture.
#### [NEW] [growth-center-ui.md](file:///Users/pankajanilyadav/Documents/Simplify/docs/growth-center-ui.md)
- Documents AI recommendation prioritization algorithms, confidence scoring visualization, and mutation workflows.
#### [NEW] [revenue-dashboard-ui.md](file:///Users/pankajanilyadav/Documents/Simplify/docs/revenue-dashboard-ui.md)
- Documents MRR component decomposition charts, region breakdown foundations, and drill-down reports.
#### [NEW] [customer-product-analytics-ui.md](file:///Users/pankajanilyadav/Documents/Simplify/docs/customer-product-analytics-ui.md)
- Documents LTV/CAC analytical matrix, loss center detection displays, marketing ROI attribution, and industry benchmark integrations.

## Verification Plan

### Automated Tests
- Run Next.js production build check in `/Users/pankajanilyadav/Documents/Simplify/frontend`:
  ```bash
  cd /Users/pankajanilyadav/Documents/Simplify/frontend && npm run build
  ```
- Verify zero TypeScript compiler errors and ESLint warnings.

### Manual Verification
- Verify responsive dark/light theme rendering and animations across all 7 routes:
  - `/dashboard/business-performance`
  - `/dashboard/growth-center`
  - `/dashboard/revenue-analytics`
  - `/dashboard/customer-analytics`
  - `/dashboard/product-analytics`
  - `/dashboard/marketing-roi`
  - `/dashboard/benchmarks`
- Validate that clicking action buttons on the AI Growth Center and KPI cards triggers optimistic React Query updates and surfaces Sonner toast notifications detailing projected revenue or efficiency impact.
