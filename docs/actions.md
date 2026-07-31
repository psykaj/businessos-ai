# Supported Actions

The Action Engine executes tasks across BusinessOS AI modules when workflow conditions are met.

## Core Actions

*   **Send Email:** Send templated or custom emails to customers/leads.
*   **Send Notification:** Trigger an in-app or push notification to a team member.
*   **Trigger Webhook:** Send a POST request to an external URL with context data.
*   **Execute Another Workflow:** Chaining workflows together.

## CRM Actions
*   **Create Task:** Automatically assign a follow-up task to a sales rep.
*   **Update CRM Property:** Update fields on a Lead, Deal, or Company (e.g., change stage).
*   **Assign Lead:** Round-robin or conditionally assign a lead to a specific owner.

## Financial Actions
*   **Create Invoice:** Generate a draft or finalized invoice.
*   **Award Loyalty Points:** Credit a customer's loyalty account.
*   **Generate Report:** Automatically compile and send a PDF financial report.

## Custom Actions
Actions are executed sequentially by the `IActionService`. If any action fails, the execution can be halted or retried based on the workflow settings.
