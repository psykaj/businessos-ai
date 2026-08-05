# Multi-Location & Warehouse Architecture

## Geographic Locations & Hierarchies
The Multi-Location platform introduces structured regional categorization. Each business branch is mapped to a `Location` entity that records:
- Country & State / Province
- City & Postal Code
- Regional Territory (e.g., "EMEA", "North America", "APAC", or "US-West")
- Operational Timezone

## Branch Warehouses
To ensure separation from item-level SKU inventories while empowering branch operations, Day 26 establishes **BranchWarehouses**:
- **Storage Capacity**: Tracked in square feet (`StorageCapacitySqFt`).
- **Utilization & Value**: Automatically recalculated upon receipt of stock shipments (`CurrentUtilizationPercentage`, `TotalStockItemsCount`, `EstimatedStockValue`).
- **Primary Warehouse Assignment**: Every branch can flag a primary dispatch warehouse.

## Inter-Warehouse Transfers Workflow
1. **Request**: `POST /api/v1/transfers` initiates a transfer order between source and destination warehouses with line items.
2. **Automated vs. Manual Approval**: If the requester is an active `BranchManager` for the source branch with approval rights and the transaction total falls below their `MaxTransferApprovalLimit`, the transfer is instantly auto-approved. Otherwise, it queues for review (`PUT /api/v1/transfers/{id}/approval`).
3. **Transit Tracking**: Updates status to `InTransit` and records carrier logs (`PATCH /api/v1/transfers/{id}/tracking`).
4. **Receipt & Reconciliation**: Marking a transfer received (`POST /api/v1/transfers/{id}/receive`) automatically decrements stock counts and valuations from the source warehouse and credits the destination warehouse.
