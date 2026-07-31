# Workflow Execution Engine

The Workflow Execution Engine is the heart of the AI Automation Studio. It is responsible for processing triggered workflows, evaluating conditions, and executing actions in a reliable and auditable manner.

## Components

1.  **IWorkflowExecutionEngine:** The primary service responsible for fetching workflow definitions and executing them sequentially.
2.  **ITriggerService:** Listens for events across the system and queues the corresponding workflows for execution.
3.  **IConditionService:** Evaluates JSON-based condition configurations against a runtime context payload.
4.  **IActionService:** Parses action configurations and interacts with other BusinessOS modules to perform the task.
5.  **ScheduleWorker:** A .NET BackgroundService that polls for due scheduled workflows and triggers them via the `IWorkflowExecutionEngine`.

## Execution Flow

1.  An event occurs in the system (e.g., `LeadCreated`).
2.  `TriggerService` finds all active workflows for the tenant that use the `LeadCreated` trigger.
3.  `TriggerService` passes the workflow ID and context payload to the `WorkflowExecutionEngine`.
4.  The Engine creates a new `WorkflowExecution` record.
5.  The Engine evaluates all `Conditions`. If any condition fails, execution stops, and the status is updated.
6.  The Engine executes all `Actions`. Execution logs are written for each step.
7.  The final status (Completed/Failed) is updated in the database.
