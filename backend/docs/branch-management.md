# Branch Management & Multi-Location Platform (Day 26)

## Overview
The Branch Management platform is an enterprise-grade solution designed to scale from single-location startups to distributed multi-branch corporate networks. Inspired by Oracle NetSuite and Microsoft Dynamics 365, it introduces robust tenancy isolation, centralized control, and branch autonomy.

## Architecture
The system is divided into 7 cleanly separated enterprise modules under `backend/Modules/`:
1. **Locations**: Geographic classification, country/state hierarchies, timezones, and regional grouping.
2. **Branches**: Core operational branches with contact information, cost center codes, status management, and primary flags.
3. **BranchSettings**: Configurable operational business hours (`WorkingHoursJson`) and real-time operational limits (`OperationalSettingsJson`).
4. **Warehouses**: Branch-specific warehouse mapping (`BranchWarehouses` table) tracking capacity (SqFt), utilization percentages, total stock count, and valuation.
5. **Transfers**: Inter-warehouse inventory transfers with automated manager approval routing and real-time tracking notes.
6. **RegionalReports**: Comprehensive aggregated revenue, profitability, inventory density, and customer/workforce density metrics across regions.
7. **BranchAnalytics (Performance Engine)**: Composite operational scoring, automated ranking, Best vs. Lowest performer flagging, and MoM growth analysis.

## Core Features & APIs
- **Branch CRUD & Status**: `POST /api/v1/branches`, `PATCH /api/v1/branches/{id}/status`
- **Manager Assignment & Approval Thresholds**: `POST /api/v1/branches/{id}/managers` (Configures transfer approval rights and max financial limit).
- **Branch Settings**: `GET/PUT /api/v1/branch-settings/{branchId}` with Redis caching.

## Business Value
- **Save Time**: Instant regional views without manual spreadsheet consolidation.
- **Reduce Operational Costs**: Automated approval routing prevents unauthorized stock shrinkage.
- **Improve Decision-Making**: Real-time performance ranking immediately highlights profitable and underperforming operations.
