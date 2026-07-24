# AI Recommendations Engine

## Overview
The **AI Recommendation Engine** automatically analyzes organization business metrics to generate actionable, revenue-driving recommendations for business owners.

## Recommendation Capabilities
1. **Inactive Lead Re-engagement**: Detects leads with no activity for 14+ days and recommends sales follow-ups or automated re-engagement workflows.
2. **Outstanding Receivable Management**: Monitors pending and overdue invoices, calculating total outstanding balances and recommending automated WhatsApp/Email reminders.
3. **Marketing Campaign Optimization**: Scans for active marketing campaigns and highlights missing lead acquisition channels.

## Entity Structure
- **Recommendation**:
  - `Id`, `OrganizationId`, `Category`, `Title`, `Description`, `Priority` (High/Medium/Low), `Reason`, `SuggestedAction`, `IsApplied`, `AppliedAt`, `CreatedAt`.

## API Endpoints
- `GET /api/ai-agent/recommendations`: Retrieve active organization recommendations (with pagination, priority, and category filtering).
- `POST /api/ai-agent/recommendations/analyze`: Trigger a real-time data analysis scan to generate new recommendations.
- `POST /api/ai-agent/recommendations/{id}/apply`: Mark a recommendation as applied/dismissed.
