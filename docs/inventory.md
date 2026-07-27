# Inventory Management Subsystem

## Overview
The Inventory Management subsystem of BusinessOS AI provides multi-warehouse product cataloging, automated stock balance tracking, SKU & Barcode/QR code payload parsing, and smart stock level optimizations tailored for SMEs.

## Key Architecture & Features

### 1. Product Catalog & Categories
- **SKU & Barcode/QR Engine**: Supports custom SKUs, EAN/UPC barcodes, and base64 JSON QR payloads (`BIZOS:PROD:...`).
- **Hierarchical Categories**: Parent-child product category trees.
- **Stock Thresholds**: Configurable Reorder Point, Safety Stock, Reorder Quantity, Minimum, and Maximum Stock Levels.

### 2. Multi-Warehouse Stock Engine
- Dynamically tracks:
  - `QuantityOnHand`: Total physical stock in the warehouse.
  - `QuantityReserved`: Stock allocated to open orders.
  - `QuantityAvailable`: Computed as `QuantityOnHand - QuantityReserved`.
- Primary warehouse assignment and location bin tracking (`LocationBin`).

### 3. API Endpoints
- `GET /api/inventory/products`: Search and filter product catalog.
- `GET /api/inventory/products/{id}`: Fetch product by ID with warehouse stock breakdown.
- `GET /api/inventory/products/barcode/{code}`: Lookup product by Barcode, QR Code payload, or SKU.
- `POST /api/inventory/products`: Create a new product.
- `PUT /api/inventory/products/{id}`: Update product configuration.
- `POST /api/inventory/products/{id}/archive`: Soft archive product.
- `GET /api/inventory/categories`: Fetch category hierarchy.
- `POST /api/inventory/categories`: Create category.
- `GET /api/inventory/warehouses`: List all active warehouses.
- `POST /api/inventory/warehouses`: Add a new warehouse facility.
