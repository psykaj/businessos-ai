# Supplier Management & Vendor Performance Platform

## Overview
The Supplier Subsystem tracks vendor profiles, contact info, tax IDs, payment terms, and automatically calculates real-time supplier performance ratings.

## Vendor Performance Scoring Engine
Supplier rating scores (0 - 100%) are calculated dynamically based on:
1. **On-Time Delivery Rate (OTD)**: Ratio of orders delivered within expected delivery dates.
2. **Fulfillment Rate**: Ratio of accepted vs ordered items across all fulfilled purchase orders.

$$ \text{Performance Score} = \left( \frac{\text{On-Time Deliveries}}{\text{Total Orders}} \right) \times 100 $$

## Supplier Portal Integration Hooks
Architected for external vendor self-service:
- Suppliers can view assigned Purchase Orders.
- Confirm estimated delivery dates.
- Export PO pdf documents and invoice details.

## API Endpoints
- `GET /api/suppliers`: Search and list suppliers.
- `GET /api/suppliers/{id}`: Get supplier profile details.
- `GET /api/suppliers/{id}/performance`: Retrieve calculated OTD and fulfillment metrics.
- `POST /api/suppliers`: Register a new vendor.
- `PUT /api/suppliers/{id}`: Update supplier contact or payment terms.
