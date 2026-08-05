# Multi-Branch Hubs & Locations UI (`/dashboard/branches` & `/dashboard/locations`)

## Architecture & UX
The Multi-Branch Management UX is engineered for distributed corporate enterprises, drawing inspiration from Oracle NetSuite and Microsoft Dynamics 365.
- **Real-Time Hub Monitoring**: Visual KPI strip tracking Total Branches, Active vs. Maintenance status, Consolidated Revenue, and Net Profit Margins.
- **Executive Leaderboard**: Highlights the #1 Top Performance Hub based on an automated composite scoring algorithm (Profit Margin + Capacity Utilization + MoM Growth).
- **Geographic Hierarchy Explorer**: `/dashboard/locations` maps branches into regional continental territories (EMEA, North America, APAC, LATAM) with live timezone classifications.

## Component Design
- **`BranchCard`**: Reusable interactive KPI card displaying operational status badges, manager contact cards, storage capacity progress bars, and instant action dropdowns.
- **Zod-Validated Modals**: `CreateBranchModal`, `AssignManagerModal`, and `WorkingHoursModal` enforce corporate compliance with zero refresh page updates via React Query optimistic mutations.
