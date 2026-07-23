# Executive Dashboard UI Architecture

The **Executive Dashboard** (`/dashboard/executive`) provides a CEO-grade command center that consolidates high-level metrics, revenue growth trends, team productivity, and AI recommendations into a single customizable dashboard view.

## Features
- **Multi-Role View Switcher**:
  - CEO Command: Revenue growth velocity, inbound leads, active customer health score, top AI strategic action items.
  - Sales Pipeline: Total pipeline value, won deals, win rate %, average deal size, sales cycle length, rep rankings.
  - Marketing ROI: Lead acquisition volume, average campaign ROI %, QR scan volume, campaign performance table.
  - Finance & MRR: Monthly Recurring Revenue (MRR), Annual Recurring Revenue (ARR), total outstanding invoices, subscription churn rate.
  - Operations & Workflows: Active workflows, execution volume, AI requests processed, connected integration status.
  - Team Performance: Team activity logs, closed deals, revenue generated per team member.
- **Time Horizon Filter**: Quick switching between `7d`, `30d`, `90d`, `1y` with percentage comparison badges.
- **Recharts Visualizations**: Interactive area charts and trend graphs.
- **Draggable & Customizable Layout**: Allows business owners to toggle customization mode and adjust layout hierarchy.
