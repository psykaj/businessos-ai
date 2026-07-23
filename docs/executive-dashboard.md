# Executive Dashboard Architecture

The **Executive Dashboard Engine** exposes specialized, role-tailored REST APIs that empower business leaders to make informed data-driven decisions without jumping across multiple analytics tools.

## Dashboards Provided
1. **CEO Dashboard** (`GET /api/v1/bi/dashboard/ceo`): High-level business overview, revenue growth trends, top insights, and active goals.
2. **Sales Dashboard** (`GET /api/v1/bi/dashboard/sales`): Pipeline value, won deals, sales win rate, sales cycle length, rep performance rankings.
3. **Marketing Dashboard** (`GET /api/v1/bi/dashboard/marketing`): Lead volume, lead conversion rates, campaign ROI, QR scan volume.
4. **Finance Dashboard** (`GET /api/v1/bi/dashboard/finance`): MRR, ARR, cash flow, outstanding invoices, churn rates.
5. **Operations Dashboard** (`GET /api/v1/bi/dashboard/operations`): Active workflows, execution volumes, AI usage metrics, connected integrations.
6. **Team Performance Dashboard** (`GET /api/v1/bi/dashboard/team`): Activity logs, deal breakdown, revenue per team member.

## Query & Filter Support
- **Period Filter**: `7d`, `30d`, `90d`, `1y`, `custom`
- **Date Range**: `StartDate` and `EndDate`
- **Comparison Periods**: Evaluates current performance against previous equal timeframes.
