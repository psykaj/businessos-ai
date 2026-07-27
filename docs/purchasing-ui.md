# Purchasing Workspace UI Architecture

## Overview
The Purchasing Workspace handles purchase order lifecycles, supplier ordering, approval workflows, and receiving physical goods shipments into warehouse stock.

## Key Features & UI Components

### 1. Purchase Order Management (`/dashboard/purchase-orders`)
- Status tabs: `All Orders`, `Draft`, `Approved`, `Partially Received`, `Fully Received`.
- Dynamic status badges and financial summaries.

### 2. Purchase Order Creator (`purchase-order-modal.tsx`)
- Dynamic line-item rows: product selection, order quantity, unit cost, line total calculations.
- Automatic subtotal, configurable tax (18%), shipping cost, and grand total calculations.

### 3. Goods Receipt Note (GRN) Processing (`receive-goods-modal.tsx`)
- Interactive GRN entry modal displaying remaining expected quantities.
- Input fields for quantity received vs quantity rejected with rejection reasons.
- Submitting automatically increments warehouse inventory stock, posts immutable stock movement logs, and updates PO status.
