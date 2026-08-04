# Business Performance & AI Growth Center Frontend Architecture

## Executive Summary
The Business Performance Dashboard & AI Growth Center (`Day 25 Frontend`) delivers an enterprise command center inspired by **Microsoft Power BI**, **HubSpot Revenue Analytics**, and **Salesforce Revenue Intelligence**. Built upon Next.js 15, React 19, TypeScript, Tailwind CSS, Shadcn UI, React Query, and Recharts, it enforces the **Mandatory Business Value Rule**:

> **Every screen must:**
> - Save time
> - Increase revenue
> - Reduce operational costs
> - Improve business decision-making
> 
> *No passive dashboards: Every UI view provides interactive AI recommendations and one-click execution hooks.*

---

## Architectural Layout & Core Views
The system implements a dedicated navigation group in `sidebar.tsx` titled **"AI Growth & Performance"** mapping to 7 strategic page routes:

| Dashboard Route | Primary Business Objective | Key Reusable Components |
| :--- | :--- | :--- |
| `/dashboard/business-performance` | Executive overview of health score, ARR momentum, and gross margins | `KpiActionCard`, `GrowthOpportunityBanner`, `RevenueTrendChart`, `MarginRankingTable` |
| `/dashboard/growth-center` | Master AI strategy orchestrator prioritizing actions by revenue lift & confidence | Opportunity Vault Scorecard, Filter Vectors, Interactive AI Execution Cards |
| `/dashboard/revenue-analytics` | MRR component decomposition (New, Expansion, Contraction, Churned) & audits | `RevenueTrendChart`, Monthly Evolution Ledger, Audit-Grade Drilldown Dialog |
| `/dashboard/customer-analytics` | LTV : CAC unit economics, repeat purchase rates, and churn intervention | `LtvCacMatrix`, Proactive Win-Back Dispatchers |
| `/dashboard/product-analytics` | Gross margin ranking matrix and operating loss center detection/sunsetting | `MarginRankingTable`, Instant Reserve Capacity Reorder Hooks |
| `/dashboard/marketing-roi` | Channel ROAS evaluation and budget reallocation from broad networks to ABM | `RoasChannelOptimizer`, Interactive Spend Shifting |
| `/dashboard/benchmarks` | Cross-company quartile comparison against SaaS and E-Commerce medians | `BenchmarkGauge`, AI Consultant Advisory Prompts |

---

## Component Design System & Reusability

### 1. Actionable Executive KPI Card (`KpiActionCard`)
Located in `components/business-performance/kpi-action-card.tsx`, this component supersedes standard static metric boxes by embedding real-time trend velocity indicators (+/- percentage vs prior period), explicit financial impact badges (e.g. `+$44.5k/yr lift available`), and direct action execution triggers (`onActionClick`).

### 2. AI Hero Opportunity Banner (`GrowthOpportunityBanner`)
Located in `components/business-performance/growth-opportunity-banner.tsx`, this component highlights the primary high-ROI recommendation from the AI Growth Center, rendering algorithmic confidence progress bars, projected annual recurring revenue (ARR) lift, and a one-click execution button powered by Optimistic UI mutations.

### 3. Multi-Series Revenue & MRR Decomposition Engine (`RevenueTrendChart`)
Located in `components/business-performance/revenue-trend-chart.tsx`, this Recharts orchestrator allows instant view switching across:
- **Revenue & Net Profit**: Area chart comparing total yields vs operational target trajectories.
- **MRR Decomposition**: Stacked bar chart isolating compound subscription growth (`New` + `Expansion`) against contraction risks (`Contraction` + `Churned`).
- **Global Regions**: Geographic ARR distribution pie chart and YoY regional momentum ledger.

---

## State Management & Optimistic Mutations
State is managed via `@tanstack/react-query` through `hooks/use-business-performance.ts`, wrapping standard HTTP requests to `/api/v1/...` while maintaining audit-grade offline fallback datasets in `lib/business-performance-service.ts`.

### Automated Workflow Mutations:
- `useExecuteGrowthAction`: Dispatches execution sequences for campaign optimizations, win-back calls, and tier expansion offers.
- `useReallocateMarketingBudget`: Shifting advertising budget dollars out of sub-median channels directly into conversion leaders.
- `useOptimizeStockOrPricing`: Sunsetting negative-margin service tiers and generating purchase orders for infrastructure capacity reserves.
