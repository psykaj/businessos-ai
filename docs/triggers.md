# Supported Triggers

The Automation Studio supports a wide variety of triggers to initiate workflows automatically.

## Core Triggers

*   **Schedule-Based:** Execute workflows at specific intervals (e.g., daily, weekly) or exact dates using Cron expressions.
*   **Webhook Received:** Trigger workflows when an external system sends a POST request to a generated webhook URL.
*   **ManualTrigger:** Triggered manually via API or dashboard button.

## CRM Triggers
*   **Lead Created:** Triggered when a new lead is added to the CRM.
*   **Lead Updated:** Fired when lead properties are updated.
*   **Lead Qualified:** Fired when lead score crosses qualification threshold.
*   **Customer Created:** Triggered when a new customer profile is created.
*   **Deal Stage Changed:** Triggered when a sales deal moves to a new stage.

## Financial Triggers
*   **Invoice Paid:** Triggered when an invoice's status changes to Paid.
*   **Invoice Overdue:** Triggered when the current date passes an invoice's due date.
*   **Payment Received:** Triggered when a new payment is logged.
*   **Payment Failed:** Fired when a payment attempt fails.
*   **Subscription Expiring:** Fired near subscription expiry.

## Operations Triggers
*   **Inventory Low:** Triggered when product stock drops below a predefined threshold.
*   **Document Signed:** Triggered when an e-signature request is completed.
*   **Form Submitted:** Triggered when a lead capture form is submitted.
*   **QR Scan:** Triggered when a customer scans a tracking QR code.
*   **Workflow Completed:** Triggered when another workflow finishes execution (for chaining workflows).
*   **Campaign Completed:** Fired when a marketing campaign finishes.
*   **Team Member Added:** Fired when a team member joins the org.

## Dispatcher API

`POST /api/workflows/triggers/test-dispatch?triggerType=LeadCreated`
Dispatch a trigger payload across all matching active workflows for the tenant.
