# Regional Reporting & Branch Performance Engine

## Overview
BusinessOS AI includes a real-time analytics layer that synthesizes financial, inventory, customer, and workforce data across multi-branch organizations.

## Regional Reports APIs (`/api/v1/regional-reports`)
All regional summary queries utilize Redis distributed caching (30-minute expiration) to guarantee sub-millisecond API response times even across massive organizational data sets:
1. **Revenue by Branch**: `GET /revenue-by-branch?year=2026&month=8&region=EME` - Returns branches sorted by top revenue with MoM growth rates.
2. **Profit by Branch**: `GET /profit-by-branch` - Calculates net profitability and profit margins.
3. **Inventory by Branch**: `GET /inventory-by-branch` - Summarizes warehouse count, total items, inventory valuation, and average space utilization.
4. **Customer Density**: `GET /customer-count` - Analyzes revenue yield per active branch customer.
5. **Employee Productivity**: `GET /employee-count` - Tracks workforce distribution and calculates revenue per employee.
6. **Executive Branch Ranking**: `GET /branch-ranking` - Produces an executive leaderboard sorted by composite performance scores.

## Branch Performance Engine (`/api/v1/branch-analytics`)
An automated analytical engine (`BranchPerformanceEngineService`) and Background Hosted Worker (`BranchPerformanceWorkerJob`) continuously assess multi-location health:
- **Composite Scoring Formula**: Combines operational profit margin (40% weight), warehouse capacity utilization (30% weight), and month-over-month revenue growth momentum (30% weight).
- **Best & Lowest Performer Flags**: Automatically highlights organizational champions and identifies branches requiring executive intervention or operational restructuring.
- **Monthly Comparison**: `GET /api/v1/branch-analytics/comparison` provides immediate variance reporting against historical performance periods.
