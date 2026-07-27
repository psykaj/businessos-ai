# Inventory Center Frontend Architecture

## Overview
The Inventory Center frontend for BusinessOS AI provides a command dashboard for tracking total product catalog valuation, stock risks (Low stock, Out of stock, Overstock), reorder suggestions, and product movement velocity.

## Key Features & UI Components

### 1. KPI Metric Cards (`inventory-kpi-cards.tsx`)
- Displays real-time counts for Total Products, Combined Asset Valuation ($), Low Stock Items, Out-of-Stock Items, Pending Purchase Orders, and Overstock Items.
- Gradient border cards with responsive grid breakdown.

### 2. Velocity & Risk Analytics (`inventory-stock-chart.tsx`)
- Recharts Bar Chart showing top fast-moving products by movement quantity and total cost valuation over 30 days.
- Recharts Pie Chart depicting stock risk breakdown.

### 3. Quick Actions & Reorder Planning Engine
- 1-Click modal launch for Purchase Order issuance (`PurchaseOrderModal`).
- 1-Click modal launch for Stock operations and transfers (`StockOperationModal`).
- Reorder suggestion table listing stock gaps with recommended purchase order quantities.

## Page Route
- `/dashboard/inventory`: Main Inventory Command Center.
