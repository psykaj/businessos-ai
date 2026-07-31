# AI Automation Studio

BusinessOS AI Automation Studio is a powerful, low-code/no-code workflow automation engine designed to save business owners time, reduce operational costs, and streamline operations. It allows users to connect various modules (CRM, Finance, Inventory) through automated rules, replacing repetitive manual tasks with automated sequences.

## Core Concepts

*   **Automation Workflow:** The container for a set of triggers, conditions, and actions.
*   **Trigger:** The event that starts the workflow. Triggers can be event-based (e.g., Lead Created) or schedule-based (e.g., Every Monday at 9 AM).
*   **Condition:** Rules that evaluate data to determine if the workflow should continue. Allows for branching (If/Else).
*   **Action:** Tasks executed by the engine when conditions are met. E.g., Send an email, update CRM, create an invoice.
*   **Execution Log:** A granular, step-by-step audit of what happened during a workflow execution, useful for debugging.

## Architecture

The Automation Studio is built using Clean Architecture principles within the `backend/Modules/AutomationStudio/` folder. It relies heavily on:
- Entity Framework Core for data persistence.
- Background Services (Workers) for schedule-based triggers.
- Dependency Injection for modular action/condition execution.

For details on the execution flow, see [Workflow Engine](workflow-engine.md).
