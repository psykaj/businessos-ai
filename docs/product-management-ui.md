# Product Management UI Architecture

## Overview
The Product Management UI allows SME business owners to manage product catalogs, custom SKUs, barcode/QR payloads, cost and selling prices, and reorder thresholds.

## Key Features & UI Components

### 1. Catalog Search & Filtering (`/dashboard/products`)
- Real-time search by product name, SKU, or barcode.
- Category dropdown filtering and 1-click "Low Stock Only" filter.
- Stock status badges: `In Stock`, `Low Stock`, `Out of Stock`.

### 2. Product Creation & Editing (`product-modal.tsx`)
- Form validation for SKU, product name, unit of measure (PCS, BOX, KG, L, SET), cost price, selling price, reorder point, safety stock, and max stock thresholds.
- Automatic SKU and barcode string generator.

### 3. Barcode & QR Scanner Label (`product-barcode-modal.tsx`)
- Displays printable product label with visual QR and EAN/UPC barcode representation.
- Base64 QR payload preview and 1-click copy & print functionality.

### 4. Detailed Product View (`/dashboard/products/[id]`)
- Product catalog metadata summary.
- Multi-warehouse stock level cards (`QuantityOnHand`, `QuantityReserved`, `QuantityAvailable`, `LocationBin`).
- Historical stock movement audit table.
