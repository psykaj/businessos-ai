# BusinessOS AI – AI Business Intelligence Engine & Dashboard

The **AI Business Intelligence (BI) Engine & Dashboard** in BusinessOS AI provides automated, real-time diagnostic analytics across all core enterprise operations, aggregating metrics from CRM, Marketing Automation, Workflows, Billing, Inventory, Cash Flow, and AI Usage into proactive, one-click decision workflows.

---

## 🎨 Frontend Architecture & Executive Command Dashboard

Built on **Next.js 16 (App Router)**, **React 19**, **TypeScript**, **TailwindCSS**, **Recharts**, and **TanStack Query v5**, the frontend delivers an interactive executive suite at `/business-intelligence` and `/dashboard/business-intelligence`:

- **Business Health Gauge (`BusinessHealthCard`)**: Calculates an aggregated **0–100 Business Health Score** using radial SVG animations. Displays real-time momentum badges, revenue trajectories, and granular 6-pillar breakdown diagnostics (Revenue, Customer Retention, Cash Flow, Inventory Turnover, Accounts Receivable, and AI Workflow adoption).
- **Actionable AI Recommendations (`AiRecommendationsGrid`)**: Synthesizes cross-pillar telemetry into prioritized recommendation cards (e.g., *"5 customers likely to churn"*, *"8 invoices overdue"*, *"Inventory running low"*). Features algorithmic confidence ratings (0–100%), estimated dollar business impact, category filter pills, and a one-click automated execution sequence with visual toast notifications.
- **Revenue Intelligence & Analytics (`RevenueAnalyticsChart`)**: Dual-mode Area & Bar charts powered by Recharts with instant toggles for Daily, Weekly, and Monthly horizons, contrast baseline tracking, repeat customer yield metrics, and natural language AI driver explanations.
- **Customer Intelligence & Churn Warning Center (`CustomerIntelligenceView`)**: Spotlights new vs. returning customers, top VIP account spenders, and an interactive **At-Risk Churn Table** featuring AI root-cause diagnoses and automated "Send Retention Offer" triggers.
- **Inventory & Replenishment Intelligence (`InventoryIntelligenceSection`)**: Evaluates inventory velocity, fastest-selling SKU turnover rates, dead stock holding cost calculations, and low stock depletion schedules with one-click "Approve PO" action triggers.
- **Cash Flow & Working Capital Schedule (`CashFlowOverview`)**: Analyzes total cash inflow vs. operating expenses to compute liquidity runway in months. Includes an actionable Accounts Receivable collection schedule with one-click automated dunning email dispatches.
- **Executive Command Briefing (`ExecutiveSummaryBriefing`)**: Daily high-contrast operational summary highlighting Today's Highlights, Top Opportunities ($ Yield), Top Risks, and AI-guided Next Best Actions.

---

## ⚙️ Backend Engine & API Architecture (.NET 9 / .NET 10 Clean Architecture)

The backend BI engine synthesizes multi-pillar business metrics into unified data transfer objects:
- **Automatic KPI & Health Score Calculation**: Analyzes Revenue trends, Customer Growth rates, Working Capital Cash Flow, Inventory velocity, and Overdue Payables.
- **AI Recommendation Synthesizer**: Generates proactive actionable insights complete with Title, Description, Priority, Category, Recommended Action, Confidence Score, and Expected Business Impact.

### REST API Endpoints
- `GET /api/business-intelligence/dashboard`: Complete consolidated dashboard snapshot covering all operational tiers.
- `GET /api/business-intelligence/recommendations`: Active AI recommendation cards with confidence scores and action parameters.
- `GET /api/business-intelligence/health`: Real-time 0–100 Business Health Score evaluation and dimensional pillar breakdown.
- `GET /api/business-intelligence/revenue`: Multi-period daily, weekly, and monthly revenue metrics and AI driver analysis.
- `GET /api/business-intelligence/customers`: Customer growth rates, churn risk evaluation list, and VIP customer rankings.
- `GET /api/business-intelligence/inventory`: Low stock reorder schedule, turnover rates, and dead inventory diagnostic suggestions.
- `GET /api/v1/bi/kpis`: List calculated enterprise KPIs (optional category filtering).
- `POST /api/v1/bi/kpis/recalculate`: Force automated real-time recalculation of all diagnostic telemetry.
