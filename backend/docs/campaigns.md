# Campaigns Module

The Campaigns Module tracks marketing initiatives, budgets, and overall performance to determine ROI and lead attribution.

## Tracking & Analytics
- **Source & Medium**: Follows UTM standard conventions.
- **Budget Tracking**: Records allocated marketing spend per initiative.
- **Status Management**: Supports Draft, Scheduled, Running, and Completed statuses.

## Architecture
- `CampaignService`: Manages campaign lifecycles.
- Re-uses `Campaign` entity from previous Email/WhatsApp features but extends it with financial and attribution data (Source, Medium, Budget).
