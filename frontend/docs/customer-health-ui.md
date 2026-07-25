# Customer Health UI Documentation

## Overview
The Customer Health UI displays health score breakdowns and automated churn risk assessments across all business accounts.

## Interface Components
1. **Risk Category Metric Cards**: Clickable cards filtering by `Healthy`, `Stable`, `Needs Attention`, and `High Risk`.
2. **Search & Filter Bar**: Instant search input filtering customer accounts by name or risk status.
3. **Account Health Table**: Interactive data table displaying live health scores, risk badges, lifetime value, support ticket counts, CSAT ratings, last interaction dates, and quick links to Customer 360° profiles.
4. **Bulk Recalculation Button**: Triggers `recalculateAllHealth()` mutation to re-evaluate telemetry across all active accounts.

## Component File
- `frontend/app/dashboard/customer-health/page.tsx`
- `frontend/components/customer-success/health-score-badge.tsx`
