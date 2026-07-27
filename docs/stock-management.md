# Stock Operations, Adjustments & Audit Engine

## Overview
The Stock Management subsystem maintains an immutable audit trail for all physical inventory operations across warehouses, physical count reconciliations, and damage reporting.

## Operations Overview

### 1. Stock Movement Types
- **StockIn**: Manual stock addition or opening balance entry.
- **StockOut**: Stock issue, sales dispatch, or manual reduction.
- **Transfer**: Inter-warehouse stock transfers. Atomically deducts stock from source warehouse and credits destination warehouse.
- **Adjustment**: Physical count adjustments and variance reconciliation.
- **Damage / Loss / Spoilage**: Formal write-offs for damaged or expired inventory.

### 2. Physical Count & Reconciliation Workflow
1. Create a `StockAdjustment` with type `PhysicalCount` or `Damage`.
2. Enter system quantity vs actual physical count.
3. Review variance (`VarianceQuantity = Actual - System`).
4. Approving the adjustment updates warehouse stock levels and creates immutable movement log entries.

### 3. API Endpoints
- `GET /api/inventory/stock/movements`: Query stock movement audit trail with date/warehouse/product filters.
- `POST /api/inventory/stock/in`: Perform manual Stock In.
- `POST /api/inventory/stock/out`: Perform manual Stock Out.
- `POST /api/inventory/stock/transfer`: Execute Inter-warehouse stock transfer.
- `POST /api/inventory/stock/reconcile`: Submit physical count reconciliation.
- `GET /api/inventory/adjustments`: List physical count & damage adjustments.
- `POST /api/inventory/adjustments`: Draft stock adjustment.
- `POST /api/inventory/adjustments/{id}/approve`: Approve adjustment & post stock variance.
