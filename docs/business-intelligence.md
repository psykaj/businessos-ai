# BusinessOS AI – Business Intelligence Backend Engine

The **Business Intelligence (BI) Engine** in BusinessOS AI provides automated, real-time analytics across all core business operations, aggregating metrics from CRM, Marketing Automation, Workflows, Billing, QR Management, and AI Usage into actionable business intelligence.

## Features & Core Modules
- **Automatic KPI Calculation Engine**: Calculates 14 enterprise KPIs in real time.
- **Executive Dashboard API Layer**: Dedicated views for CEO, Sales, Marketing, Finance, Operations, and Team Performance.
- **AI Business Insights Engine**: Heuristic and predictive recommendation engine surfacing business risks and opportunities.
- **Predictive Forecasting Engine**: Statistical and ML-ready projections for Revenue, Sales, Leads, Customer Growth, and Subscriptions.
- **Report Generator & Multi-Format Exporter**: On-demand report creation with native CSV, Excel, and PDF exports.
- **Goal Tracking Framework**: Tracks organizational objectives against live calculated metrics.

## API Endpoints
- `GET /api/v1/bi/kpis`: List calculated KPIs (optional category filter)
- `GET /api/v1/bi/kpis/{name}`: Get specific KPI details
- `POST /api/v1/bi/kpis/recalculate`: Force automated recalculation of all KPIs
