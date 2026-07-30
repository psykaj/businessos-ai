# Executive Dashboard UI

## Overview
The Executive Dashboard (`/app/dashboard/executive`) serves as the central command center for business owners and executives. It provides an immediate, high-level overview of the company's performance, health, and strategic direction.

## Key Features
- **Health Gauge**: A prominent visualization of the overall Business Health score (0-100).
- **Top-level KPIs**: Displays the most critical metrics such as Revenue Growth, Lead Conversion Rate, and Active Goals.
- **AI Executive Insights**: A feed of automated, prioritized insights pointing out anomalies or trends. Includes business impact estimates and confidence levels.
- **Strategic Goals Summary**: Progress bars showing the status of top-level objectives.

## Technical Implementation
- Built with React, Tailwind CSS, and Shadcn UI.
- Data fetching via custom React Query hooks (`useBusinessHealth`, `useExecutiveInsights`, `useKpis`).
- The `HealthGauge` component utilizes Recharts `PieChart` to create a beautiful half-doughnut gauge.
