# Multi-Branch Warehouses & Transfer Center UI (`/dashboard/warehouses` & `/dashboard/transfers`)

## Multi-Branch Storage Network (`/dashboard/warehouses`)
Upgraded to support Day 26 Multi-Branch Warehouse capabilities:
- **Capacity Tracking**: Visualizes facility storage capacity in Square Feet (SqFt) with automated usage percentage progress bars.
- **Stock Valuation**: Real-time dollar valuation sync ($2.45M+) with automated alerts when facilities near capacity constraints (>85%).
- **Primary Hub Designation**: Clear visual ribbons indicating default dispatch headquarters.

## Inter-Warehouse Transfers Workflow (`/dashboard/transfers`)
An interactive logistics command center controlling inventory movement across branches:
- **Status Pipeline Filters**: Categorizes transfers across Pending Approval, In Transit & Shipped, Received & Inducted, and Rejected orders.
- **Automated Manager Threshold Engine**: Transfers below an authorized Manager's financial threshold are highlighted with an instantaneous **"⚡ Auto-Approved by Manager Threshold"** badge.
- **Two-Step Induction & Reconciliation**: Managers can authorize shipments with notes (`ApproveTransferModal`), and receiving facility leads can confirm arrival (`ReceiveTransferModal`)—which automatically reconciles stock valuations between source and destination warehouses in real-time.
