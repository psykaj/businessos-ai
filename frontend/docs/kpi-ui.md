# KPI Workspace UI

## Overview
The KPI Workspace (`/app/dashboard/kpis`) allows executives to dive deeper into their metrics, view trends over time, and compare internal performance against external industry standards.

## Key Features
- **Metric Cards**: Standardized cards displaying current values, targets, and percentage trends.
- **Industry Benchmarks Table**: A clear tabular view comparing the company's KPI values against industry averages and top quartile performers. Includes dynamic status badges (e.g., "Top Quartile", "Below Average").

## Technical Implementation
- Built with standard Shadcn UI components (`Card`, `Table`, `Badge`).
- Data is managed by `useKpis` and `useBenchmarks` custom hooks.
- Status badges dynamically calculate color based on whether the current value exceeds the industry average or top quartile.
