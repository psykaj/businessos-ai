# KPI Engine

## Overview
The KPI Engine aggregates data across all BusinessOS AI modules (Invoices, Leads, Deals) to calculate Key Performance Indicators.

## Supported KPIs
- **Revenue Growth**: Calculates month-over-month revenue growth using `Invoices`.
- **Lead Conversion Rate**: Calculates the percentage of `Leads` that turned into `Converted` status.
- **Average Deal Size**: Calculates the average amount of `Deals` in `Closed Won` stage.

## Background Jobs
- **KpiCalculationBackgroundJob**: Runs periodically (e.g., daily) using `IHostedService` to pre-calculate heavy KPIs and store them in the `KPIHistory` table. This ensures the frontend dashboards load instantly.

## Database Entities
- `KPI`: Stores the definition and current value of a KPI.
- `KPIHistory`: Stores daily/weekly snapshots of the KPI for trend analysis.
